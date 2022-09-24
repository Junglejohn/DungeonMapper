using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Animator))]
public class BackgroundColorchange : MonoBehaviour {

    public Animator anim;

    private void OnEnable()
    {
        EventManager.OnIsForegroundActivation += ChangeColorMatchForegroundBackgroundActive;
    }

    private void OnDisable()
    {
        EventManager.OnIsForegroundActivation -= ChangeColorMatchForegroundBackgroundActive;
    }


    public void ChangeColorMatchForegroundBackgroundActive(bool IsForeground)
    {
        if (anim == null)
        {
            anim = gameObject.GetComponent<Animator>(); 
        }

        anim.SetBool("IsForeground", IsForeground);



    }

}
