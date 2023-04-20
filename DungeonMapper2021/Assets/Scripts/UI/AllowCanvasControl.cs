using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AllowCanvasControl : MonoBehaviour {

	public void SetIsCanvasControllable(bool IsControl)
    {
        EventManager.IsCanvasControllable = IsControl;   
    }

    public void RefreshTiles()
    {

        if (EventManager.OnSessionRefresh != null && StaticVariables.currentSession != null)
        {
            EventManager.OnSessionRefresh.Invoke(StaticVariables.currentSession);
        } else
        {
            Debug.LogError("null reference - No session to refresh");
        }
    }
}
