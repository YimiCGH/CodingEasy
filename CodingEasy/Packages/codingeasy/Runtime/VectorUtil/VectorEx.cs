using UnityEngine;
using System.Collections;

namespace YimiTools.VectorUtil
{
    public static class VectorEx
    {
        public static Vector2 XZ(this Vector3 vector) => new Vector2(vector.x, vector.z);
        public static Vector3 ToVector3(this Vector2 vector, float z = 0) => new Vector3(vector.x,  vector.y,z);
        public static Vector3 ToXZ(this Vector2 vector, float y = 0) => new Vector3(vector.x, y,vector.y);


        public static Vector2 Rotate(this Vector2 from, float angle) {
            Matrix4x4 mat = new Matrix4x4();
            mat.SetTRS(Vector3.zero,Quaternion.AngleAxis(angle,Vector3.up),Vector3.one);
            return mat.MultiplyVector(new Vector3(from.x,0,from.y)).XZ();
        }

        /// <summary>
        /// 求向量 op 是否 位于 oa 和ob 之间
        /// </summary>
        /// <param name="p">点p</param>
        /// <param name="o">共同起始点</param>
        /// <param name="a">点a</param>
        /// <param name="b">点b</param>
        /// <returns></returns>
        public static bool Between(this Vector2 p ,Vector2 o,Vector2 a,Vector2 b) {
            var oa = a - o;
            var ob = b - o;

            //【定比分点公式】
            //使用内分公式,计算 向量oa和向量ob 之间比例为 t : 1- t的位置的 
            // p = (1 - t) * oa + t * ob,( 0 <= t <= 1)，该公式表示 向量 ab 上的任意一点
            //用α  表示 （1 - t）,β 表示 t 
            //则有 p = αoa + βob  (α >= 0,β>= 0 ,且α+β= 1)

            //将公式经过变换后得到最简公式
            // delta = oa.x * ob,y - ob.x * oa.y
            // α = dx * (ob.y - ob.x) / delta
            // β = dy * (-oa.y + oa.x) / delta

            var dx = p.x - o.x;
            var dy = p.y - o.y;

            var delta = oa.x * ob.y - ob.x * oa.y;
            var alpha = (dx * ob.y - dy * ob.x) / delta;
            var beta = (-dx * oa.y + dy * oa.x) / delta;

            if (alpha >= 0 && beta >= 0)
            {
                // 向量p 在 向量a 和 b 之间
                return true;
            }
            return false;
        }
    }
}