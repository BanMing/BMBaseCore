/******************************************************************
** Utility.Math.cs
** @Author       : BanMing 
** @Date         : 9/28/2020 5:51:58 PM
** @Description  : 
*******************************************************************/

using UnityEngine;
using System;
using System.Collections.Generic;

namespace BMBaseCore
{
    public static partial class Utility
    {
        public static class Math
        {
            public static Quaternion Angle2Quaternion(float angle)
            {
                return Quaternion.AngleAxis(-angle, Vector3.up) * Quaternion.AngleAxis(90, Vector3.up);
            }

            public static Quaternion GetQuaternionByPos(Vector3 sourcePos, Vector3 targetPos)
            {
                Vector3 nowPos = sourcePos;
                if (nowPos == targetPos)
                {
                    return Quaternion.identity;
                }
                Vector3 direction = (targetPos - nowPos).normalized;
                return Quaternion.LookRotation(direction, Vector3.up);
            }
        }
    }
}
