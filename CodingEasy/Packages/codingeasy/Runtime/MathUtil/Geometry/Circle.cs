using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace YimiTools.MathUtils
{
    [System.Serializable]
    public struct Circle
    {
        [SerializeField]
        private Vector2 _center;
        [SerializeField]
        private float _radius;

        public Circle(Vector2 center, float radius) => (this._center, this._radius) = (center, radius);

        public Vector2 Center { get => _center; set => _center = value; }
        public float Radius { get => _radius; set => _radius = value; }

        public float Left { get => _center.x - _radius; }
        public float Right { get => _center.x + _radius; }
        public float Top { get => _center.y + _radius; }
        public float Bottom { get => _center.y - _radius; }

        #region override
        public static bool operator ==(Circle left, Circle right) => (left.Center, left.Radius) == (right.Center, right.Radius);
        public static bool operator !=(Circle left, Circle right) => (left.Center, left.Radius) != (right.Center, right.Radius);
        public override int GetHashCode() => Center.GetHashCode() ^ Radius.GetHashCode();
        public override bool Equals(object obj) => (obj is Circle circle) ? this == circle : false;
        #endregion

        #region 相交检测
        public bool ContainPoint(Vector2 point) => Vector2.SqrMagnitude(_center - point) < _radius * _radius;
        #endregion
    }
}