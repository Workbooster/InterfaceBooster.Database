/*
 * This Source Code Form is subject to the terms of the Mozilla Public
 * License, v. 2.0. If a copy of the MPL was not distributed with this
 * file, You can obtain one at http://mozilla.org/MPL/2.0/.
 * 
 * It is an adaption of the the original "NetSerializer.Primitives"-class
 * by Tomi Valkeinen.
 * 
 * SOURCE:
 * repository:  https://github.com/tomba/netserializer
 * class:       NetSerializer.Primitives
 */

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using InterfaceBooster.Database.Interfaces.ErrorHandling;

namespace InterfaceBooster.Database.Core.Storage
{
    public class PrimitiveSerializer
    {
        static uint EncodeZigZag32(int n)
        {
            return (uint)((n << 1) ^ (n >> 31));
        }

        static ulong EncodeZigZag64(long n)
        {
            return (ulong)((n << 1) ^ (n >> 63));
        }

        static int DecodeZigZag32(uint n)
        {
            return (int)(n >> 1) ^ -(int)(n & 1);
        }

        static long DecodeZigZag64(ulong n)
        {
            return (long)(n >> 1) ^ -(long)(n & 1);
        }

        static uint ReadVarint32(Stream stream)
        {
            int result = 0;
            int offset = 0;

            for (; offset < 32; offset += 7)
            {
                int b = stream.ReadByte();
                if (b == -1)
                    throw new EndOfStreamException();

                result |= (b & 0x7f) << offset;

                if ((b & 0x80) == 0)
                    return (uint)result;
            }

            throw new InvalidDataException();
        }

        static void WriteVarint32(Stream stream, uint value)
        {
            for (; value >= 0x80u; value >>= 7)
                stream.WriteByte((byte)(value | 0x80u));

            stream.WriteByte((byte)value);
        }

        static ulong ReadVarint64(Stream stream)
        {
            long result = 0;
            int offset = 0;

            for (; offset < 64; offset += 7)
            {
                int b = stream.ReadByte();
                if (b == -1)
                    throw new EndOfStreamException();

                result |= ((long)(b & 0x7f)) << offset;

                if ((b & 0x80) == 0)
                    return (ulong)result;
            }

            throw new InvalidDataException();
        }

        static void WriteVarint64(Stream stream, ulong value)
        {
            for (; value >= 0x80u; value >>= 7)
                stream.WriteByte((byte)(value | 0x80u));

            stream.WriteByte((byte)value);
        }

        /// <summary>
        /// Writes a primitive type to the stream.
        /// Method added by Roger Guillet (roger.guillet@workbooster.ch).
        /// </summary>
        /// <param name="stream"></param>
        /// <param name="value">a primitive value</param>
        public static void WriteNullable(Stream stream, object value)
        {
            // write "has-value-flag" to stream
            // 0 = value is null
            // 1 = has value (not null)

            if (value == null)
            {
                Write(stream, (uint)0);
                return; // value is null -> stop here
            }
            else
            {
                Write(stream, (uint)1);
            }

            // continue writing the value

            if (value is string) { Write(stream, (string)value); return; }
            if (value is bool) { Write(stream, (bool)value); return; }
            if (value is int) { Write(stream, (int)value); return; }
            if (value is float) { Write(stream, (float)value); return; }
            if (value is double) { Write(stream, (double)value); return; }
            if (value is decimal) { Write(stream, (decimal)value); return; }
            if (value is DateTime) { Write(stream, (DateTime)value); return; }
            if (value is byte) { Write(stream, (byte)value); return; }
            if (value is sbyte) { Write(stream, (sbyte)value); return; }
            if (value is char) { Write(stream, (char)value); return; }
            if (value is ushort) { Write(stream, (ushort)value); return; }
            if (value is short) { Write(stream, (short)value); return; }
            if (value is uint) { Write(stream, (uint)value); return; }
        }

        /// <summary>
        /// Reads the given primitive type from the given stream.
        /// Returns the default value of the given type if the value is null.
        /// Method added by Roger Guillet (roger.guillet@workbooster.ch).
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="stream"></param>
        /// <returns>an instance of the requested type</returns>
        public static T Read<T>(Stream stream, bool isNullable = false)
        {
            object value = Read(stream, typeof(T), isNullable);

            if (value != null)
            {
                return (T)value;
            }
            else
            {
                return default(T);
            }
        }

        /// <summary>
        /// Reads the given primitive type from the given stream.
        /// The return can be null if isNullable is set to true.
        /// Method added by Roger Guillet (roger.guillet@workbooster.ch).
        /// </summary>
        /// <param name="stream"></param>
        /// <param name="type"></param>
        /// <returns>an instance of the requested type or null</returns>
        public static object Read(Stream stream, Type type, bool isNullable = false)
        {
            if (isNullable == true)
            {
                // read "has-value-flag" from stream
                // 0 = value is null
                // 1 = has value (not null)

                if (stream.ReadByte() == 0)
                {
                    // value is null
                    return null;
                }
            }

            // value is not null -> continue reading

            if (type == typeof(string))
            {
                return ReadString(stream);
            }
            else if (type == typeof(bool))
            {
                return ReadBool(stream);
            }
            else if (type == typeof(int))
            {
                return ReadInt(stream);
            }
            else if (type == typeof(float))
            {
                return ReadFloat(stream);
            }
            else if (type == typeof(double))
            {
                return ReadDouble(stream);
            }
            else if (type == typeof(decimal))
            {
                return ReadDecimal(stream);
            }
            else if (type == typeof(DateTime))
            {
                return ReadDateTime(stream);
            }
            else if (type == typeof(byte))
            {
                return ReadByte(stream);
            }
            else if (type == typeof(sbyte))
            {
                return ReadSbyte(stream);
            }
            else if (type == typeof(char))
            {
                return ReadChar(stream);
            }
            else if (type == typeof(ushort))
            {
                return ReadUshort(stream);
            }
            else if (type == typeof(short))
            {
                return ReadShort(stream);
            }
            else if (type == typeof(uint))
            {
                return ReadUint(stream);
            }

            throw new SyneryDBException(String.Format("the method ReadPrimitive does not support the type '{0}'.", type.Name));
        }

        public static void Write(Stream stream, bool value)
        {
            stream.WriteByte(value ? (byte)1 : (byte)0);
        }

        public static void Read(Stream stream, out bool value)
        {
            var b = stream.ReadByte();
            value = b != 0;
        }

        public static bool ReadBool(Stream stream)
        {
            var b = stream.ReadByte();
            return b != 0;
        }

        public static void Write(Stream stream, byte value)
        {
            stream.WriteByte(value);
        }

        public static void Read(Stream stream, out byte value)
        {
            value = (byte)stream.ReadByte();
        }

        public static byte ReadByte(Stream stream)
        {
            return (byte)stream.ReadByte();
        }

        public static void Write(Stream stream, sbyte value)
        {
            stream.WriteByte((byte)value);
        }

        public static void Read(Stream stream, out sbyte value)
        {
            value = (sbyte)stream.ReadByte();
        }

        public static sbyte ReadSbyte(Stream stream)
        {
            return (sbyte)stream.ReadByte();
        }

        public static void Write(Stream stream, char value)
        {
            WriteVarint32(stream, value);
        }

        public static void Read(Stream stream, out char value)
        {
            value = (char)ReadVarint32(stream);
        }

        public static char ReadChar(Stream stream)
        {
            return (char)ReadVarint32(stream);
        }

        public static void Write(Stream stream, ushort value)
        {
            WriteVarint32(stream, value);
        }

        public static void Read(Stream stream, out ushort value)
        {
            value = (ushort)ReadVarint32(stream);
        }

        public static ushort ReadUshort(Stream stream)
        {
            return (ushort)ReadVarint32(stream);
        }

        public static void Write(Stream stream, short value)
        {
            WriteVarint32(stream, EncodeZigZag32(value));
        }

        public static void Read(Stream stream, out short value)
        {
            value = (short)DecodeZigZag32(ReadVarint32(stream));
        }

        public static short ReadShort(Stream stream)
        {
            return (short)DecodeZigZag32(ReadVarint32(stream));
        }

        public static void Write(Stream stream, uint value)
        {
            WriteVarint32(stream, value);
        }

        public static void Read(Stream stream, out uint value)
        {
            value = ReadVarint32(stream);
        }

        public static uint ReadUint(Stream stream)
        {
            return ReadVarint32(stream);
        }

        public static void Write(Stream stream, int value)
        {
            WriteVarint32(stream, EncodeZigZag32(value));
        }

        public static void Read(Stream stream, out int value)
        {
            value = DecodeZigZag32(ReadVarint32(stream));
        }

        public static int ReadInt(Stream stream)
        {
            return DecodeZigZag32(ReadVarint32(stream));
        }

        public static void Write(Stream stream, ulong value)
        {
            WriteVarint64(stream, value);
        }

        public static void Read(Stream stream, out ulong value)
        {
            value = ReadVarint64(stream);
        }

        public static ulong ReadUlong(Stream stream)
        {
            return ReadVarint64(stream);
        }

        public static void Write(Stream stream, long value)
        {
            WriteVarint64(stream, EncodeZigZag64(value));
        }

        public static void Read(Stream stream, out long value)
        {
            value = DecodeZigZag64(ReadVarint64(stream));
        }

        public static long ReadLong(Stream stream)
        {
            return DecodeZigZag64(ReadVarint64(stream));
        }

#if !NO_UNSAFE
        public static unsafe void Write(Stream stream, float value)
        {
            uint v = *(uint*)(&value);
            WriteVarint32(stream, v);
        }

        public static unsafe void Read(Stream stream, out float value)
        {
            uint v = ReadVarint32(stream);
            value = *(float*)(&v);
        }

        public static unsafe float ReadFloat(Stream stream)
        {
            uint v = ReadVarint32(stream);
            return *(float*)(&v);
        }

        public static unsafe void Write(Stream stream, double value)
        {
            ulong v = *(ulong*)(&value);
            WriteVarint64(stream, v);
        }

        public static unsafe void Read(Stream stream, out double value)
        {
            ulong v = ReadVarint64(stream);
            value = *(double*)(&v);
        }

        public static unsafe double ReadDouble(Stream stream)
        {
            ulong v = ReadVarint64(stream);
            return *(double*)(&v);
        }
#else
		public static void Write(Stream stream, float value)
		{
			Write(stream, (double)value);
		}

		public static void Read(Stream stream, out float value)
		{
			double v;
			Read(stream, out v);
			value = (float)v;
		}

		public static float ReadFloat(Stream stream)
		{
			double v;
			Read(stream, out v);
			return (float)v;
		}

		public static void Write(Stream stream, double value)
		{
			ulong v = (ulong)BitConverter.DoubleToInt64Bits(value);
			WriteVarint64(stream, v);
		}

		public static void Read(Stream stream, out double value)
		{
			ulong v = ReadVarint64(stream);
			value = BitConverter.Int64BitsToDouble((long)v);
		}

		public static double ReadDouble(Stream stream)
		{
			ulong v = ReadVarint64(stream);
			return BitConverter.Int64BitsToDouble((long)v);
		}
#endif

        /// <summary>
        /// writes a decimal value to the stream
        /// method added by Roger Guillet (roger.guillet@workbooster.ch)
        /// </summary>
        /// <param name="stream"></param>
        /// <param name="value"></param>
        public static void Write(Stream stream, decimal value)
        {
            int[] bits = Decimal.GetBits(value);

            if (bits.Length != 4)
                throw new Exception("Decimal with bits-lenght different to 4 is not supported.");

            foreach (int bit in bits)
            {
                Write(stream, bit);
            }
        }

        /// <summary>
        /// reads a decimal value from the stream
        /// method added by Roger Guillet (roger.guillet@workbooster.ch)
        /// </summary>
        /// <param name="stream"></param>
        /// <param name="value"></param>
        public static void Read(Stream stream, out decimal value)
        {
            value = ReadDecimal(stream);
        }

        /// <summary>
        /// reads a decimal value from the stream
        /// method added by Roger Guillet (roger.guillet@workbooster.ch)
        /// </summary>
        /// <param name="stream"></param>
        /// <returns></returns>
        public static decimal ReadDecimal(Stream stream)
        {
            int[] bits = new int[4];

            for (int i = 0; i < 4; i++)
            {
                bits[i] = ReadInt(stream);
            }

            return new decimal(bits);
        }

        public static void Write(Stream stream, DateTime value)
        {
            long v = value.ToBinary();
            Write(stream, v);
        }

        public static void Read(Stream stream, out DateTime value)
        {
            long v;
            Read(stream, out v);
            value = DateTime.FromBinary(v);
        }

        public static DateTime ReadDateTime(Stream stream)
        {
            long v;
            Read(stream, out v);
            return DateTime.FromBinary(v);
        }

#if NO_UNSAFE
		public static void Write(Stream stream, string value)
		{
			if (value == null)
			{
				Write(stream, (uint)0);
				return;
			}

			var encoding = new UTF8Encoding(false, true);

			int len = encoding.GetByteCount(value);

			Write(stream, (uint)len + 1);

			var buf = new byte[len];

			encoding.GetBytes(value, 0, value.Length, buf, 0);

			stream.Write(buf, 0, len);
		}

		public static void Read(Stream stream, out string value)
		{
			uint len;
			Read(stream, out len);

			if (len == 0)
			{
				value = null;
				return;
			}
			else if (len == 1)
			{
				value = string.Empty;
				return;
			}

			len -= 1;

			var encoding = new UTF8Encoding(false, true);

			var buf = new byte[len];

			int l = 0;

			while (l < len)
			{
				int r = stream.Read(buf, l, (int)len - l);
				if (r == 0)
					throw new EndOfStreamException();
				l += r;
			}

			value = encoding.GetString(buf);
		}

		public static string ReadString(Stream stream)
		{
			uint len;
			Read(stream, out len);

			if (len == 0)
			{
				value = null;
				return;
			}
			else if (len == 1)
			{
				value = string.Empty;
				return;
			}

			len -= 1;

			var encoding = new UTF8Encoding(false, true);

			var buf = new byte[len];

			int l = 0;

			while (l < len)
			{
				int r = stream.Read(buf, l, (int)len - l);
				if (r == 0)
					throw new EndOfStreamException();
				l += r;
			}

			return encoding.GetString(buf);
		}
#else
        sealed class StringHelper
        {
            public StringHelper()
            {
                this.Encoding = new UTF8Encoding(false, true);
            }

            public const int BYTEBUFFERLEN = 256;
            public const int CHARBUFFERLEN = 128;

            Encoder m_encoder;
            Decoder m_decoder;

            byte[] m_byteBuffer;
            char[] m_charBuffer;

            public UTF8Encoding Encoding { get; private set; }
            public Encoder Encoder { get { if (m_encoder == null) m_encoder = this.Encoding.GetEncoder(); return m_encoder; } }
            public Decoder Decoder { get { if (m_decoder == null) m_decoder = this.Encoding.GetDecoder(); return m_decoder; } }

            public byte[] ByteBuffer { get { if (m_byteBuffer == null) m_byteBuffer = new byte[BYTEBUFFERLEN]; return m_byteBuffer; } }
            public char[] CharBuffer { get { if (m_charBuffer == null) m_charBuffer = new char[CHARBUFFERLEN]; return m_charBuffer; } }
        }

        [ThreadStatic]
        static StringHelper s_stringHelper;

        public unsafe static void Write(Stream stream, string value)
        {
            if (value == null)
            {
                Write(stream, (uint)0);
                return;
            }
            else if (value.Length == 0)
            {
                Write(stream, (uint)1);
                return;
            }

            var helper = s_stringHelper;
            if (helper == null)
                s_stringHelper = helper = new StringHelper();

            var encoder = helper.Encoder;
            var buf = helper.ByteBuffer;

            int totalChars = value.Length;
            int totalBytes;

            fixed (char* ptr = value)
                totalBytes = encoder.GetByteCount(ptr, totalChars, true);

            Write(stream, (uint)totalBytes + 1);
            Write(stream, (uint)totalChars);

            int p = 0;
            bool completed = false;

            while (completed == false)
            {
                int charsConverted;
                int bytesConverted;

                fixed (char* src = value)
                fixed (byte* dst = buf)
                {
                    encoder.Convert(src + p, totalChars - p, dst, buf.Length, true,
                        out charsConverted, out bytesConverted, out completed);
                }

                stream.Write(buf, 0, bytesConverted);

                p += charsConverted;
            }
        }

        public static void Read(Stream stream, out string value)
        {
            uint totalBytes;
            Read(stream, out totalBytes);

            if (totalBytes == 0)
            {
                value = null;
                return;
            }
            else if (totalBytes == 1)
            {
                value = string.Empty;
                return;
            }

            totalBytes -= 1;

            uint totalChars;
            Read(stream, out totalChars);

            var helper = s_stringHelper;
            if (helper == null)
                s_stringHelper = helper = new StringHelper();

            var decoder = helper.Decoder;
            var buf = helper.ByteBuffer;
            char[] chars;
            if (totalChars <= StringHelper.CHARBUFFERLEN)
                chars = helper.CharBuffer;
            else
                chars = new char[totalChars];

            int streamBytesLeft = (int)totalBytes;

            int cp = 0;

            while (streamBytesLeft > 0)
            {
                int bytesInBuffer = stream.Read(buf, 0, Math.Min(buf.Length, streamBytesLeft));
                if (bytesInBuffer == 0)
                    throw new EndOfStreamException();

                streamBytesLeft -= bytesInBuffer;
                bool flush = streamBytesLeft == 0 ? true : false;

                bool completed = false;

                int p = 0;

                while (completed == false)
                {
                    int charsConverted;
                    int bytesConverted;

                    decoder.Convert(buf, p, bytesInBuffer - p,
                        chars, cp, (int)totalChars - cp,
                        flush,
                        out bytesConverted, out charsConverted, out completed);

                    p += bytesConverted;
                    cp += charsConverted;
                }
            }

            value = new string(chars, 0, (int)totalChars);
        }



        public static string ReadString(Stream stream)
        {
            uint totalBytes;
            Read(stream, out totalBytes);

            if (totalBytes == 0)
            {
                return null;
            }
            else if (totalBytes == 1)
            {
                return "";
            }

            totalBytes -= 1;

            uint totalChars;
            Read(stream, out totalChars);

            var helper = s_stringHelper;
            if (helper == null)
                s_stringHelper = helper = new StringHelper();

            var decoder = helper.Decoder;
            var buf = helper.ByteBuffer;
            char[] chars;
            if (totalChars <= StringHelper.CHARBUFFERLEN)
                chars = helper.CharBuffer;
            else
                chars = new char[totalChars];

            int streamBytesLeft = (int)totalBytes;

            int cp = 0;

            while (streamBytesLeft > 0)
            {
                int bytesInBuffer = stream.Read(buf, 0, Math.Min(buf.Length, streamBytesLeft));
                if (bytesInBuffer == 0)
                    throw new EndOfStreamException();

                streamBytesLeft -= bytesInBuffer;
                bool flush = streamBytesLeft == 0 ? true : false;

                bool completed = false;

                int p = 0;

                while (completed == false)
                {
                    int charsConverted;
                    int bytesConverted;

                    decoder.Convert(buf, p, bytesInBuffer - p,
                        chars, cp, (int)totalChars - cp,
                        flush,
                        out bytesConverted, out charsConverted, out completed);

                    p += bytesConverted;
                    cp += charsConverted;
                }
            }

            return new string(chars, 0, (int)totalChars);
        }
#endif

        public static void Write(Stream stream, byte[] value)
        {
            if (value == null)
            {
                Write(stream, (uint)0);
                return;
            }

            Write(stream, (uint)value.Length + 1);

            stream.Write(value, 0, value.Length);
        }

        static readonly byte[] s_emptyByteArray = new byte[0];

        public static void Read(Stream stream, out byte[] value)
        {
            uint len;
            Read(stream, out len);

            if (len == 0)
            {
                value = null;
                return;
            }
            else if (len == 1)
            {
                value = s_emptyByteArray;
                return;
            }

            len -= 1;

            value = new byte[len];
            int l = 0;

            while (l < len)
            {
                int r = stream.Read(value, l, (int)len - l);
                if (r == 0)
                    throw new EndOfStreamException();
                l += r;
            }
        }
    }
}
