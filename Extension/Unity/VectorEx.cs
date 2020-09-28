/******************************************************************
** VectorEx.cs
** @Author       : banming 
** @Date         : 9/28/2020 4:05:28 PM
** @Description  : vector2 & vector3 extension
*******************************************************************/

using UnityEngine;
using System.Collections;

namespace BMBaseCore
{
    public static class VectorEx
    {
        /// <summary>
        /// <see cref="Vector3" /> (x,y,z) cast to <see cref="Vector2" /> (x,z)
        /// </summary>
        /// <param name="vector3"></param>
        /// <returns></returns>
        public static Vector2 ToVector2(this Vector3 vector3)
        {
            return new Vector2(vector3.x, vector3.z);
        }

        /// <summary>
        /// <see cref="Vector2" /> (x,y) cast to <see cref="Vector3" /> (x,0,y)
        /// </summary>
        /// <param name="vector2"></param>
        /// <returns></returns>
        public static Vector3 ToVector3(this Vector2 vector2)
        {
            return new Vector3(vector2.x, 0f, vector2.y);
        }

        /// <summary>
        /// <see cref="Vector2" /> (x,y) cast to <see cref="Vector3" /> (x,param y,y)
        /// </summary>
        /// <param name="vector2"></param>
        /// <param name="y"></param>
        /// <returns></returns>
        public static Vector3 ToVector3(this Vector2 vector2, float y)
        {
            return new Vector3(vector2.x, 0f, vector2.y);
        }
    }
}