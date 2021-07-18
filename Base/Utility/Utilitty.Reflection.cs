/******************************************************************
** Utilitty.cs
** @Author       : BanMing 
** @Date         : 7/18/2021 10:25:32 AM
** @Description  : 
*******************************************************************/

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BMBaseCore
{
	public static partial class Utility
	{
		public static class Reflection
		{
            public static readonly Type kVoidType = typeof(void);
            public static readonly Type kBoolType = typeof(bool);
            public static readonly Type kStringType = typeof(string);

            public static readonly Type kByteType = typeof(byte);
            public static readonly Type kIntType = typeof(int);
            public static readonly Type kLongType = typeof(long);
            public static readonly Type kFloatType = typeof(float);
            public static readonly Type kDoubleType = typeof(double);
            public static readonly Type kDecimalType = typeof(decimal);

            public static readonly Type kInt16Type = typeof(Int16);
            public static readonly Type kInt32Type = typeof(Int32);
            public static readonly Type kInt64Type = typeof(Int64);
            public static readonly Type kUInt16Type = typeof(UInt16);
            public static readonly Type kUInt32Type = typeof(UInt32);
            public static readonly Type kUInt64Type = typeof(UInt64);

            public static class GetType<T>
            {
                public static readonly Type kType = typeof(T);
                public static readonly string kName = kType.Name;
                public static readonly string kFullName = kType.FullName;
                public static readonly int kFullNameHashCode = kFullName.GetHashCode();
                public static readonly bool kIsValueType = kType.IsValueType;
            }

            public static bool IsNumeric(Type attributeType)
            {
                if ((attributeType == kByteType) ||
                    (attributeType == kIntType) ||
                    (attributeType == kLongType) ||
                    (attributeType == kFloatType) ||
                    (attributeType == kDoubleType) ||
                    (attributeType == kDecimalType) ||

                    (attributeType == kInt16Type) ||
                    (attributeType == kInt32Type) ||
                    (attributeType == kInt64Type) ||
                    (attributeType == kUInt16Type) ||
                    (attributeType == kUInt32Type) ||
                    (attributeType == kUInt64Type))
                {
                    return true;
                }

                return false;
            }

            public static bool IsBool(Type attributeType)
            {
                return (attributeType == kBoolType);
            }

            public static bool IsString(Type attributeType)
            {
                return (attributeType == kStringType);
            }

            public static bool IsEnum(Type attributeType)
            {
                return attributeType.IsEnum;
            }

        }
	}
}
