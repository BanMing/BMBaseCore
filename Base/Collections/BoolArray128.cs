﻿using System;


namespace BMBaseCore.Collections
{
    /// <summary>
    /// 128-bit struct that wraps two longs and acts like an array of boolean flags.
    /// </summary>
    public struct BoolArray128 : IEquatable<BoolArray128>, IComparable<BoolArray128>
    {
        public static BoolArray128 None = default(BoolArray128);

        public const int kNumBits = 128;
        private const int kMask0To63 = 0x3f;

        private UInt64 _data0, _data1;

        public BoolArray128(ulong flags0, ulong flags1)
        {
            _data0 = flags0;
            _data1 = flags1;
        }

        public bool this[int index]
        {
            get
            {
                // Are we in _data0 or _data1
                if (index < 64)
                {
                    UInt64 bit = (1ul << index);
                    return ((_data0 & bit) != 0);
                }
                else
                {
                    UInt64 bit = (1ul << (index & kMask0To63));
                    return ((_data1 & bit) != 0);
                }
            }
            set
            {
                // Are we in _data0 or _data1
                if (index < 64)
                {
                    UInt64 bit = (1ul << index);
                    if (value)
                    {
                        _data0 |= bit;
                    }
                    else
                    {
                        _data0 &= ~bit;
                    }
                }
                else
                {
                    UInt64 bit = (1ul << (index & kMask0To63));
                    if (value)
                    {
                        _data1 |= bit;
                    }
                    else
                    {
                        _data1 &= ~bit;
                    }
                }
            }
        }

        public bool Matches(BoolArray128 allOf, BoolArray128 noneOf)
        {
            return ((_data0 & allOf._data0) == allOf._data0) &&
                   ((_data1 & allOf._data1) == allOf._data1) &&
                   ((_data0 & noneOf._data0) == 0) &&
                   ((_data1 & noneOf._data1) == 0);
        }

        public bool Matches(BoolArray128 allOf)
        {
            return ((_data0 & allOf._data0) == allOf._data0) &&
                   ((_data1 & allOf._data1) == allOf._data1);
        }

        public static bool Matches(BoolArray128 flags, BoolArray128 allOf, BoolArray128 noneOf)
        {
            return ((flags._data0 & allOf._data0) == allOf._data0) &&
                   ((flags._data1 & allOf._data1) == allOf._data1) &&
                   ((flags._data0 & noneOf._data0) == 0) &&
                   ((flags._data1 & noneOf._data1) == 0);
        }

        public void Clear()
        {
            _data0 = 0;
            _data1 = 0;
        }

        /// <summary>
        /// Convert a string to a BoolArray128.
        /// </summary>
        /// <remarks>
        /// Assumes string precisely matches that generated by ToString, i.e. 32 characters long,
        /// hex characters upper case. Does not allocate any additional memory.
        /// </remarks>
        /// <param name="s">String to parse.</param>
        /// <returns>Parsed value.</returns>
        public static BoolArray128 Parse(string s)
        {
            if (string.IsNullOrEmpty(s))
            {
                return None;
            }

            ulong data0 = 0, data1 = 0;

            // data1 first, high bits to left.
            for (int i = 0; i < 16; ++i)
            {
                char nibbleChar = s[i];
                ulong nibble = (nibbleChar >= '0') && (nibbleChar <= '9') ? (ulong)(nibbleChar - '0') : (ulong)(nibbleChar - 'A' + 0xA);
                data1 = (data1 << 4) | nibble;
            }

            for (int i = 16; i < 32; ++i)
            {
                char nibbleChar = s[i];
                ulong nibble = (nibbleChar >= '0') && (nibbleChar <= '9') ? (ulong)(nibbleChar - '0') : (ulong)(nibbleChar - 'A' + 0xA);
                data0 = (data0 << 4) | nibble;
            }

            return new BoolArray128(data0, data1);
        }

        /// <summary>
        /// Convert BoolArray128 to string, as 128-bit / 32-character hex number.
        /// </summary>
        /// <returns>String representation.</returns>
        public override string ToString()
        {
            // _data1 first,high bits to left
            return $"{_data1.ToString("X16")}{_data0.ToString("X16")}";
        }

        #region Equality and related boilerplate
        public override bool Equals(object obj)
        {
            return (obj is BoolArray128) && Equals((BoolArray128)obj);
        }

        public bool Equals(BoolArray128 other)
        {
            return (other._data0 == _data0) && (other._data1 == _data1);
        }

        public static bool operator ==(BoolArray128 a, BoolArray128 b)
        {
            return (a._data0 == b._data0) && (a._data1 == b._data1);
        }

        public static bool operator !=(BoolArray128 a, BoolArray128 b)
        {
            return (a._data0 != b._data0) || (a._data1 != b._data1);
        }

        public int CompareTo(BoolArray128 other)
        {
            int result = _data1.CompareTo(other._data1);
            if (result == 0)
            {
                result = _data0.CompareTo(other._data0);
            }

            return result;
        }

        public override int GetHashCode()
        {
            int hashCode = -1921635433;
            hashCode = hashCode * -1521134295 + _data0.GetHashCode();
            hashCode = hashCode * -1521134295 + _data1.GetHashCode();
            return hashCode;
        }
        #endregion
    }
}
