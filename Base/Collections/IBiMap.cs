/******************************************************************
** IBiMap.cs
** @Author       : BanMing 
** @Date         : 7/17/2021 11:28:29 AM
** @Description  : 
*******************************************************************/

using System.Collections.Generic;

namespace BMBaseCore.Collections
{
    /// <summary>
    /// A bimap holds 1:1 mapping between pairs of objects.
    /// Siilar to a dictionary,but bidirectional -- you can look up a value from a key,
    /// and you can also look up a key from a value.
    /// So values are also keys and rather than "key" and "value"
    /// we refer to "left key" and "right key",with the bimap
    /// holding (left,right) pairs supporting lookup in either direction.
    /// 
    /// Note that IBiMap does not support iteration of either keys or pairs,
    /// see IEnumberableBiMap for enumerability.
    /// </summary>
    /// <typeparam name="TLeft">The type of the "left" keys</typeparam>
    /// <typeparam name="TRight">The type of the "right" keys</typeparam>
    /// <seealso cref="IEnumerableBiMap{TLeft, TRight}"/>
    public interface IBiMap<TLeft, TRight>
    {
        /// <summary>
        /// Note that if TLeft and TRight are the same then indexing is ambiguous
        /// and you should use GetR/GetL/Set methods instead.
        /// </summary>
        TRight this[TLeft keyLeft] { get; }

        TLeft this[TRight keyRight] { get; }

        int Count { get; }

        void Add(TLeft keyLeft, TRight keyRight);

        void Remove(TLeft keyLeft, TRight keyRight);

        bool Contains(TLeft keyLeft,TRight keyRight);

        void Clear();

        TLeft GetLeft(TRight keyRight);

        TRight GetRight(TLeft keyLeft);

        bool ContainsLeft(TLeft keyLeft);

        bool ContainsRight(TRight keyRight);

        bool RemoveLeft(TLeft keyLeft);
        
        bool RemoveRight(TRight keyRight);

        bool TryGetLeft(TRight keyRight, out TLeft keyleft);

        bool TrgGetRight(TLeft keyLeft, out TRight keyRight);
    }
}
