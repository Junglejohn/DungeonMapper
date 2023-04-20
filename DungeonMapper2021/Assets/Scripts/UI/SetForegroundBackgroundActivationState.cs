using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SetForegroundBackgroundActivationState : MonoBehaviour
{
    public void SetForeAndBackgroundEditingState(bool isForegroundActive)
    {
        EventManager.IsForegroundActive = isForegroundActive;
    }

    public void ToggleForeAndBackgroundEditingState()
    {
        EventManager.IsForegroundActive = !EventManager.IsForegroundActive;
    }

}
