using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ISOCameraCtrl : MonoBehaviour
{
  [HorizontalGroup("Left")]
  public float Left;
  [HorizontalGroup("Left"), Button]
  void SetLeft()
  {
      Left = transform.position.x;
  }

  [HorizontalGroup("Right")]
  public float Right;
  [HorizontalGroup("Right"), Button]
  void SetRight()
  {
      Right = transform.position.x;
  }

  [HorizontalGroup("Top")]
  public float Top;
  [HorizontalGroup("Top"), Button]
  void SetTop()
  {
      Top = transform.position.z;
  }
  [HorizontalGroup("Down")]
  public float Down;
  [HorizontalGroup("Down"), Button]
  void SetDown()
  {
      Down = transform.position.z;
  }


  [BoxGroup("Offset"), HideLabel, HorizontalGroup("Offset/1")]

  public Vector3 CameraOffset;
  [BoxGroup("Offset"), Button("Read"), HorizontalGroup("Offset/1")]
  void GetOffset()
  {
      if (Target == null)
      {
          Debug.LogError("需要设置跟随目标");
          return;
      }
      CameraOffset = transform.position - Target.position;
  }

  [BoxGroup("Offset"), Button("Set"), HorizontalGroup("Offset/1")]
  void AlignCamera()
  {
      var targetPos = Target.position + CameraOffset;
      transform.position = targetPos;
  }

  public Transform Target;

  public float MobieModify = 0.02f;
  public void SetMobieModify(float value)
  {
      MobieModify = value;
  }

  [ShowInInspector, ReadOnly]
  private bool isRotating;
  [ShowInInspector, ReadOnly]
  private bool isPanning;
  [ShowInInspector, ReadOnly]
  private bool isFollowTarget;
  [ShowInInspector, ReadOnly]
  private bool isFocusTarget;


  private float velocity;
  private float zoomVelocity;

  public float MaxDeltaX = 0.5f;
  public float MaxDeltaY = 0.5f;

  [HorizontalGroup("ViewSize"),MinMaxSlider(0, 100, showFields: true)]
  public Vector2 ViewSize;
  [HorizontalGroup("ViewSpeed"), MinMaxSlider(0, 1, showFields: true)]
  public Vector2 ViewSpeed;

  public float MinView => ViewSize.x;
  public float MaxView => ViewSize.y;

  public float GetCameraSpeed() {
      float rate = 0;
      if (Camera.orthographicSize == MinView)
      {
          rate = 0;
      }
      else if (Camera.orthographicSize == MaxView)
      {
          rate = 1;
      }
      else {
          rate = (Camera.orthographicSize - MinView) / (MaxView - Camera.orthographicSize);
      }
      return Mathf.Lerp(ViewSpeed.x, ViewSpeed.y, rate);
  }

  [HorizontalGroup("ViewSize"),Button("应用最小")]
  void ApplyMinView() {
      Camera.orthographicSize = ViewSize.x;
  }
  [HorizontalGroup("ViewSize"), Button("应用最大")]
  void ApplyMaxView() {
      Camera.orthographicSize = ViewSize.y;
  }

  private float deltaX = 0f;
  private float deltaY = 0f;

  private Transform _root;
  [Required]
  public Camera Camera;

  bool _isMobie;

  private void Awake()
  {
      _root = transform;

#if UNITY_EDITOR
      _isMobie = false;
#else
      _isMobie = true;
#endif
  }

  void Update()
  {

      if (UnityEngine.EventSystems.EventSystem.current == null)
          return;
#if UNITY_EDITOR
      if (UnityEngine.EventSystems.EventSystem.current.IsPointerOverGameObject())

#else
      if (Input.touchCount > 0 && UnityEngine.EventSystems.EventSystem.current.IsPointerOverGameObject(Input.GetTouch(0).fingerId))
#endif
      {
          return;
      }

      if (isFollowTarget && Target != null)
      {
          var targetPos = Target.position + CameraOffset;
          _root.position = Vector3.Lerp(_root.position, targetPos, Time.deltaTime * 3);
          return;
      }

      if (PlayingFocus)
      {
          return;
      }
      HandlePlayerInput();


      CalculateRotation();
      CalculateZoom();
      CalculateTranslate();
  }

  void HandlePlayerInput()
  {
      if (_isMobie)
      {
          MobieControl();
      }
      else
      {
          PCControl();
      }

      if (isRotating == true)
      {
          RotateCamera();
      }
      else if (isPanning == true)
      {
          TranslateCamera();
      }
  }

  void PCControl()
  {
      //右键,旋转
      if (Input.GetMouseButton(1))
      {
          isRotating = true;
          velocity = 0f;
      }
      else
      {
          isRotating = false;
      }

      //左键,平移
      if (Input.GetMouseButton(0))
      {
          isPanning = true;
          deltaX = 0f;
          deltaY = 0f;
      }
      else
      {
          isPanning = false;
      }
  }
  void MobieControl()
  {
      if (Input.touchCount > 0)
      {
          var touch = Input.GetTouch(0);
          if (Input.touchCount == 1 && touch.phase == TouchPhase.Moved)
          {
              isPanning = true;
              deltaX = 0f;
              deltaY = 0f;
          }
          else
          {
              isPanning = false;
          }
      }
  }

  void CalculateRotation()
  {
      Vector3 rot = _root.eulerAngles;
      rot.y += velocity;
      _root.eulerAngles = rot;
      velocity = Mathf.Lerp(velocity, 0f, Time.deltaTime * 5f);
  }

  void CalculateZoom()
  {

      if (Camera.orthographicSize < ViewSize.x)
      {
          Camera.orthographicSize = ViewSize.x;
          zoomVelocity = 0f;
          return;
      }

      if (Camera.orthographicSize > ViewSize.y)
      {
          Camera.orthographicSize = ViewSize.y;
          zoomVelocity = 0f;
          return;
      }

      if (Camera.orthographicSize + zoomVelocity >= ViewSize.x &&
          Camera.orthographicSize + zoomVelocity <= ViewSize.y)
      {
          Camera.orthographicSize += zoomVelocity;
      }

      if (_isMobie)
      {
          if (Input.touchCount > 1)
          {
              var p1 = Input.GetTouch(0).position;
              var p1_ = p1 - Input.GetTouch(0).deltaPosition;
              var p2 = Input.GetTouch(1).position;
              var p2_ = p2 - Input.GetTouch(1).deltaPosition;

              var dis1 = Vector3.Magnitude(p1 - p2);
              var dis2 = Vector3.Magnitude(p1_ - p2_);
              var delta = dis1 - dis2;

              zoomVelocity -= delta * MobieModify;
          }
      }
      else
      {
          zoomVelocity -= Input.GetAxis("Mouse ScrollWheel") * 2f;
      }

      zoomVelocity = Mathf.Lerp(zoomVelocity, 0f, Time.deltaTime * 5f);
  }

  void CalculateTranslate()
  {


      if (deltaX == 0 && deltaY == 0)
      {
          return;
      }

      if (deltaX < 0)
      {
          _root.Translate(-deltaX, 0, -deltaX, Space.World);
      }

      if (deltaX > 0)
      {
          _root.Translate(-deltaX, 0, -deltaX, Space.World);
      }

      if (deltaY < 0)
      {
          _root.Translate(deltaY, 0, -deltaY, Space.World);
      }

      if (deltaY > 0)
      {
          _root.Translate(deltaY, 0, -deltaY, Space.World);
      }



      var x = Mathf.Clamp(_root.position.x, Left, Right);
      var z = Mathf.Clamp(_root.position.z, Down, Top);
      _root.position = new Vector3(x, _root.position.y, z);

      deltaY = Mathf.Lerp(deltaY, 0f, Time.deltaTime * 5);
      deltaX = Mathf.Lerp(deltaX, 0f, Time.deltaTime * 5);

      if (deltaY > 0 && deltaY < 0.01f)
      {
          deltaY = 0;
      }
      else if (deltaY < 0 && deltaY > -0.01f)
      {
          deltaY = 0;
      }

      if (deltaX > 0 && deltaX < 0.01f)
      {
          deltaX = 0;
      }
      else if (deltaX < 0 && deltaX > -0.01f)
      {
          deltaX = 0;
      }
  }

  void RotateCamera()
  {
      if (_isMobie)
      {
          //
      }
      else
      {
          velocity += Input.GetAxis("Mouse X") * 3f;
      }

  }

  void TranslateCamera()
  {
      var cameraSpeed = GetCameraSpeed();

      if (_isMobie)
      {
          var touch = Input.GetTouch(0);
          deltaX = touch.deltaPosition.x * cameraSpeed * MobieModify;
          deltaY = touch.deltaPosition.y * cameraSpeed * MobieModify;
      }
      else
      {
          deltaX = Input.GetAxis("Mouse X") * cameraSpeed;
          deltaY = Input.GetAxis("Mouse Y") * cameraSpeed;
      }


      deltaX = Mathf.Clamp(deltaX, -MaxDeltaX, MaxDeltaX);
      deltaY = Mathf.Clamp(deltaY, -MaxDeltaY, MaxDeltaY);
  }

  [Button]
  void ToggleFollowTarget(bool toggle)
  {
      isFollowTarget = toggle;
  }
  [Button]
  void ToggleFocusTarget(bool toggle,float viewSize)
  {
      isFocusTarget = toggle;
      if (isFocusTarget)
      {
          TargetSize = viewSize;
      }
      else{
          TargetSize = viewSize;
      }

      StopCoroutine("ChangeViewSize");
      StartCoroutine("ChangeViewSize");
  }
  #region 调用接口
  public void FocuseTarget(Transform target,float viewSize) {
      Target = target;

      viewSize = Mathf.Clamp(viewSize,ViewSize.x,ViewSize.y);
      ToggleFocusTarget(true, viewSize);
  }
  public void CancelFocuse() {

      ToggleFocusTarget(false,MaxView);
  }
  public void FollowTarget(Transform target) {
      Target = target;
      ToggleFollowTarget(true);
  }
  public void CancelFollowTarget() {
      Target = null;
      ToggleFollowTarget(false);
  }
  #endregion

  #region 动画
  public float animationTime;
  public float scaleSpeed = 1;
  public AnimationCurve Curve;
  float TargetSize;
  [ShowInInspector, ReadOnly]
  bool PlayingFocus;
  IEnumerator ChangeViewSize()
  {
      PlayingFocus = true;

      float beginSize = Camera.orthographicSize;
      var targetPos = Target.position + CameraOffset;
      var beginPos = _root.position;

      float time = 0;
      while (time < animationTime)
      {
          time += Time.deltaTime * scaleSpeed;
          var rate = Curve.Evaluate(time / animationTime);
          Camera.orthographicSize = Mathf.Lerp(beginSize, TargetSize, rate);
          _root.position = Vector3.Lerp(beginPos, targetPos, rate);

          yield return null;
      }
      Camera.orthographicSize = TargetSize;
      _root.position = targetPos;

      PlayingFocus = false;
      if (!isFocusTarget) {
          Target = null;
      }
  }
  #endregion


  #region Gizoms
  private void OnDrawGizmos()
  {
      var x = (Right + Left) / 2;
      var y = (Top + Down) / 2;

      Gizmos.color = new Color(1, 0.8f, 0.5f, 1f);
      var p1 = new Vector3(Left, transform.position.y + 0.5f, Top);
      var p2 = new Vector3(Right, transform.position.y + 0.5f, Top);
      var p3 = new Vector3(Right, transform.position.y + 0.5f, Down);
      var p4 = new Vector3(Left, transform.position.y + 0.5f, Down);

      var p1_ = new Vector3(Left, transform.position.y - 0.5f, Top);
      var p2_ = new Vector3(Right, transform.position.y - 0.5f, Top);
      var p3_ = new Vector3(Right, transform.position.y - 0.5f, Down);
      var p4_ = new Vector3(Left, transform.position.y - 0.5f, Down);

      Gizmos.DrawLine(p1, p2);
      Gizmos.DrawLine(p2, p3);
      Gizmos.DrawLine(p3, p4);
      Gizmos.DrawLine(p4, p1);

      Gizmos.DrawLine(p1, p1_);
      Gizmos.DrawLine(p2, p2_);
      Gizmos.DrawLine(p3, p3_);
      Gizmos.DrawLine(p4, p4_);


      Gizmos.DrawLine(p1_, p2_);
      Gizmos.DrawLine(p2_, p3_);
      Gizmos.DrawLine(p3_, p4_);
      Gizmos.DrawLine(p4_, p1_);


      Gizmos.color = new Color(1, 0.8f, 0.5f, 0.5f);
      Gizmos.DrawCube((p1 + p3_) / 2, new Vector3(p3.x - p1.x, p1.y - p1_.y, p1.z - p3.z));
  }
  #endregion
}
