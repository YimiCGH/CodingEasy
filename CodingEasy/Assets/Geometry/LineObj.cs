using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using YimiTools.MathUtils;
using YimiTools.VectorUtil;

public class LineObj : MonoBehaviour
{
    public LineObj lineObj;

    public Line line;
    public Color color;
    public Transform p1;
    public Transform p2;

    private void OnDrawGizmos()
    {
        if (p1 == null || p2 == null) {
            return;
        }

        line.P1 = p1.transform.position.XZ();
        line.P2 = p2.transform.position.XZ();

        GizmosUtil.Push();

        Gizmos.color = color;
        Gizmos.DrawSphere(p1.position, 0.1f);
        Gizmos.DrawSphere(p2.position, 0.1f);


        var col = color;

        if (lineObj != null ) {
            Vector2? intersectPoint = line.IntersectWith(lineObj.line);
            if (intersectPoint != null)
            {
                col = Color.red;
                Gizmos.color = col;
                Gizmos.DrawSphere(intersectPoint.Value.ToXZ(),0.1f);
            }
        }

        GizmosUtil.DrawLine(line,col);

        GizmosUtil.Pop();
    }
}
