using System.Collections.Generic;
using UnityEngine;

namespace LineEditor
{
    [System.Serializable]
    public class Line
    {

        public List<Vector3> Points = new List<Vector3>();


        public void InsertNewPoint(int index, Vector3 position)
        {
            Points.Insert(index, position);
        }

        public void RemovePoint(int index)
        {
            Points.RemoveAt(index);
        }
    }
}