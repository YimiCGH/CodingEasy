using UnityEngine;
using System.Collections;
namespace YimiTools.MathUtils
{
    [System.Serializable]
    public struct Rectangle_AABB
    {
        [SerializeField]
        private Vector2 _center;
        [SerializeField]
        private Vector2 _halfSize;

        public Rectangle_AABB(Vector2 center, Vector2 size) => (_center, _halfSize) = (center, size * 0.5f);
        public Rectangle_AABB(float x, float y, float xExtend, float yExtend) => 
            (_center, _halfSize) = (new Vector2(x, y), new Vector2(xExtend, yExtend));

        public Vector2[] Corners
        {
            get
            {
                Vector2[] corners = new Vector2[4];
                corners[0] = _center + new Vector2(-1, 1) * _halfSize;
                corners[1] = _center + new Vector2(1, 1) * _halfSize;
                corners[2] = _center + new Vector2(1, -1) * _halfSize;
                corners[3] = _center + new Vector2(-1, -1) * _halfSize;
                return corners;
            }
        }

        public Vector2 Center { get => _center; }
        public Vector2 Exten { get => _halfSize; }
        public Vector2 Size { get => _halfSize * 2f; }

        public float Witdh { get => _halfSize.x * 2f; }
        public float Height { get => _halfSize.y * 2f; }
        public float Left { get => _center.x - _halfSize.x; }
        public float Right { get => _center.x + _halfSize.x; }
        public float Top { get => _center.y + _halfSize.y; }
        public float Bottom { get => _center.y - _halfSize.y; }

        public Vector2 RightTop { get => _center + new Vector2(1, 1) * _halfSize; }
        public Vector2 RightBottom { get => _center + new Vector2(1, -1) * _halfSize; }
        public Vector2 LeftBottom { get => _center + new Vector2(-1, -1) * _halfSize; }
        public Vector2 LeftTop { get => _center + new Vector2(-1, 1) * _halfSize; }

        public bool ContainPoint(Vector2 point)
        {
            float xoffset = Mathf.Abs(point.x - _center.x);
            float yoffset = Mathf.Abs(point.y - _center.y);

            return xoffset < _halfSize.x && yoffset < _halfSize.y;
        }
        public Rectangle_AABB Expand(float x, float y)
        {
            return new Rectangle_AABB(_center.x,_center.y, _halfSize.x + x,_halfSize.y + y);
        }
        public void UpdateCenter(Vector2 center) {
            _center = center;
        }
    }
}