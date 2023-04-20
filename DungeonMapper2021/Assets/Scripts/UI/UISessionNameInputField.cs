using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UISessionNameInputField : MonoBehaviour
{

    public InputField nameInputField;

    private void Awake()
    {
        if (nameInputField != null)
        {
            nameInputField.onEndEdit.AddListener(OnEditNameSessionInput);
        }
        else
        {
            Debug.LogWarning("No Input field assigned UINameInput. Assign to correctly apply Area Size");
        }
    }

    private void OnEnable()
    {
        if (nameInputField != null)
        {
            nameInputField.text = StaticVariables.currentSessionName;

        }
    }

    public string GetNameFromInputField()
    {
        return NameFromInputField(nameInputField, StaticVariables.currentSessionName);
    }

    public void SelectNameInputField()
    {
        if(nameInputField != null)
        {
            nameInputField.Select();
        }
    }

    public void OnEditNameSessionInput(string s)
    {
        NameFromInputField(nameInputField, StaticVariables.currentSessionName);
    }

    string NameFromInputField(InputField currentInputField, string fallback)
    {
        if (currentInputField != null && Session.isNameValid(currentInputField.text))
        {

            return currentInputField.text;
        }

        return fallback; //fallback is the default size in case input field is not valid

    }
}
