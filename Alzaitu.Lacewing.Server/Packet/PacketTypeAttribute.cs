using System;

namespace Alzaitu.Lacewing.Server.Packet
{
    internal class PacketTypeAttribute : Attribute
    {
        /// <summary>
        /// The type of the packet.
        /// </summary>
        public byte Type { get; }

        /// <summary>
        /// The subtype of the packet.
        /// </summary>
        public byte? SubType { get; }

        /// <summary>
        /// If true, this packet can be written.
        /// </summary>
        public bool CanWrite { get; }
        /// <summary>
        /// If true, this packet can be read.
        /// </summary>
        public bool CanRead { get; }

        public PacketTypeAttribute(byte type, bool canWrite, bool canRead)
        {
            Type = type;
            CanWrite = canWrite;
            CanRead = canRead;
        }

        public PacketTypeAttribute(byte type, byte subType, bool canWrite, bool canRead) : this(type, canWrite, canRead) => SubType = subType;
    }
}
