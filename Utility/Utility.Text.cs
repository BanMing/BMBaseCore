/******************************************************************
** Utility.Text.cs
** @Author       : BanMing
** @Date         : 9/28/2020 3:07:30 PM
** @Description  :
*******************************************************************/

using System;
using System.Text;

namespace BMBaseCore
{
    public static partial class Utility
    {
        public static class Text
        {
            #region Format

            [ThreadStatic]
            private static StringBuilder s_CacheSb = null;

            public static string Format(string format, object arg0)
            {
                if (format == null)
                {
                    throw new Exception("Format is invalid.");
                }

                CheckCachedStringBuilder();
                s_CacheSb.Clear();
                s_CacheSb.Length = 0;
                s_CacheSb.AppendFormat(format, arg0);
                return s_CacheSb.ToString();
            }

            public static string Format(string format, object arg0, object arg1)
            {
                if (format == null)
                {
                    throw new Exception("Format is invalid.");
                }

                CheckCachedStringBuilder();
                s_CacheSb.Clear();
                s_CacheSb.Length = 0;
                s_CacheSb.AppendFormat(format, arg0, arg1);
                return s_CacheSb.ToString();
            }

            public static string Format(string format, object arg0, object arg1, object arg2)
            {
                if (format == null)
                {
                    throw new Exception("Format is invalid.");
                }

                CheckCachedStringBuilder();
                s_CacheSb.Clear();
                s_CacheSb.Length = 0;
                s_CacheSb.AppendFormat(format, arg0, arg1, arg2);
                return s_CacheSb.ToString();
            }

            public static string Format(string format, params object[] args)
            {
                if (format == null)
                {
                    throw new Exception("Format is invalid.");
                }

                CheckCachedStringBuilder();
                s_CacheSb.Clear();
                s_CacheSb.Length = 0;
                s_CacheSb.AppendFormat(format, args);
                return s_CacheSb.ToString();
            }

            private static void CheckCachedStringBuilder()
            {
                if (s_CacheSb == null)
                {
                    s_CacheSb = new StringBuilder();
                }
            }
            #endregion

            #region Check Char

            private static string s_legalChars = " ⅠⅡⅢⅣⅤⅥⅦⅧⅨⅩⅪⅫㄱㄲㄳㄴㄵㄶㄷㄸㄹㄺㄻㄼㄽㄾㄿㅀㅁㅂㅃㅄㅅㅆㅇㅈㅉㅊㅋㅌㅍㅎㅏㅐㅑㅒㅓㅔㅕㅖㅗㅘㅙㅚㅛㅜㅝㅞㅟㅠㅡㅢㅥㅦㅧㅨㅩㅪㅫㅬㅭㅮㅯㅰㅱㅲㅳㅴㅵㅶㅷㅸㅹㅺㅻㅼㅽㅾㅿㆀㆁㆂㆃㆄㆅㆆㆇㆈㆉㆊぁあぃいぅうぇえぉおかがきぎくぐけげこごさざしじすずせぜそぞただちぢっつづてでとどなにぬねのはばぱひびぴふぶぷへべぺほぼぽまみむめもゃやゅゆょよらりるれろゎわゐゑをんゔゕゖ゚゛゜ゝゞゟ゠ァアィイゥウェエォオカガキギクグケゲコゴサザシジスズセゼソゾタダチヂッツヅテデトドナニヌネノハバパヒビピフブプヘベペホボポマミムメモャヤュユョヨラリルレロヮワヰヱヲンヴヵヶヷヸヹヺ・ーヽヾヿ√♡♢♀♂★☆↖↗↘↙↓←→↑％＋－／＝∧∠∩∪°≡≥∞∫≤≠∨‰π±√∑∴×αβγ︰:！＃＄％＆＊，．：；？＠～•、。…〈〈〉《》「」『』【】〔〕︵︶︷︸︹︺︻︼︽︽︾︿﹀﹁﹁﹂﹃﹄﹙﹙﹚﹛﹜﹝﹞﹤﹥（）＜＞｛｛｝";

            //是否为合法字符中的一个
            private static bool IsLegalChar(char c)
            {
                if (s_legalChars.Contains(c.ToString()))
                    return true;//是合法字符
                return false;
            }


            /// <summary>
            /// 含有非法字符则返回true
            /// </summary>
            /// <param name="str"></param>
            /// <returns></returns>
            public static bool DetectChar(string str)
            {
                for (int i = 0; i < str.Length; i++)
                {
                    if (str[i] >= 'a' && str[i] <= 'z')
                    {
                    }
                    else if (str[i] >= 'A' && str[i] <= 'Z')
                    {
                    }
                    else if (str[i] >= 0x4e00 && str[i] <= 0x9fbb)
                    {
                    }
                    else if (str[i] >= '0' && str[i] <= '9')
                    {
                    }
                    else if (IsLegalChar(str[i]))
                    {
                    }
                    else
                    {
                        return true;
                    }
                }
                return false;
            }

            #endregion
        }
    }
}