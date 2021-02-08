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


    

    #region Text
    public static void DrawString(string text, Vector3 worldPos, Color? colour = null, Color? bgColor = null)
    {
        Rect textRect;
        Rect bgRect;

        UnityEditor.Handles.BeginGUI();
        {
            var view = UnityEditor.SceneView.currentDrawingSceneView;
            Vector3 screenPos = view.camera.WorldToScreenPoint(worldPos);
            Vector2 size = GUI.skin.label.CalcSize(new GUIContent(text));

            textRect = new Rect(screenPos.x - (size.x / 2), -screenPos.y + view.position.height - 2 * size.y, size.x, size.y);
            bgRect = new Rect(textRect.x, textRect.y, size.x + 5, size.y + 5);
            GUI.color = bgColor.HasValue ? bgColor.Value : new Color(0, 0, 0, 0.7f);
            GUI.DrawTexture(bgRect, Texture2D.whiteTexture, ScaleMode.StretchToFill);
            var restoreColor = GUI.color;
            GUI.color = colour.HasValue ? colour.Value : Color.white;

            GUI.Label(textRect, text);
            GUI.color = restoreColor;
        }
        UnityEditor.Handles.EndGUI();

    }

    public static void DrawTextArea(string text, Vector3 worldPos, Vector3 offset, Color? textColor = null, Color? lineColor = null, Color? bgColor = null)
    {
        var left = worldPos + offset;
        GizmosUtility.DrawString(text, left, textColor.HasValue ? textColor.Value : Color.white, bgColor);
        Gizmos.color = lineColor.HasValue ? lineColor.Value : Color.white;
        Gizmos.DrawLine(worldPos, left);
    }

    static Color color;
    static void PushColor()
    {
        color = Gizmos.color;
    }
    static void PopColor()
    {
        Gizmos.color = color;
    }
    public static void DrawArrow(Vector3 pos, Vector3 direction, Color? color = null, float arrowHeadLength = 0.5f, float arrowHeadAngle = 20.0f)
    {
        PushColor();
        Gizmos.color = color ?? Color.white;

        //arrow shaft
        Gizmos.DrawRay(pos, direction);

        if (direction != Vector3.zero)
        {
            //arrow head
            Vector3 right = Quaternion.LookRotation(direction) * Quaternion.Euler(0, 180 + arrowHeadAngle, 0) * new Vector3(0, 0, 1);
            Vector3 left = Quaternion.LookRotation(direction) * Quaternion.Euler(0, 180 - arrowHeadAngle, 0) * new Vector3(0, 0, 1);
            Gizmos.DrawRay(pos + direction, right * arrowHeadLength);
            Gizmos.DrawRay(pos + direction, left * arrowHeadLength);
        }
        PopColor();
    }
    #endregion
    #region Other
    public static void DrawRateBar(float value, float left, float right, Vector3 worldPos, Color? colour = null)
    {
        UnityEditor.Handles.BeginGUI();
        {
            var restoreColor = GUI.color;
            var text = $"{value}/{right}";
            if (colour.HasValue) GUI.color = colour.Value;

            var view = UnityEditor.SceneView.currentDrawingSceneView;
            Vector3 screenPos = view.camera.WorldToScreenPoint(worldPos);
            Vector2 size = GUI.skin.label.CalcSize(new GUIContent(text));
            var rect = new Rect(screenPos.x - (size.x / 2), -screenPos.y + view.position.height + 4, size.x, size.y);
            GUI.Label(rect, text);
            GUI.HorizontalSlider(new Rect(screenPos.x - 32, rect.y - 10, 64, 1), value, left, right, GUI.skin.horizontalSlider, GUI.skin.horizontalSliderThumb);
            GUI.color = restoreColor;
        }
        UnityEditor.Handles.EndGUI();
    }
    #endregion
}
