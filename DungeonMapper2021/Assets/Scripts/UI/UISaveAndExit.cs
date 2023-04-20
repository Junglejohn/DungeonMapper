using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UISaveAndExit : MonoBehaviour
{
    public void SaveSession()
    {
        StaticVariables.SaveCurrentSession();
    }

    public void ExitSession(bool isSave)
    {
        if (isSave)
        {
            SaveSession();
        }

        GameManager.ReturnToMainMenu();

    }
}
