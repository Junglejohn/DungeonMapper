using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnableHelpMenu : MonoBehaviour {

	public void CallSetHelpMenuActivation(bool isActive)
    {
        EventManager.isHelpEnabled = isActive;

    }

    public void CallToggleHelpMenuActivation()
    {
        EventManager.isHelpEnabled = !EventManager.isHelpEnabled;
    }
}
