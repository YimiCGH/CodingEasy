using UnityEngine;
using System.Collections;
namespace YimiTools.MathUtils {
    [System.Serializable]
    public struct Line
    {
        private Vector2 _pointA;
        private Vector2 _pointB;
        public Vector2 P1 { get => _pointA; set => _pointA = value; }
        public Vector2 P2 { get => _pointB; set => _pointB = value; }


        public Line(Vector2 p1, Vector2 p2) => (_pointA, _pointB) = (p1, p2);

    }

}
