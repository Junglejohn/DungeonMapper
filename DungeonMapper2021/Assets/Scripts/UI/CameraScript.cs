using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

[RequireComponent(typeof(Camera))]
public class CameraScript : MonoBehaviour {

    public delegate void CameraScriptEvent(CameraScript currentCamScript);
    public CameraScriptEvent OnCamScriptValuesChanged;

    public Camera cam;

    private Vector2 _cameraPos;
    public Vector2 CameraPos { get { return _cameraPos; } set {

            float camHalfWidth = cam.orthographicSize * cam.aspect;

            float horizontal = Mathf.Clamp(((StaticVariables.AreaWidth / 2) - camHalfWidth) + StaticVariables.borderSize, (-(StaticVariables.AreaWidth / 2) + camHalfWidth) - StaticVariables.borderSize, (StaticVariables.AreaWidth/2 - camHalfWidth) + StaticVariables.borderSize);
            float vertical = Mathf.Clamp( (StaticVariables.AreaHeight / 2) - cam.orthographicSize + StaticVariables.borderSize,0 , StaticVariables.AreaHeight/2 + StaticVariables.borderSize);

            _cameraPos = new Vector2(
                Mathf.Clamp(value.x, -horizontal, horizontal),
                Mathf.Clamp(value.y, -vertical, vertical)
                );

            //Debug.Log("cameraXPos is: " + _cameraPos.x + ". the camHalfwidth is: " + camHalfWidth + ". combined horizontalpos: " + Mathf.Abs(Mathf.Abs(_cameraPos.x) + camHalfWidth) + ". max value " + StaticVariables.AreaWidth/2);
            //Debug.Log("cameraYPos is: " + _cameraPos.y + ". the OrtographicSize is: " + cam.orthographicSize + ". combined verticalpos: " + Mathf.Abs(Mathf.Abs(_cameraPos.y) + cam.orthographicSize /2) + ". max value " + StaticVariables.AreaHeight / 2);

            if (cam != null)
            {
                cam.transform.position = new Vector3(_cameraPos.x,CameraPos.y, cam.transform.position.z);
            }

            if (OnCamScriptValuesChanged != null)
            {
                OnCamScriptValuesChanged.Invoke(this);
            }
        }
    }

    public int camMinSize = 50;
    [SerializeField]
    private int _camMaxSize = 3000;
    public int camMaxSize { get { return getClampedCameraMaxSize(_camMaxSize); } set {
            _camMaxSize = getClampedCameraMaxSize(value);
        } 
    }

    private int getClampedCameraMaxSize(int size)
    {
        int clampMax = (int)StaticVariables.AreaHeight + StaticVariables.borderSize * 2;

        if (StaticVariables.AreaWidth < StaticVariables.AreaHeight * cam.aspect)
        {
            clampMax = (int)(StaticVariables.AreaWidth * cam.aspect) + StaticVariables.borderSize * 2;
        }

        return Mathf.Clamp(size, camMinSize, clampMax);
    }

    private float _camOrthographicSize = 500;
    public float camOrthographicSize{ get {
            if (cam != null)
            {
                return cam.orthographicSize;
            } else
            {
                return _camOrthographicSize;
            }
        } set {
            _camOrthographicSize = Mathf.Clamp(value, camMinSize, camMaxSize / 2);
            if (cam != null)
            {
                cam.orthographicSize = _camOrthographicSize;

                CameraPos = cam.transform.position;
            }
        }
    }

    
    private void OnEnable()
    {
        cam = gameObject.GetComponent<Camera>();
        EventManager.OnBackgroundSizeChanged += OnAreaUpdated;

    }

    private void OnDisable()
    {
        EventManager.OnBackgroundSizeChanged -= OnAreaUpdated;
    }

    public float ScrollSpeed = 1;
    
    void OnAreaUpdated(Vector2 newAreaSize)
    {
        camOrthographicSize = camOrthographicSize;
    }


    void Update()
    {

        if (!EventManager.IsCanvasControllable || TileScript.IsRunning)
        {
            return;
        }

        if (Mathf.Abs(Input.mouseScrollDelta.y) > 0)
        {
            AddCameraOrthographicSize(Input.mouseScrollDelta.y);
        }

    }

    void AddCameraOrthographicSize(float scrollYValue)
    {

        if (cam != null)
        {
            camOrthographicSize = cam.orthographicSize - (scrollYValue * StaticVariables.Preferences.camScrollSpeed);
        }

    }


    public void OnMoveCam()
    {
        StartCoroutine(MoveScreen());
    }

    bool IsRunning = false;
    IEnumerator MoveScreen()
    {

        if (IsRunning)
        {
            yield break;
        }

        IsRunning = true;

        Debug.Log("Start Moving screen");

        Vector2 storedMousePos = cam.ScreenToWorldPoint(Input.mousePosition);
        while (IsRunning && Input.GetMouseButton(0) && EventManager.IsCanvasControllable && !TileScript.IsRunning)
        {
            //the current Mouse Position for this frame
            Vector2 currentMousePos = cam.ScreenToWorldPoint(Input.mousePosition);

            CameraPos = new Vector2(
                cam.transform.position.x - (currentMousePos.x - storedMousePos.x),
                cam.transform.position.y - (currentMousePos.y - storedMousePos.y)
                );

         //the MousePos From PreviousFrame - used to get the equivalent of deltaMousePosition but in world space since we are moving a physical cam
            storedMousePos = cam.ScreenToWorldPoint(Input.mousePosition);

            yield return null;
        }

        IsRunning = false;
    }

}
