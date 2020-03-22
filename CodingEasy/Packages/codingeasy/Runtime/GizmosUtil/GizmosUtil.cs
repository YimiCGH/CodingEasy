using UnityEngine;
using System.Collections;
using YimiTools.MathUtils;
using YimiTools.VectorUtil;
using UnityEditor;
using System.Collections.Generic;

public class GizmosUtil
{
    static Color color;

    public static void Push() {
        color = Gizmos.color;
    }
    public static void Pop() {
        Gizmos.color = color;
    }
    public static void DrawLine(Line line,Color color) {
        Gizmos.color = color;
        Gizmos.DrawLine(line.P1.ToXZ(),line.P2.ToXZ());
    }

    public static void DrawRect(Rectangle_AABB _rect, Color _color)
    {
        Color oldColor = Gizmos.color;
        Gizmos.color = _color;

        Gizmos.DrawSphere(_rect.Center.ToXZ(),0.05f);

        var conners = _rect.Corners;
        Gizmos.DrawLine(conners[0].ToXZ(), conners[1].ToXZ());
        Gizmos.DrawLine(conners[1].ToXZ(), conners[2].ToXZ());
        Gizmos.DrawLine(conners[2].ToXZ(), conners[3].ToXZ());
        Gizmos.DrawLine(conners[3].ToXZ(), conners[0].ToXZ());
        Gizmos.color = oldColor;

    }
    public static void DrawRectSolid(Rectangle_AABB _rect, Color _color)
    {
        Color oldColor = Gizmos.color;
        Gizmos.color = _color;
        Gizmos.DrawCube(_rect.Center.ToXZ(), _rect.Size.ToXZ());
        Gizmos.color = oldColor;

    }
    public static void DrawPoint(Vector2 _point)
    {
        Gizmos.DrawSphere(_point.ToXZ(), 0.2f);
    }

    public static void Draw2DLine(Vector2 from, Vector2 to)
    {
        Gizmos.DrawLine(new Vector3(from.x, 0, from.y), new Vector3(to.x, 0, to.y));
    }

    #region 绘制圆形，扇形
    public static void DrawCircleSolid(Circle _circle, Color _color ,Mesh mesh = null)
    {
        Gizmos.color = _color;

        Vector3 pos = _circle.Center.ToXZ() + Vector3.up * 0.01f; // 0.01fは地面と高さだと見づらいので調整用。

        Quaternion rot = Quaternion.identity;
        Vector3 scale = Vector3.one * _circle.Radius;

        mesh = CreateFanMesh(360f, 18, mesh);
        Gizmos.DrawMesh(mesh, pos, rot, scale);
    }
    public static void DrawFan(Fan _fan, Color _color,Mesh mesh)
    {
        Gizmos.color = _color;

        Vector3 pos = _fan.Center.ToXZ() + Vector3.up * 0.01f; // 0.01fは地面と高さだと見づらいので調整用。
        Vector3 forward = _fan.Forward.ToXZ();
        //Debug.Log(forward);
        Quaternion rot = Quaternion.identity;
        if (forward != Vector3.forward)
        {
            rot = Quaternion.LookRotation(forward, Vector3.up);
        }

        Vector3 scale = Vector3.one * _fan.Length;

        mesh = CreateFanMesh(_fan.OpenAngle, 16, mesh);
        Gizmos.DrawMesh(mesh, pos, rot, scale);
        Gizmos.DrawLine(_fan.Center.ToXZ(), _fan.LeftCorner.ToXZ());
        Gizmos.DrawLine(_fan.Center.ToXZ(), _fan.RightCorner.ToXZ());
        Gizmos.DrawSphere(_fan.LeftCorner.ToXZ(),0.1f);
        Gizmos.DrawSphere(_fan.RightCorner.ToXZ(), 0.1f);
    }

   

    public static Mesh CreateFanMesh(float _angle, int _triangleAmount,Mesh mesh)
    {
        if (mesh == null)
            mesh = new Mesh();
        else
            mesh.Clear();

        var vertices = CreateFanVertices(_angle, _triangleAmount);
        var triangleIndexes = new List<int>(_triangleAmount + 3);
        for (int i = 0; i < _triangleAmount; i++)
        {
            triangleIndexes.Add(0);
            triangleIndexes.Add(i + 1);
            triangleIndexes.Add(i + 2);
        }

        mesh.vertices = vertices;
        mesh.triangles = triangleIndexes.ToArray();
        mesh.RecalculateNormals();

        return mesh;
    }

    static Vector3[] CreateFanVertices(float _angle, int _triangleAmount)
    {
        if (_angle <= 0f)
        {
            throw new System.ArgumentException(string.Format("角度がおかしい！ _angle={0}", _angle));
        }
        if (_triangleAmount <= 0)
        {
            throw new System.ArgumentException(string.Format("数がおかしい！ _triangleAmount={0}", _triangleAmount));
        }
        _angle = Mathf.Min(_angle, 360);
        var vertices = new List<Vector3>(_triangleAmount + 2);

        //始点
        vertices.Add(Vector3.zero);

        float radian = _angle * Mathf.Deg2Rad;//角度转为弧度
        float startRad = -radian * 0.5f;//起始弧度
        float incRad = radian / _triangleAmount;//increase amount

        for (int i = 0; i < _triangleAmount + 1; i++)
        {
            float curRad = startRad + (incRad * i);

            Vector3 vertex = new Vector3(Mathf.Sin(curRad), 0f, Mathf.Cos(curRad));
            vertices.Add(vertex);
        }

        return vertices.ToArray();
    }
    #endregion
}
