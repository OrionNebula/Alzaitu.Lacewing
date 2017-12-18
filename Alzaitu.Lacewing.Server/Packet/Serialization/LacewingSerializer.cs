using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Text;
using Alzaitu.Lacewing.Server.Packet.Response;

namespace Alzaitu.Lacewing.Server.Packet.Serialization
{
    internal class LacewingSerializer
    {
        private readonly Type _type;

        public LacewingSerializer(Type type)
        {
            _type = type;
        }

        public void Write(object graph, Stream stream)
        {
            var wrt = new BinaryWriter(stream, Encoding.UTF8);

            if(graph == null)
                throw new ArgumentNullException(nameof(graph), "Cannot serialize a null packet.");

            foreach (var super in InheritPath().Reverse())
            {
                var successProp = (bool?) super.GetProperty(nameof(PacketResponse.Success),
                    BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance)?.GetValue(graph);

                foreach (var prop in super.GetProperties(BindingFlags.Public | BindingFlags.NonPublic |
                                                         BindingFlags.DeclaredOnly | BindingFlags.Instance)
                    .Where(x => x.GetCustomAttribute<ProtocolPositionAttribute>() != null)
                    .OrderBy(x => x.GetCustomAttribute<ProtocolPositionAttribute>().Position))
                {
                    var attr = prop.GetCustomAttribute<ProtocolPositionAttribute>();

                    if (successProp != null && (!successProp.Value || !attr.EmitOnSuccess) &&
                        (successProp.Value || !attr.EmitOnFailure)) continue;

                    if (prop.PropertyType == typeof(string))
                    {
                        var value = (string) prop.GetValue(graph);

                        if(attr.EmitLengthPrefix)
                            wrt.Write((byte) Encoding.UTF8.GetByteCount(value));
                        wrt.Write(Encoding.UTF8.GetBytes(value));
                    } else if (prop.PropertyType == typeof(byte[]))
                    {
                        wrt.Write((byte[])prop.GetValue(graph));
                    } else if (prop.PropertyType.IsEnum)
                    {
                        typeof(BinaryWriter).GetMethod("Write", new[] {prop.PropertyType.GetEnumUnderlyingType()})
                            .Invoke(wrt, new[]
                            {
                                Convert.ChangeType(prop.GetValue(graph), prop.PropertyType.GetEnumUnderlyingType())
                            });
                    } else if (prop.PropertyType == typeof(bool))
                        wrt.Write((bool)prop.GetValue(graph) ? (byte)1 : (byte)0);
                    else
                    {
                        typeof(BinaryWriter).GetMethod("Write", new[] {prop.PropertyType})
                            .Invoke(wrt, new[] {prop.GetValue(graph)});
                    }
                }
            }

            IEnumerable<Type> InheritPath()
            {
                var current = _type;
                while (current != null && current != typeof(object))
                {
                    yield return current;
                    current = current.BaseType;
                }
            }
        }

        public void Read(object graph, long length, Stream stream)
        {
            var rdr = new BinaryReader(stream, Encoding.UTF8);

            if (graph == null)
                throw new ArgumentNullException(nameof(graph), "Cannot deserialize a null packet.");

            foreach (var super in InheritPath().Reverse())
            {
                foreach (var prop in super.GetProperties(BindingFlags.Public | BindingFlags.NonPublic |
                                                         BindingFlags.DeclaredOnly | BindingFlags.Instance)
                    .Where(x => x.GetCustomAttribute<ProtocolPositionAttribute>() != null)
                    .OrderBy(x => x.GetCustomAttribute<ProtocolPositionAttribute>().Position))
                {
                    var attr = prop.GetCustomAttribute<ProtocolPositionAttribute>();

                    if (prop.PropertyType == typeof(string))
                    {
                        var len = length;

                        if (attr.EmitLengthPrefix)
                        {
                            len = rdr.ReadByte();
                            length -= sizeof(byte);
                        }
                        prop.SetValue(graph, Encoding.UTF8.GetString(rdr.ReadBytes((int) len)));
                        length -= len;
                    }
                    else if (prop.PropertyType == typeof(byte[]))
                    {
                        prop.SetValue(graph, rdr.ReadBytes((int) length));
                        length = 0;
                    }
                    else if (prop.PropertyType.IsEnum)
                    {
                        length -= Marshal.SizeOf(prop.PropertyType.GetEnumUnderlyingType());

                        prop.SetValue(graph, typeof(BinaryReader).GetMethods().Single(x =>
                                x.ReturnType == prop.PropertyType.GetEnumUnderlyingType() &&
                                x.Name.StartsWith("Read") && x.Name != "Read")
                            .Invoke(rdr, new object[0]));
                    } else if (prop.PropertyType == typeof(bool))
                        prop.SetValue(graph, rdr.ReadByte() > 0);
                    else
                    {
                        prop.SetValue(graph, typeof(BinaryReader).GetMethods().Single(x =>
                                x.ReturnType == prop.PropertyType && x.Name.StartsWith("Read") && x.Name != "Read")
                            .Invoke(rdr, new object[0]));
                    }
                }
            }

            IEnumerable<Type> InheritPath()
            {
                var current = _type;
                while (current != null && current != typeof(object))
                {
                    yield return current;
                    current = current.BaseType;
                }
            }
        }

        public long GetSize(object graph)
        {
            var len = 0L;

            if (graph == null)
                throw new ArgumentNullException(nameof(graph), "Cannot serialize a null packet.");

            foreach (var super in InheritPath().Reverse())
            {
                var successProp = (bool?)super.GetProperty(nameof(PacketResponse.Success),
                    BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance)?.GetValue(graph);

                foreach (var prop in super.GetProperties(BindingFlags.Public | BindingFlags.NonPublic |
                                                         BindingFlags.DeclaredOnly | BindingFlags.Instance)
                    .Where(x => x.GetCustomAttribute<ProtocolPositionAttribute>() != null)
                    .OrderBy(x => x.GetCustomAttribute<ProtocolPositionAttribute>().Position))
                {
                    var attr = prop.GetCustomAttribute<ProtocolPositionAttribute>();

                    if (successProp != null && (!successProp.Value || !attr.EmitOnSuccess) &&
                        (successProp.Value || !attr.EmitOnFailure)) continue;

                    if (prop.PropertyType == typeof(string))
                    {
                        var value = (string)prop.GetValue(graph);

                        if (attr.EmitLengthPrefix)
                            len += sizeof(byte);
                        len += Encoding.UTF8.GetByteCount(value);
                    } else if (prop.PropertyType == typeof(byte[]))
                    {
                        len += ((byte[]) prop.GetValue(graph)).Length;
                    } else if (prop.PropertyType.IsEnum)
                    {
                        len += Marshal.SizeOf(prop.PropertyType.GetEnumUnderlyingType());
                    }else if (prop.PropertyType == typeof(bool))
                        len += 1;
                    else
                    {
                        len += Marshal.SizeOf(prop.PropertyType);
                    }
                }
            }

            return len;

            IEnumerable<Type> InheritPath()
            {
                var current = _type;
                while (current != null && current != typeof(object))
                {
                    yield return current;
                    current = current.BaseType;
                }
            }
        }
    }
}
