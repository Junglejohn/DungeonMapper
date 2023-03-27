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

            float horizontal =Mathf.Clamp((StaticVariables.AreaWidth / 2) - camHalfWidth, -(StaticVariables.AreaWidth / 2) + camHalfWidth, StaticVariables.AreaWidth/2 - camHalfWidth);
            float vertical = Mathf.Clamp( (StaticVariables.AreaHeight / 2) - (cam.orthographicSize),0, StaticVariables.AreaHeight/2);

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
    
    float originalCamSize = 1;
    public int camMaxSize = 1250;
    public int camMinSize = 50;
    
    private void OnEnable()
    {
        cam = gameObject.GetComponent<Camera>();
        originalCamSize = cam.orthographicSize;

    }


    public float ScrollSpeed = 1;
    
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

            float orthographicSize = Mathf.Clamp(cam.orthographicSize - (scrollYValue * StaticVariables.Preferences.camScrollSpeed), camMinSize, camMaxSize/2);
            cam.orthographicSize = orthographicSize;
            CameraPos = cam.transform.position; //correcting position of camera based on new size

        }
    }

    public void OnMoveCam()
    {

        StartCoroutine(MoveScreen());
        if (Input.GetMouseButtonDown(0) && IsRunning == false && EventManager.IsCanvasControllable)
        {
            Debug.Log("Start Moving screen");
            
        }


    }

    bool IsRunning = false;
    IEnumerator MoveScreen()
    {

        if (IsRunning)
        {
            yield break;
        }

        IsRunning = true;



        Vector2 storedMousePos = cam.ScreenToWorldPoint(Input.mousePosition);
        while (IsRunning && Input.GetMouseButton(0) && EventManager.IsCanvasControllable && !TileScript.IsRunning)
        {
            //the current Mouse Position for this frame
            Vector2 currentMousePos = cam.ScreenToWorldPoint(Input.mousePosition);

            CameraPos = new Vector2(
                cam.transform.position.x - (currentMousePos.x - storedMousePos.x),
                cam.transform.position.y - (currentMousePos.y - storedMousePos.y)
                );

            //Debug.Log(cam.orthographicSize + "cam halfwidth is: " + camHalfWidth + " camerapos is: " + CameraPos);

            //the MousePos From PreviousFrame - used to get the equivalent of deltaMousePosition but in world space since we are moving a physical cam
            storedMousePos = cam.ScreenToWorldPoint(Input.mousePosition);

            yield return null;
        }

        IsRunning = false;
    }

}
