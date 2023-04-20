using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class DisplaySessionName : MonoBehaviour
{

    public TMP_Text displayTMPText;
    public Text displayText;

    private void OnEnable()
    {
        AssignText(StaticVariables.currentSessionName);

        EventManager.OnSessionNameUpdate += AssignText;
    }

    public void AssignText(string currentDisplayString)
    {
        if (displayTMPText != null)
        {
            displayTMPText.text = currentDisplayString;
        }

        if (displayText != null)
        {
            displayText.text = currentDisplayString;
        }
    }

    private void OnDisable()
    {
        EventManager.OnSessionNameUpdate -= AssignText;
    }
}
