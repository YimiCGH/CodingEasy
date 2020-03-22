using UnityEngine;
using System.Collections;
using YimiTools.MathUtils;
using YimiTools.VectorUtil;
public class CircleObj : MonoBehaviour
{
    public RectObj rectObj;
    public PointObj pointObj;
    public FanObj fanObj;

    public Circle circle;
    public Color color;

    public bool isInterect;

    private Mesh mesh;
    private void OnDrawGizmos()
    {
        GizmosUtil.Push();

        circle.Center = transform.position.XZ();
        var col = color;

        if (rectObj != null && rectObj.rect.IntersectWith(circle)) {
            col = Color.red;
        } else if (pointObj != null && circle.ContainPoint(pointObj.point)) {
            col = Color.red;
        } else if (fanObj != null && circle.IntersectWith(fanObj.fan)) { 
            col = Color.red;
        }


        GizmosUtil.DrawCircleSolid(circle, col, mesh);
        GizmosUtil.Pop();
    }
}
