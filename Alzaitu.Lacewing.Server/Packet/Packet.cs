using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using Alzaitu.Lacewing.Server.Packet.Serialization;

namespace Alzaitu.Lacewing.Server.Packet
{
    internal abstract class Packet
    {
        private static Dictionary<byte, Dictionary<int, Type>> _typeReadMap;
        private static Dictionary<byte, Dictionary<int, Type>> TypeReadMap =>
            _typeReadMap ?? (_typeReadMap = typeof(Packet).Assembly.GetTypes()
                .Where(x => typeof(Packet).IsAssignableFrom(x) &&
                            !x.IsAbstract && !x.IsInterface &&
                            x.GetCustomAttribute<PacketTypeAttribute>()?.CanRead == true)
                .GroupBy(x => x.GetCustomAttribute<PacketTypeAttribute>().Type)
                .ToDictionary(x => x.Key,
                    x => x.ToDictionary(y => y.GetCustomAttribute<PacketTypeAttribute>().SubType ?? -1, y => y)));

        public byte Type => GetType().GetCustomAttribute<PacketTypeAttribute>().Type;
        public byte? SubType => GetType().GetCustomAttribute<PacketTypeAttribute>().SubType;

        /// <summary>
        /// Unused by the protocol. Used to distinguish packet subtypes.
        /// </summary>
        public byte Variant { get; set; }

        public bool CanWrite => GetType().GetCustomAttribute<PacketTypeAttribute>().CanWrite;
        public bool CanRead => GetType().GetCustomAttribute<PacketTypeAttribute>().CanRead;

        public void Write(Stream stream)
        {
            var s = new LacewingSerializer(GetType());

            var siz = s.GetSize(this);

            if (SubType != null)
                siz += sizeof(byte);

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

            if(SubType != null)
                wrt.Write(SubType.Value);

            s.Write(this, stream);
            //WriteImpl(new BinaryWriter(new WindowingStream(stream, siz - sizeof(byte))));
        }

        protected virtual void WriteImpl(BinaryWriter wrt) => throw new InvalidOperationException("This packet does not support writing.");
        protected virtual void ReadImpl(BinaryReader rdr, long size) => throw new InvalidOperationException("This packet does not support reading.");

        /// <summary>
        /// Retrive the size of the packet, in bytes.
        /// </summary>
        /// <returns>A number between 0 and 4294967295 describing the length of the packet in bytes.</returns>
        public long GetSize() => new LacewingSerializer(GetType()).GetSize(this);

        public static Packet ReadPacket(Stream stream, bool initialByte)
        {
            var rdr = new BinaryReader(stream);

            if(initialByte && rdr.ReadByte() != 0)
                throw new InvalidDataException("The server can only handle non-HTTP clients.");

            var type = rdr.ReadByte();

            if(!TypeReadMap.TryGetValue((byte) ((type >> 4) & 0xF), out var packetTypeBlock))
                throw new InvalidDataException($"The packet read from the stream ({(type >> 4) & 0xF}.{type & 0xF}) does not have an associated type.");

            var size = (long)rdr.ReadByte();
            if (size == 254)
                size = rdr.ReadUInt16();
            else if (size == 255)
                size = rdr.ReadUInt32();

            Type packetType;
            if (packetTypeBlock.ContainsKey(-1))
                packetType = packetTypeBlock[-1];
            else
            {
                var subType = rdr.ReadByte();
                if(!packetTypeBlock.TryGetValue(subType, out packetType))
                    throw new InvalidDataException($"The packet read from the stream ({(type >> 4) & 0xF}.{type & 0xF}.{subType}) does not have an associated type.");
                size -= sizeof(byte);
            }

            var instance = (Packet) Activator.CreateInstance(packetType);
            instance.Variant = (byte)(type & 0xF);
            new LacewingSerializer(packetType).Read(instance, size, stream);

            return instance;
        }

    }
}
