using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ChangeTextFieldsScript : MonoBehaviour {

    public AlternatingText[] TextToChangeArray;

    private void OnEnable()
    {
        for (int i = 0; i < TextToChangeArray.Length; i++)
        {
            EventManager.OnIsForegroundActivation += TextToChangeArray[i].SetActiveString;
        }
    }

    private void OnDisable()
    {
        for (int i = 0; i < TextToChangeArray.Length; i++)
        {
            EventManager.OnIsForegroundActivation -= TextToChangeArray[i].SetActiveString;
        }
    }
    
}

[System.Serializable]
public class AlternatingText
{
    public Text TextField;
    public string defautText;
    public string AlternateText;

    public void SetActiveString(bool IsDefault)
    {
        if (TextField != null)
        {
            if (IsDefault)
            {
                TextField.text = defautText;
            } else
            {
                TextField.text = AlternateText;
            }


        } else
        {
            Debug.LogError("TextField is not assigned to alternatingText object. Failed to change text");
        }
    }
}
