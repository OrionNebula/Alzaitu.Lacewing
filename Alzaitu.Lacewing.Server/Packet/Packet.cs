using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using Alzaitu.Lacewing.Server.Util;

namespace Alzaitu.Lacewing.Server.Packet
{
    internal abstract class Packet
    {
        private static Dictionary<byte, Dictionary<byte?, Type>> _typeMap;

        private static Dictionary<byte, Dictionary<byte?, Type>> TypeMap =>
            _typeMap ?? (_typeMap = typeof(Packet).Assembly.GetTypes()
                .Where(x => typeof(Packet).IsAssignableFrom(x) &&
                            !x.IsAbstract && !x.IsInterface &&
                            x.GetCustomAttribute<PacketTypeAttribute>() !=
                            null).GroupBy(x => x.GetCustomAttribute<PacketTypeAttribute>().Type)
                .ToDictionary(x => x.Key,
                    x => x.ToDictionary(y => y.GetCustomAttribute<PacketTypeAttribute>().SubType, y => y)));

        public byte Type => GetType().GetCustomAttribute<PacketTypeAttribute>().Type;

        /// <summary>
        /// Unused by the protocol. Used to distinguish packet subtypes.
        /// </summary>
        public byte Variant { get; set; }

        public abstract bool CanWrite { get; }
        public abstract bool CanRead { get; }

        public void Write(Stream stream)
        {
            var siz = GetSize();
            if(siz > 4294967295)
                throw new InvalidOperationException("Cannot send a message that's more than 4294967295 bytes long.");

            var wrt = new BinaryWriter(stream);

            wrt.Write((byte)(((Type & 0xF) << 4) | Variant & 0xF));

            if (siz < 254)
                wrt.Write((byte) siz);
            else if (siz < 65535)
            {
                wrt.Write((byte) 254);
                wrt.Write((ushort) siz);
            }
            else
            {
                wrt.Write((byte) 255);
                wrt.Write((uint) siz);
            }

            WriteImpl(new BinaryWriter(new WindowingStream(stream, siz)));
        }

        protected virtual void WriteImpl(BinaryWriter wrt) => throw new InvalidOperationException("This packet does not support writing.");
        protected virtual void ReadImpl(BinaryReader rdr, long size) => throw new InvalidOperationException("This packet does not support reading.");

        /// <summary>
        /// Retrive the size of the packet, in bytes.
        /// </summary>
        /// <returns>A number between 0 and 4294967295 describing the length of the packet in bytes.</returns>
        public abstract long GetSize();

        public static Packet ReadPacket(Stream stream)
        {
            var rdr = new BinaryReader(stream);

            var type = rdr.ReadByte();

            var packetTypeBlock = TypeMap[(byte)((type >> 4) & 0xF)];

            var size = (long)rdr.ReadByte();
            if (size == 254)
                size = rdr.ReadUInt16();
            else if (size == 255)
                size = rdr.ReadUInt32();

            var packetType = packetTypeBlock.ContainsKey(null) ? packetTypeBlock[null] : packetTypeBlock[rdr.ReadByte()];

            if (packetType == null)
                throw new InvalidDataException($"The packet read from the stream ({(type >> 4) & 0xF}.{type & 0xF}) does not have an associated type.");

            var instance = (Packet) Activator.CreateInstance(packetType);
            instance.Variant = (byte)(type & 0xF);
            instance.ReadImpl(new BinaryReader(new WindowingStream(stream, size)), size);

            return instance;
        }

    }
}
