using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using YimiTools.MathUtils;
using YimiTools.VectorUtil;

public class RectObj : MonoBehaviour
{
    public PointObj pointObj;
    public CircleObj circleObj;
    public FanObj fandObj;

    [SerializeField]
    public Rectangle_AABB rect;

    public Color color;
    
  

    private void OnDrawGizmos()
    {
        GizmosUtil.Push();

        rect.UpdateCenter(transform.position.XZ());

       
        var col = color;
        if (pointObj != null && rect.ContainPoint(pointObj.point)) {
            col = Color.red;
        }
        else if (circleObj != null) {
            var ex_rect = rect.Expand(circleObj.circle.Radius, circleObj.circle.Radius);
            GizmosUtil.DrawRect(ex_rect, Color.green);
            if (circleObj.circle.IntersectWith(rect)) {
                col = Color.red;
            }
        } else if (fandObj != null && rect.IntersectWith(fandObj.fan)) { 
                col = Color.red;
        }

        GizmosUtil.DrawRectSolid(rect, col);
        GizmosUtil.Pop();
    }
}
