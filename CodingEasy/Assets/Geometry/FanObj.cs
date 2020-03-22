using UnityEngine;
using System.Collections;
using YimiTools.MathUtils;
using YimiTools.VectorUtil;

public class FanObj : MonoBehaviour
{
    public PointObj pointObj;
    public RectObj rectObj;
    public CircleObj circleObj;

    public Color color;
    
    [SerializeField]
    public Fan fan;
    public Mesh mesh;
 

    private void DrawPointGizmos()
    {
        if (fan.Length <= 0f)
        {
            return;
        }
        var col = color;

        if (pointObj != null && fan.ContainPoint(pointObj.point)) {
            col = Color.red;
        } else if (rectObj != null && fan.IntersectWith(rectObj.rect)) {
            col = Color.red;
        } else if (circleObj != null && fan.IntersectWith(circleObj.circle)) { 
            col = Color.red;
        }



        GizmosUtil.DrawFan(fan, col, mesh);
    }

    private void OnDrawGizmos()
    {
        GizmosUtil.Push();
        fan.Center = transform.position.XZ();
        fan.Forward = transform.forward.XZ().normalized;
        DrawPointGizmos();
        GizmosUtil.Pop();
    }
}
