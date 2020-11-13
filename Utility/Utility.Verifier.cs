/******************************************************************
** Utility.CRC32.cs
** @Author       : BanMing
** @Date         : 2020/9/28 下午2:44:58
** @Description  :
*******************************************************************/

using System;
using System.Collections.Generic;
using System.IO;
using System.Security.Cryptography;
using System.Text;

namespace BMBaseCore
{
    public static partial class Utility
    {
        public static class Verifier
        {
            private static readonly SHA256Managed sha256 = new SHA256Managed();
            private static readonly CRC32 crc32 = new CRC32();

            #region SHA256Managed

            public static string GetSHA256Hash(string input)
            {
                byte[] data = sha256.ComputeHash(Encoding.UTF8.GetBytes(input));
                return ToHash(data);
            }

            public static string GetSHA256Hash(Stream input)
            {
                byte[] data = sha256.ComputeHash(input);
                return ToHash(data);
            }

            public static bool VerifySHA256Hash(string input, string hash)
            {
                StringComparer comparer = StringComparer.OrdinalIgnoreCase;
                return 0 == comparer.Compare(input, hash);
            }

            #endregion SHA256Managed

            #region CRC32

            public static uint GetCRC32(byte[] bytes)
            {
                return CRC32.Compute(bytes);
            }

            public static uint GetCRC32(Stream input)
            {
                byte[] data = crc32.ComputeHash(input);
                return GetCRC32(data);
            }

            public static uint GetCRC32(string str)
            {
                byte[] data = System.Text.UTF8Encoding.UTF8.GetBytes(str);
                return GetCRC32(data);
            }

            public static string GetCRC32String(byte[] bytes)
            {
                byte[] data = crc32.ComputeHash(bytes);
                return ToHash(data);
            }

            public static string GetCRC32String(Stream input)
            {
                byte[] data = crc32.ComputeHash(input);
                return ToHash(data);
            }

            public static string GetCRC32String(string str)
            {
                byte[] data = System.Text.UTF8Encoding.UTF8.GetBytes(str);
                return ToHash(data);
            }

            public static string ToHash(byte[] data)
            {
                StringBuilder sb = new StringBuilder();
                for (int i = 0; i < data.Length; i++)
                {
                    sb.Append(data[i].ToString("x2"));
                }

                return sb.ToString();
            }

            public static string GetCRC32Hash(string input)
            {
                byte[] data = crc32.ComputeHash(Encoding.UTF8.GetBytes(input));
                return ToHash(data);
            }

            public static bool VerifyCrc32Hash(string input, string hash)
            {
                StringComparer comparer = StringComparer.OrdinalIgnoreCase;
                return 0 == comparer.Compare(input, hash);
            }

            #endregion CRC32
        }
        
        #region CRC32 Struct
        /// <summary>
        /// https://damieng.com/blog/2006/08/08/calculating_crc32_in_c_and_net
        /// https://jonlabelle.com/snippets/view/csharp/calculate-crc32-hash-in-csharp
        /// </summary>
        private sealed class CRC32 : HashAlgorithm
        {
            private const UInt32 DefaultPolynomial = 0xedb88320u;
            private const UInt32 DefaultSeed = 0xffffffffu;

            private static UInt32[] _defaultTable;

            private readonly UInt32 _seed;
            private readonly UInt32[] _table;
            private UInt32 _hash;

            public CRC32() : this(DefaultPolynomial, DefaultSeed)
            {
            }

            public CRC32(UInt32 polynomial, UInt32 seed)
            {
                if (!BitConverter.IsLittleEndian)
                {
                    throw new PlatformNotSupportedException("Not supported on Big Endian processors");
                }

                _table = InitializeTable(polynomial);
                this._seed = _hash = seed;
            }

            public override void Initialize()
            {
                _hash = _seed;
            }

            protected override void HashCore(byte[] array, int ibStart, int cbSize)
            {
                _hash = CalculateHash(_table, _hash, array, ibStart, cbSize);
            }

            protected override byte[] HashFinal()
            {
                byte[] hashBuffer = UInt32ToBigEndianBytes(~_hash);
                HashValue = hashBuffer;
                return hashBuffer;
            }

            public override int HashSize
            {
                get { return 32; }
            }

            public static UInt32 Compute(byte[] buffer)
            {
                return Compute(DefaultSeed, buffer);
            }

            public static UInt32 Compute(UInt32 seed, byte[] buffer)
            {
                return Compute(DefaultPolynomial, seed, buffer);
            }

            public static UInt32 Compute(UInt32 polynomial, UInt32 seed, byte[] buffer)
            {
                return ~CalculateHash(InitializeTable(polynomial), seed, buffer, 0, buffer.Length);
            }

            private static UInt32[] InitializeTable(UInt32 polynomial)
            {
                if (polynomial == DefaultPolynomial && _defaultTable != null)
                {
                    return _defaultTable;
                }

                UInt32[] createTable = new UInt32[256];
                for (int i = 0; i < 256; i++)
                {
                    UInt32 entry = (UInt32)i;
                    for (int j = 0; j < 8; j++)
                    {
                        if ((entry & 1) == 1)
                        {
                            entry = (entry >> 1) ^ polynomial;
                        }
                        else
                        {
                            entry >>= 1;
                        }
                    }
                    createTable[i] = entry;
                }

                if (polynomial == DefaultPolynomial)
                {
                    _defaultTable = createTable;
                }

                return createTable;
            }

            private static UInt32 CalculateHash(UInt32[] table, UInt32 seed, IList<byte> buffer, int start, int size)
            {
                uint hash = seed;
                for (int i = start; i < start + size; i++)
                {
                    hash = (hash >> 8) ^ table[buffer[i] ^ hash & 0xff];
                }
                return hash;
            }

            private static byte[] UInt32ToBigEndianBytes(UInt32 uint32)
            {
                byte[] result = BitConverter.GetBytes(uint32);

                if (BitConverter.IsLittleEndian)
                {
                    Array.Reverse(result);
                }

                return result;
            }
        }
        #endregion
    }
}