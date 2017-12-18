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
        public byte? SubType { get; } = null;

        public PacketTypeAttribute(byte type) => Type = type;

        public PacketTypeAttribute(byte type, byte subType) : this(type) => SubType = subType;
    }
}
