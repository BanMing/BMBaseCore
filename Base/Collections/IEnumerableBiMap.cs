/******************************************************************
** IEnumerableBiMap.cs
** @Author       : BanMing 
** @Date         : 7/17/2021 1:05:36 PM
** @Description  : 
*******************************************************************/

using System.Collections.Generic;

namespace BMBaseCore.Collections
{
    /// <summary>
    /// A bimap that supports enumeration of left and right keys and (left,right) pairs.
    /// </summary>
    /// <typeparam name="TLeft">The type of the "left" keys.</typeparam>
    /// <typeparam name="TRight">The type of the "right" keys.</typeparam>
    public interface IEnumerableBiMap<TLeft, TRight> : IBiMap<TLeft, TRight>, IEnumerable<KeyValuePair<TLeft, TRight>>
    {
        IEnumerable<TLeft> KeysLeft { get; }

        IEnumerable<TRight> KeysRight { get; }
    }
}
