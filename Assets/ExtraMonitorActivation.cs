using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExtraMonitorActivation : MonoBehaviour {

    public CameraScript camScript;

    public Camera newCam;

    public LayerMask CamWatchMask;

    private void Awake()
    {
        if (camScript == null)
        {
            camScript = gameObject.GetComponent<CameraScript>();
        }
    }

    public void ActivateAdditionalMonitor()
    {
        if (camScript == null)
        {
            Debug.LogError("ExtraMonitorActivation: No camera attached. Cant create a copy");
            return;
        }

        if (Display.displays.Length > 1 && Display.displays[1].active != true)
        {
            Display.displays[1].Activate();
        } else
        {

            Debug.LogError("ExtraMonitorActivation: Display 2 is not activated");
            return;
        }

        GameObject camObject = Instantiate(new GameObject(), camScript.transform.position, camScript.transform.rotation);

        newCam = camObject.AddComponent<Camera>();

        newCam.orthographic = true;

        newCam.targetDisplay = 1;

        newCam.cullingMask = CamWatchMask;
        
        AdjustCam(camScript);

        camScript.OnCamScriptValuesChanged += AdjustCam;
        
    }

    public void DeactivateCamera()
    {
        if (newCam == null)
        {
            Debug.LogError("ExtraMonitorActivation: No camera created. Cant deactivate it");
            return;
        }

        camScript.OnCamScriptValuesChanged -= AdjustCam;

        Destroy(newCam.gameObject);
    }

    public void AdjustCam(CameraScript currentCamScript)
    {
        newCam.orthographicSize = currentCamScript.cam.orthographicSize;
        newCam.transform.position = currentCamScript.transform.position;
        newCam.transform.rotation = currentCamScript.transform.rotation;
        
    }


}
