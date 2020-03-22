using UnityEngine;
using System.Collections;
using YimiTools.VectorUtil;

namespace YimiTools.MathUtils
{
    [System.Serializable]
    public struct Fan
    {

        [SerializeField]
        private Vector2 _center;
        [SerializeField]
        private Vector2 _forward;
        [SerializeField]
        private float _length;
        [SerializeField,Range(0.1f,360f)]
        private float _openAngle;

        public Fan(Vector2 center, Vector2 forward, float length, float angle) =>
            (_center, _forward, _length, _openAngle) = (center, forward, length, angle);

        public Vector2 Forward { get => _forward; set => _forward = value; }
        public float Length { get => _length; }
        public float OpenAngle { get => _openAngle;}
        public Vector2 Center { get => _center; set => _center = value; }
        public Vector2 LeftCorner { get => _forward.Rotate(-_openAngle * 0.5f).normalized * _length + _center; }
        public Vector2 RightCorner { get => _forward.Rotate(_openAngle * 0.5f).normalized * _length + _center; }


        public bool ContainPoint(Vector2 point)
        {
            //判断该点和圆心构成的向量是否在两条边之间
            if (point.Between(_center, LeftCorner, RightCorner))
            {
                //判断是否在扇形内
                if (Vector2.SqrMagnitude(point - _center) <= Mathf.Pow(_length, 2)){
                    return true;
                }
            }

            return false;
        }

        public Fan Rotate(float angle) {
            var forward = _forward.Rotate(angle);
            return new Fan(_center, forward, _openAngle,_length);
        }
        public Fan LookForward(Vector2 _forward)
        {
            return new Fan(_center, _forward, OpenAngle, Length);
        }
    }
}