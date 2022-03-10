using UnityEngine;
using UnityEditor;
using Sirenix.OdinInspector;

namespace LineEditor
{
    [System.Serializable]
    public class LineGenerator
    {
        public float DrawPlaneHigh = 0;
        public float PointRadius = 1f;

        public Line line;
        public SelectionInfo selectionInfo;
        bool needRepain = false;
        public void OnEnable()
        {
            if (line == null)
            {
                line = new Line();
            }
            if (selectionInfo == null)
            {
                selectionInfo = new SelectionInfo();
            }
        }
        public void OnSceneGUI()
        {
            Event guiEvent = Event.current;

            if (guiEvent.type == EventType.Repaint)
            {
                Draw();
                needRepain = false;
            }
            else if (guiEvent.type == EventType.Layout)
            {
                HandleUtility.AddDefaultControl(GUIUtility.GetControlID(FocusType.Passive));
            }
            else
            {
                HandleInput(guiEvent);
                if (needRepain)
                {
                    HandleUtility.Repaint();
                }
            }

        }

        void Draw()
        {

            for (int i = 0; i < line.Points.Count; i++)
            {
                var point = line.Points[i];


                //画线
                var nextIndex = i + 1;
                if (nextIndex < line.Points.Count)
                {
                    var nextPoint = line.Points[nextIndex];

                    if (i == selectionInfo.lineIndex)
                    {
                        Handles.color = Color.red;
                        Handles.DrawLine(point, nextPoint);
                    }
                    else
                    {
                        Handles.color = Color.white;
                        Handles.DrawDottedLine(point, nextPoint, 4);
                    }
                }


                //画点
                if (i == selectionInfo.pointIndex)
                {
                    Handles.color = selectionInfo.IsPointSelected ? Color.green : Color.red;
                }
                else
                {
                    Handles.color = Color.white;
                }
                Handles.DrawSolidDisc(point, Vector3.up, PointRadius);
            }
        }

        #region 用户输入
        void HandleInput(Event guiEvent)
        {
            Ray mouseRay = HandleUtility.GUIPointToWorldRay(guiEvent.mousePosition);

            float drawPlaneHight = DrawPlaneHigh;
            float distanceToDrawPlane = (drawPlaneHight - mouseRay.origin.y) / mouseRay.direction.y;

            Vector3 mousePos = mouseRay.GetPoint(distanceToDrawPlane);
            if (guiEvent.type == EventType.MouseDown && guiEvent.button == 0 && guiEvent.modifiers == EventModifiers.None)
            {
                HandleLeftMouseDown(mousePos);
            }

            if (guiEvent.type == EventType.MouseUp && guiEvent.button == 0 && guiEvent.modifiers == EventModifiers.None)
            {
                HandleLeftMouseUp(mousePos);
            }

            if (guiEvent.type == EventType.MouseDrag && guiEvent.button == 0 && guiEvent.modifiers == EventModifiers.None)
            {
                HandleLeftMouseDrag(mousePos);
            }

            if (guiEvent.type == EventType.MouseDown && guiEvent.button == 1 && guiEvent.modifiers == EventModifiers.None)
            {
                HandleRightMouseDown(mousePos);
            }

            if (!selectionInfo.IsPointSelected)
                UpdateMouseOverSelection(mousePos);
        }

        void HandleRightMouseDown(Vector3 mousePosition) 
        {
            //删除
            if (selectionInfo.pointIndex != -1)  // 没有按住其他键 点击的时候才会添加，有按住其它键 如 alt ，ctrl，之类的用来做拖拽处理，移除处理
            {               
                line.RemovePoint(selectionInfo.pointIndex);
                selectionInfo.pointIndex = -1;
                selectionInfo.IsPointSelected = false;
                selectionInfo.IsMouseOverPoint = false;
                needRepain = true;
            }
        }

        //创建新的点
        void HandleLeftMouseDown(Vector3 mousePosition)
        {
            //没有在其他点的范围内
            if (!selectionInfo.IsMouseOverPoint)  // 没有按住其他键 点击的时候才会添加，有按住其它键 如 alt ，ctrl，之类的用来做拖拽处理，移除处理
            {               
                int newPointIndex = (selectionInfo.IsMouseOverLine) ? selectionInfo.lineIndex + 1 : line.Points.Count;
                line.InsertNewPoint(newPointIndex, mousePosition);
                selectionInfo.pointIndex = newPointIndex;                
            }
            selectionInfo.IsPointSelected = true;

            needRepain = true;
        }

        void HandleLeftMouseUp(Vector3 mousePosition)
        {
            if (selectionInfo.IsPointSelected)
            {
                selectionInfo.IsPointSelected = false;
                selectionInfo.pointIndex = -1;
                needRepain = true;
            }
        }

        void HandleLeftMouseDrag(Vector3 mousePosition)
        {
            if (selectionInfo.IsPointSelected)
            {
                line.Points[selectionInfo.pointIndex] = mousePosition;
                needRepain = true;
            }
        }
        #endregion

        #region Util

        [Button]
        void Reset()
        {
            line.Points.Clear();
        }

        public class SelectionInfo
        {
            public int pointIndex = -1;
            public bool IsMouseOverPoint = false;
            public bool IsPointSelected = false;
            public int lineIndex = -1;
            public bool IsMouseOverLine = false;
        }

        void UpdateMouseOverSelection(Vector3 mousePosition)
        {
            int mouseOverIndex = -1;
            for (int i = 0; i < line.Points.Count; i++)
            {
                var point = line.Points[i];
                if (Vector3.Distance(mousePosition, point) < PointRadius)
                {
                    mouseOverIndex = i;
                    break;
                }
            }
            if (mouseOverIndex != selectionInfo.pointIndex)
            {
                selectionInfo.pointIndex = mouseOverIndex;
                selectionInfo.IsMouseOverPoint = mouseOverIndex != -1;
                needRepain = true;
            }

            if (selectionInfo.IsMouseOverPoint)
            {
                selectionInfo.IsMouseOverLine = false;
                selectionInfo.lineIndex = -1;
                needRepain = true;
            }
            else
            {
                //判断选择线段

                int mouseOverLineIndex = -1;
                float closetLineDistance = PointRadius;
                for (int i = 0; i < line.Points.Count; i++)
                {
                    var point = line.Points[i];
                    var nextIndex = i + 1;
                    if (nextIndex < line.Points.Count)
                    {
                        var nextPoint = line.Points[nextIndex];
                        var mousePos_XZ = new Vector2(mousePosition.x, mousePosition.z);
                        var p1_XZ = new Vector2(point.x, point.z);
                        var p2_XZ = new Vector2(nextPoint.x, nextPoint.z);
                        float distanceFromMouseToLine = HandleUtility.DistancePointToLineSegment(mousePos_XZ, p1_XZ, p2_XZ);

                        if (distanceFromMouseToLine < closetLineDistance)
                        {
                            closetLineDistance = distanceFromMouseToLine;
                            mouseOverLineIndex = i;
                        }
                    }
                }
                if (mouseOverLineIndex != selectionInfo.lineIndex)
                {
                    selectionInfo.lineIndex = mouseOverLineIndex;
                    selectionInfo.IsMouseOverLine = mouseOverLineIndex != -1;
                    needRepain = true;
                }
            }
        }

        #endregion
    }
}