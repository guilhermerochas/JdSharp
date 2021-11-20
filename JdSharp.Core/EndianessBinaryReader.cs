using System;
using System.Buffers.Binary;
using System.IO;
using System.Text;

namespace JdSharp.Core
{
    public class EndianessBinaryReader : BinaryReader
    {
        private readonly bool _useBigEndian = !BitConverter.IsLittleEndian;
        
        public EndianessBinaryReader(Stream input) : base(input)
        {
        }

        public EndianessBinaryReader(Stream input, Encoding encoding) : base(input, encoding)
        {
        }

        public EndianessBinaryReader(Stream input, Encoding encoding, bool leaveOpen) : base(input, encoding, leaveOpen)
        {
        }
        
        public override short ReadInt16() => _ReadInt16();

        public override int ReadInt32() => _ReadInt32();

        public override long ReadInt64() => _ReadInt64();

        public override ushort ReadUInt16() => _ReadUInt16();

        public override uint ReadUInt32() => _ReadUInt32();

        public override ulong ReadUInt64() => _ReadUInt64();

        private short _ReadInt16() => _useBigEndian
            ? BinaryPrimitives.ReadInt16LittleEndian(ReadBytes(sizeof(short)))
            : BinaryPrimitives.ReadInt16BigEndian(ReadBytes(sizeof(short)));

        private int _ReadInt32() => _useBigEndian
            ? BinaryPrimitives.ReadInt32LittleEndian(ReadBytes(sizeof(int)))
            : BinaryPrimitives.ReadInt32BigEndian(ReadBytes(sizeof(int)));

        private long _ReadInt64() => _useBigEndian
            ? BinaryPrimitives.ReadInt64LittleEndian(ReadBytes(sizeof(long)))
            : BinaryPrimitives.ReadInt64BigEndian(ReadBytes(sizeof(long)));

        private ushort _ReadUInt16() => _useBigEndian
            ? BinaryPrimitives.ReadUInt16LittleEndian(ReadBytes(sizeof(ushort)))
            : BinaryPrimitives.ReadUInt16BigEndian(ReadBytes(sizeof(ushort)));

        private uint _ReadUInt32() => _useBigEndian
            ? BinaryPrimitives.ReadUInt32LittleEndian(ReadBytes(sizeof(uint)))
            : BinaryPrimitives.ReadUInt32BigEndian(ReadBytes(sizeof(uint)));

        private ulong _ReadUInt64() => _useBigEndian
            ? BinaryPrimitives.ReadUInt64LittleEndian(ReadBytes(sizeof(ulong)))
            : BinaryPrimitives.ReadUInt64BigEndian(ReadBytes(sizeof(ulong)));
    }
}