using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CanvasAnim : MonoBehaviour {

    public Animator Anim;
    public bool IsActive;

    public string AnimStateName = "Activated";

    private void Start()
    {
        SetState(IsActive);
    }

    public void SetState(bool currentState)
    {
        IsActive = currentState;

        if (Anim != null)
        {
            Anim.SetBool(AnimStateName, currentState);
        }

    }

    public void ToggleState(){

        IsActive = !IsActive;

        if (Anim != null)
        {
            Anim.SetBool(AnimStateName, IsActive);
        }

    }
}
