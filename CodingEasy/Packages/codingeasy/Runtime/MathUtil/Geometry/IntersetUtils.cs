using UnityEngine;
using System.Collections;
using YimiTools.VectorUtil;

namespace YimiTools.MathUtils 
{
    public static class IntersetUtils
    {
        #region 线段
        public static Vector2? IntersectWith(this Line l1 ,Line l2 ) {
            var x1 = l1.P1.x;
            var y1 = l1.P1.y;
            var x2 = l1.P2.x;
            var y2 = l1.P2.y;

            var x3 = l2.P1.x;
            var y3 = l2.P1.y;
            var x4 = l2.P2.x;
            var y4 = l2.P2.y;

            var den = (x1 - x2) * (y3 - y4) - (y1 - y2) * (x3 - x4);
            //分母为0，表示两线段不相交，即平行
            if (den == 0){
                return null;
            }

            var t = ((x1 - x3) * (y3 - y4) - (y1 - y3) * (x3 - x4)) / den;// 如果 0 <= t <= 1.0，表示交点落在第一个线段内
            var u = -((x1 - x2) * (y1 - y3) - (y1 - y2) * (x1 - x3)) / den;//如果 0 <= u <= 1.0 ，表示交点落在第二个线段内

            if (t > 0 && t < 1 && u > 0 && u < 1){
                return new Vector2 {
                    x = x1 + t * (x2 - x1),
                    y = y1 + t * (y2 - y1)
                };
            }
            else{
                return null;
            }
        }
        #endregion

        #region 圆形
      
        public static bool IntersectWith(this Circle _circle, Rectangle_AABB _rect)
        {
            var ex_rect = _rect.Expand(_circle.Radius, _circle.Radius);
            
            //var coners = _rect.Corners;
            if (ex_rect.ContainPoint(_circle.Center))
            { //圆心在扩展矩形内

                //如果在四个角落，只有包含该加点才算相交，否则不算
                var offsetStep = new Vector2(_rect.Witdh + _circle.Radius, _rect.Height + _circle.Radius) * 0.5f;
                var size = new Vector2(_circle.Radius, _circle.Radius);

                var leftTop_rect = new Rectangle_AABB(_rect.Center + offsetStep * new Vector2(-1, 1), size);
                var rightTop_rect = new Rectangle_AABB(_rect.Center + offsetStep * new Vector2(1, 1), size);
                var leftbottom_rect = new Rectangle_AABB(_rect.Center + offsetStep * new Vector2(-1, -1), size);
                var rightbottom = new Rectangle_AABB(_rect.Center + offsetStep * new Vector2(1, -1), size);

                if (leftTop_rect.ContainPoint(_circle.Center))
                {
                    return _circle.ContainPoint(_rect.LeftTop);
                }

                if (rightTop_rect.ContainPoint(_circle.Center))
                {
                    return _circle.ContainPoint(_rect.RightTop);
                }

                if (leftbottom_rect.ContainPoint(_circle.Center))
                {
                    return _circle.ContainPoint(_rect.LeftBottom);
                }

                if (rightbottom.ContainPoint(_circle.Center))
                {
                    return _circle.ContainPoint(_rect.RightBottom);
                }

                return true;
            }
            else {
                return false;//圆心在扩展矩形外，肯定不相交
            }
        }
        public static bool IntersectWith(this Circle _circle ,Fan _fan) { 
            return _fan.IntersectWith(_circle);
        }
        public static bool IntersectWith(this Circle _circle, Line _line)
        {
            var circle_center = _circle.Center;
            var begin = _line.P1;
            var end = _line.P2;

            //把线段方程代入圆的方程 可以得到一个二次方程
            // a * x^2 + 2 * b * x + c = 0

            //如果 x 有解，说明相交，无解
            //b^2 - a *c >= 0，有解

            var v = end - begin;
            var dx = circle_center.x - begin.x;
            var dy = circle_center.y - begin.y;

            var a = v.x * v.x + v.y * v.y;
            var b = -(dx * v.x + dy * v.y);
            var c = dx * dx + dy * dy - _circle.Radius * _circle.Radius;
            var d = b * b - a * c;
            if (d >= 0) {
                var t = (-b - Mathf.Sqrt(d)) / a;
                if ((t >= 0f) && (t <= 1f)) {
                    return true;
                }
            }

            return false;
        }
        #endregion

        #region 矩形
        public static bool IntersectWith(this Rectangle_AABB _rect1, Rectangle_AABB _rect2)
        {
            //逆向思维，不可能相交的情况，取反，就是相交的情况
            //如果rect2 在 rect1 的左边 ，右边，下边，或者上边
            return !(_rect2.Left > _rect1.Right ||
                _rect2.Right < _rect1.Left ||
                _rect2.Top > _rect1.Bottom ||
                _rect2.Bottom < _rect1.Top);
        }
        public static bool IntersectWith(this Rectangle_AABB _rect, Circle _circle)
        {
            return _circle.IntersectWith(_rect);
        }
        public static bool IntersectWith(this Rectangle_AABB _rect, Fan _fan) {
            return _fan.IntersectWith(_rect);
        }
        #endregion

        #region 扇形
        public static bool IntersectWith(this Fan _fan, Rectangle_AABB _rect)
        {

            //条件一：矩形四个角落的判断
            var coners = _rect.Corners;
            for (int i = 0; i < 4; i++)
            {
                if (_fan.ContainPoint(coners[i]))
                    return true;
            }
            //条件二：扇形的两个边点落在矩形内
            if (_rect.ContainPoint(_fan.LeftCorner))
            {
                return true;
            }
            if (_rect.ContainPoint(_fan.RightCorner))
            {
                return true;
            }
            //条件三：彼此中心点
            if (_rect.ContainPoint(_fan.Center))
            {
                return true;
            }
            if (_fan.ContainPoint(_rect.Center))
            {
                return true;
            }

            //条件四，矩形的边和弧相交
            var begin = _fan.Center;
            var end = _rect.Center;
            var v = end - begin;
            //圆的方程（x - xc）^2 + ( y - yc ) ^ 2 = r^2 
            //线段的方程 
            // x = v.x * t + begin.x
            // y = v.y * t + begin.y

            //求扇形圆心和矩形的中心线段


            //把线段方程代入可以得到一个二次方程
            //把它化为形如 a * x^2 + 2 * b * x + c = 0

            //如果 x 有解，说明相交，无解
            //b^2 - a *c >= 0，有解

            var dx = _fan.Center.x - begin.x;
            var dy = _fan.Center.y - begin.y;
            var sqr_r = _fan.Length * _fan.Length;

            var a = v.x * v.x + v.y * v.y;
            var b = -(dx * v.x + dy * v.y);
            var c = dx * dx + dy * dy - sqr_r;
            var d = b * b - a * c;

            if (d >= 0) // 有解
            {
                var t = (-b + Mathf.Sqrt(d)) / a;

                var x = v.x * t + begin.x;
                var y = v.y * t + begin.y;
                var point = new Vector2(x, y);
                //扇形只会包含其中一个解
                var left = _fan.LeftCorner;
                var right = _fan.RightCorner;


                if (_rect.ContainPoint(point) && point.Between(_fan.Center, left, right)){
                    return true;
                }

                //另一个方向
                t = (-b + Mathf.Sqrt(d)) / a;
                x = v.x * t + begin.x;
                y = v.y * t + begin.y;
                point = new Vector2(x, y);

                if (_rect.ContainPoint(point) && point.Between(_fan.Center, left, right)){
                    return true;
                }
            }
            else
            {
                //无解，说明要么扇形包含矩形中心，要么矩形包含扇形圆心，否则就不相交
            }

            return false;
        }
        public static bool IntersectWith(this Fan _fan, Circle _circle)
        {
            var a = _fan.LeftCorner;//向量a，即扇形的左边
            var b = _fan.RightCorner;//向量b，即扇形的右边
            var point = _circle.Center;

            //条件一：扇形圆心 在 圆内
            if (_circle.ContainPoint(_fan.Center))
            {
                return true;
            }

            //条件二：,判断该点和圆心构成的向量是否在两条边之间
            if (point.Between(_fan.Center, a, b))
            {
                //圆形的圆心 与 扇形的圆心 的距离 ，小于 “圆形的半径 + 扇形的半径”
                var addRadius = _fan.Length + _circle.Radius;
                //判断是否在扇形内
                if (Vector2.SqrMagnitude(point - _fan.Center) <= Mathf.Pow(addRadius, 2))
                {
                    return true;
                }
            }

            //条件三：扇形 的边与 圆形相交

            if (IntersectWith(_circle, new Line(_fan.Center, _fan.LeftCorner))){
                return true;
            }
            if (IntersectWith(_circle, new Line(_fan.Center, _fan.RightCorner))){
                return true;
            }

            return false;
        }
        #endregion
    }
}