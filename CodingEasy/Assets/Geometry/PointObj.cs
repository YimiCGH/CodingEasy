using UnityEngine;
using System.Collections;
using YimiTools.VectorUtil;

public class PointObj : MonoBehaviour
{
    public Vector2 point;
    public Color color;

    private void OnDrawGizmos()
    {
        point = transform.position.XZ();
        GizmosUtil.Push();
        Gizmos.color = color;
        Gizmos.DrawSphere(point.ToXZ(),0.05f);
        GizmosUtil.Pop();
    }
}
