using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MenuCanvasScript : MonoBehaviour {


    public void Exit(bool IsSaveOnExit)
    {
        if (IsSaveOnExit)
        {
            Save();
        }

        GameManager.ReturnToMainMenu();


    }

    public void ToggleForeAndBackgroundEditingState()
    {
        EventManager.IsForegroundActive = !EventManager.IsForegroundActive;
    }

    public void Save()
    {
        if (Manager.currentManager != null && Manager.currentManager.session != null)
        {
            Manager.currentManager.session.SaveSession();
        }
    }

}
