using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class toggledWindowedModeScript : MonoBehaviour {

    private void OnEnable()
    {
        feedbackText.SetActiveString(Screen.fullScreen);
    }

    public AlternatingText feedbackText;

	public void ToggleWindowedMode()
    {
        

        Screen.fullScreen = !Screen.fullScreen;

        feedbackText.SetActiveString(Screen.fullScreen);


    }
}
