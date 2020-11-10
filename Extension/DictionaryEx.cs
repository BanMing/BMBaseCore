/******************************************************************
** DictionaryEx.cs
** @Author       : BanMing 
** @Date         : 9/30/2020 11:06:19 AM
** @Description  : 
*******************************************************************/

using System.Collections.Generic;

namespace BMBaseCore
{
    public static class DictionaryEx
    {
        public static List<Key> ToKeyList<Key, Value>(this Dictionary<Key, Value> dict)
        {
            List<Key> keys = new List<Key>();
            keys.AddRange(dict.Keys);
            return keys;
        }

        public static List<Value> ToValueList<Key, Value>(this Dictionary<Key, Value> dict)
        {
            List<Value> values = new List<Value>();
            values.AddRange(dict.Values);
            return values;
        }

    }
}
