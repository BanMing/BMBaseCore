/******************************************************************
** TransformEx.cs
** @Author       : wupeng_a 
** @Date         : 9/28/2020 3:52:30 PM
** @Description  : 
*******************************************************************/

using UnityEngine;
using System.Collections;

namespace BMBaseCore
{
    public static class TransformEx
    {
        #region Position

        /// <summary>
        /// set world position x
        /// </summary>
        /// <param name="transform"></param>
        /// <param name="newValue"></param>
        public static void SetPositionX(this Transform transform, float newValue)
        {
            Vector3 pos = transform.position;
            pos.x = newValue;
            transform.position = pos;
        }

        /// <summary>
        /// set world position y
        /// </summary>
        /// <param name="transform"></param>
        /// <param name="newValue"></param>
        public static void SetPositionY(this Transform transform, float newValue)
        {
            Vector3 pos = transform.position;
            pos.y = newValue;
            transform.position = pos;
        }

        /// <summary>
        /// set world position z
        /// </summary>
        /// <param name="transform"></param>
        /// <param name="newValue"></param>
        public static void SetPositionZ(this Transform transform, float newValue)
        {
            Vector3 pos = transform.position;
            pos.z = newValue;
            transform.position = pos;
        }

        /// <summary>
        /// add world position x
        /// </summary>
        /// <param name="transform"></param>
        /// <param name="newValue"></param>
        public static void AddPositionX(this Transform transform, float newValue)
        {
            Vector3 pos = transform.position;
            pos.x += newValue;
            transform.position = pos;
        }

        /// <summary>
        /// add world position y
        /// </summary>
        /// <param name="transform"></param>
        /// <param name="newValue"></param>
        public static void AddPositionY(this Transform transform, float newValue)
        {
            Vector3 pos = transform.position;
            pos.y += newValue;
            transform.position = pos;
        }

        /// <summary>
        /// add world position z
        /// </summary>
        /// <param name="transform"></param>
        /// <param name="newValue"></param>
        public static void AddPositionZ(this Transform transform, float newValue)
        {
            Vector3 pos = transform.position;
            pos.z += newValue;
            transform.position = pos;
        }

        #endregion

        #region Loacl Position

        /// <summary>
        /// set local position x
        /// </summary>
        /// <param name="transform"></param>
        /// <param name="newValue"></param>
        public static void SetLocalPositionX(this Transform transform, float newValue)
        {
            Vector3 pos = transform.localPosition;
            pos.x = newValue;
            transform.localPosition = pos;
        }

        /// <summary>
        /// set local position y
        /// </summary>
        /// <param name="transform"></param>
        /// <param name="newValue"></param>
        public static void SetLocalPositionY(this Transform transform, float newValue)
        {
            Vector3 pos = transform.localPosition;
            pos.y = newValue;
            transform.localPosition = pos;
        }

        /// <summary>
        /// set local position z
        /// </summary>
        /// <param name="transform"></param>
        /// <param name="newValue"></param>
        public static void SetLocalPositionZ(this Transform transform, float newValue)
        {
            Vector3 pos = transform.localPosition;
            pos.z = newValue;
            transform.localPosition = pos;
        }

        /// <summary>
        /// add local position x
        /// </summary>
        /// <param name="transform"></param>
        /// <param name="newValue"></param>
        public static void AddLocalPositionX(this Transform transform, float newValue)
        {
            Vector3 pos = transform.localPosition;
            pos.x += newValue;
            transform.localPosition = pos;
        }

        /// <summary>
        /// add lcoal position y
        /// </summary>
        /// <param name="transform"></param>
        /// <param name="newValue"></param>
        public static void AddLoaclPositionY(this Transform transform, float newValue)
        {
            Vector3 pos = transform.localPosition;
            pos.y += newValue;
            transform.localPosition = pos;
        }

        /// <summary>
        /// add local position z
        /// </summary>
        /// <param name="transform"></param>
        /// <param name="newValue"></param>
        public static void AddLocalPositionZ(this Transform transform, float newValue)
        {
            Vector3 pos = transform.localPosition;
            pos.z += newValue;
            transform.localPosition = pos;
        }

        #endregion

        #region Local Scale

        /// <summary>
        /// set local scale x
        /// </summary>
        /// <param name="transform"></param>
        /// <param name="newValue"></param>
        public static void SetLocalScaleX(this Transform transform, float newValue)
        {
            Vector3 scale = transform.localScale;
            scale.x = newValue;
            transform.localScale = scale;
        }

        /// <summary>
        /// set local scale y
        /// </summary>
        /// <param name="transform"></param>
        /// <param name="newValue"></param>
        public static void SetLocalScaleY(this Transform transform, float newValue)
        {
            Vector3 scale = transform.localScale;
            scale.y = newValue;
            transform.localScale = scale;
        }

        /// <summary>
        /// set local scale z
        /// </summary>
        /// <param name="transform"></param>
        /// <param name="newValue"></param>
        public static void SetLocalScaleZ(this Transform transform, float newValue)
        {
            Vector3 scale = transform.localScale;
            scale.z = newValue;
            transform.localScale = scale;
        }

        #endregion
    }
}
