/******************************************************************
** Utility.Math.cs
** @Author       : BanMing
** @Date         : 9/28/2020 5:51:58 PM
** @Description  :
*******************************************************************/

using UnityEngine;

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

            /// <summary>
            /// angle relative to the forward [0,360]
            /// </summary>
            /// <param name="vectorAim"></param>
            /// <returns></returns>
            public static float AngleBetweenForward2Vector3(Vector3 vectorAim)
            {
                vectorAim.y = 0;
                float angleBetweenForward2Vector = 0;
                if (vectorAim.x > 0)
                {
                    angleBetweenForward2Vector = Vector3.Angle(Vector3.forward, vectorAim);
                }
                else
                {
                    angleBetweenForward2Vector = 360 - Vector3.Angle(Vector3.forward, vectorAim);
                }

                return angleBetweenForward2Vector;
            }

            public static float Radian(float fromX, float fromY, float toX, float toY)
            {
                float dx = toX - fromX;
                float dy = toY - fromY;
                return Mathf.Atan2(dy, dx);
            }

            public static float Angle(float fromX, float fromY, float toX, float toY)
            {
                return Radian(fromX, fromY, toX, toY) * 180f / Mathf.PI;
            }

            public static float AngleToRadian(float angle)
            {
                return angle * Mathf.PI / 180f;
            }

            public static float RadianToAngle(float radian)
            {
                return radian * 180f / Mathf.PI;
            }

            public static float Distance2d(float fromX, float fromY, float toX, float toY)
            {
                float dx = toX - fromX;
                float dy = toY - fromY;
                return Mathf.Sqrt(dx * dx + dy * dy);
            }

            public static float Distance2dSqrt(float fromX, float fromY, float toX, float toY)
            {
                float dx = toX - fromX;
                float dy = toY - fromY;
                return (dx * dx + dy * dy);
            }

            /// <summary>
            /// get a vector 2
            /// </summary>
            public static Vector2 Dirction2dPoint(float fromX, float fromY, float toX, float toY, float length)
            {
                float angle = Radian(fromX, fromY, toX, toY);
                Vector2 point = new Vector2();

                point.x = Mathf.Cos(angle) * length;
                point.y = Mathf.Sign(angle) * length;

                point.x += fromX;
                point.y += fromY;

                return point;
            }

            /// <summary>
            /// get a vector3 by direction (to - from) and length
            /// </summary>
            /// <param name="from"></param>
            /// <param name="to"></param>
            /// <param name="distance"></param>
            /// <returns></returns>
            public static Vector3 Get3dPoint(Vector3 from, Vector3 to, float length)
            {
                Vector3 direction = to - from;
                return from + direction.normalized * length;
            }

            /// <summary>
            /// get a point which is from o  direction of radian`s distance
            /// </summary>
            public static Vector3 Get3dPointRadian(Vector3 o, float radian, float distance)
            {
                Vector3 p = o.Clone();
                p.x = o.x + Mathf.Sin(radian) * distance;
                p.z = o.z + Mathf.Cos(radian) * distance;
                return p;
            }

            /// <summary>
            /// get a point which is from o  direction of angle`s distance
            /// </summary>
            public static Vector3 Get3dPointAngle(Vector3 o, float angle, float distance)
            {
                return Get3dPointRadian(o, Mathf.Deg2Rad * angle, distance);
            }

            /// <summary>
            /// get a vector2 by direction (to - from) and length
            /// </summary>
            public static Vector2 Get2dPoint(Vector2 from, Vector2 to, float length)
            {
                Vector2 direction = to - from;
                return from + direction.normalized * length;
            }

            /// <summary>
            /// distance from piont to line(lineFrom-lineTo) 
            /// </summary>
            public static float GetPoint2LineDis(Vector3 point, Vector3 lineFrom, Vector3 lineTo)
            {
                Vector3 ab = lineTo - lineFrom;
                Vector3 ac = point - lineFrom;

                // cross = |ac|*|ab|*sinθ
                Vector3 cross = Vector3.Cross(ac, ab);
                return cross.magnitude / ab.magnitude;
            }

            /// <summary>
            /// distance from piont to line(lineFrom-lineTo) 
            /// </summary>
            public static float GetPoint2LineDis(Vector2 point, Vector2 lineFrom, Vector2 lineTo)
            {
                Vector2 ab = lineTo - lineFrom;
                Vector2 ac = point - lineFrom;

                //Vector2.
                float dot = Vector2.Dot(ac, ab);
                //return cross.magnitude / ab.magnitude;
                return 0;
            }

            /// <summary>
            /// get intersecting perpendicular point from point to line
            /// </summary>
            public static Vector3 GetIntersectionPoint(Vector3 point, Vector3 lineFrom, Vector3 lineTo)
            {
                Vector3 line = (lineTo - lineFrom).normalized;
                return Vector3.Dot((point - lineFrom), line) * line + lineFrom;
            }

            /// <summary>
            /// get intersecting perpendicular point from point to line
            /// </summary>
            public static Vector2 GetIntersectionPoint(Vector2 point, Vector2 lineFrom, Vector2 lineTo)
            {
                Vector2 line = (lineTo - lineFrom).normalized;
                return Vector2.Dot((point - lineFrom), line) * line + lineFrom;
            }
        }
    }
}