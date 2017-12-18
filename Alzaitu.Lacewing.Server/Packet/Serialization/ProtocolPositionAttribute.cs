using System;

namespace Alzaitu.Lacewing.Server.Packet.Serialization
{
    [AttributeUsage(AttributeTargets.Property)]
    internal class ProtocolPositionAttribute : Attribute
    {
        public uint Position { get; }

        public bool EmitOnSuccess { get; }

        public bool EmitOnFailure { get; }

        public bool EmitLengthPrefix { get; }

        public ProtocolPositionAttribute(uint position, bool emitOnSuccess = true, bool emitOnFailure = true, bool emitLengthPrefix = false)
        {
            Position = position;
            EmitOnSuccess = emitOnSuccess;
            EmitOnFailure = emitOnFailure;
            EmitLengthPrefix = emitLengthPrefix;
        }
    }
}
