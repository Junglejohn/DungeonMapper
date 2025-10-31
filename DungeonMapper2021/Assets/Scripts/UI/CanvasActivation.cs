using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CanvasActivation : MonoBehaviour {

    public int activeCanvasIndex;
    public CanvasGroup[] canvasGroupArray;

    public void Start()
    {
        ActivateSingleCanvasByIndex(activeCanvasIndex);
    }

    public void ActivateSingleCanvasByIndex(int p)
    {
        if (canvasGroupArray.Length >= p && canvasGroupArray[p] != null)
        {
            for (int i = 0; i < canvasGroupArray.Length; i++)
            {
                if (canvasGroupArray[i] != null)
                {
                    if (i == p)
                    {
                        canvasGroupArray[i].SetCanvasGroupActivation(true);
                    }
                    else
                    {
                        canvasGroupArray[i].SetCanvasGroupActivation(false);
                    }
                } else
                {
                    Debug.Log("CanvasActivation Message: CanvasGroupArray[" + i + "] was not assigned and was ignored");
                }

            }
        }
    }



}

[System.Serializable]
public class CanvasGroup
{
    public CanvasScript[] canvasArray;

    public void SetCanvasGroupActivation(bool IsActivate)
    {
        if (canvasArray != null)
        {
            for (int i = 0; i < canvasArray.Length; i++)
            {
                if (canvasArray[i] == null)
                {
                    continue;
                }

                if (IsActivate)
                {
                    canvasArray[i].ShowCanvas();
                }
                else
                {
                    canvasArray[i].HideCanvas();
                }
            }
        }
    }
}

/*
[System.Serializable]
public class CanvasGroup
{
    public Canvas[] canvasArray;

    public void SetCanvasGroupActivation(bool IsActivate)
    {
        if (canvasArray != null)
        {
            for (int i = 0; i < canvasArray.Length; i++)
            {
                if (canvasArray[i] != null)
                {
                    canvasArray[i].enabled = IsActivate;
                }
            }
        }
    }
}
*/