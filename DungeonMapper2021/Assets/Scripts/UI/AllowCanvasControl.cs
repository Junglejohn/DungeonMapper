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
        if (EventManager.OnSessionRefresh != null && Manager.currentManager != null && Manager.currentManager.session != null)
        {
            EventManager.OnSessionRefresh.Invoke(Manager.currentManager.session);
        } else
        {
            Debug.LogError("null reference - No session to refresh");
        }
    }
}
