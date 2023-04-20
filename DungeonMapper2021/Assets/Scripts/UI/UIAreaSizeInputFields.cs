using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIAreaSizeInputFields : MonoBehaviour
{

    public InputField XSizeInput;
    public InputField YSizeInput;

    public bool showCurrentSessionInfo = false;
    public int StartingInputX()
    {
        if (showCurrentSessionInfo)
        {
            return (int)StaticVariables.AreaWidth;
        } else
        {
            return StaticVariables.defaultAreaWidth;
        }
    }

    public int StartingInputY()
    {
        if (showCurrentSessionInfo)
        {
            return (int)StaticVariables.AreaHeight;
        }
        else
        {
            return StaticVariables.defaultAreaHeight;
        }
    }

    private void Awake()
    {
        if (XSizeInput != null)
        {
            XSizeInput.onEndEdit.AddListener(OnEditXSizeInput);
        } else
        {
            Debug.LogWarning("No Input field assigned UIAreaInput. Assign to correctly apply Area Size");
        }

        if (YSizeInput != null)
        {
            YSizeInput.onEndEdit.AddListener(OnEditYSizeInput);
        } else
        {
            Debug.LogWarning("No Input field assigned UIAreaInput. Assign to correctly apply Area Size");
        }
    }

    private void OnEnable()
    {
        if (XSizeInput != null)
        {
            XSizeInput.text = SizeFromInputField(XSizeInput, StartingInputX()).ToString();
        }

        if (YSizeInput != null)
        {
            YSizeInput.text = SizeFromInputField(YSizeInput, StartingInputY()).ToString();
        }
    }

    public Vector2 GetCurrentAreaSizeFromInputFields()
    {
        return new Vector2(SizeFromInputField(XSizeInput, (int)StaticVariables.AreaWidth), SizeFromInputField(YSizeInput, (int)StaticVariables.AreaHeight));
    }

    public void OnEditXSizeInput(string s)
    {
        if (XSizeInput == null)
        {
            Debug.Log("Input field for XSizeInput is not assigned");
            return;
        }

        if (!string.IsNullOrEmpty(XSizeInput.text) && int.TryParse(XSizeInput.text, out int i))
        {
            XSizeInput.text = StaticVariables.clampedAreaWidth(i).ToString();
        }
        else
        {
            XSizeInput.text = StartingInputX().ToString();
        }
    }

    public void OnEditYSizeInput(string s)
    {
        if (YSizeInput == null)
        {
            Debug.Log("Input field for YSizeInput is not assigned");
            return;
        }

        if (!string.IsNullOrEmpty(YSizeInput.text) && int.TryParse(YSizeInput.text, out int i))
        {
            YSizeInput.text = StaticVariables.clampedAreaHeight(i).ToString();
        }
        else
        {
            YSizeInput.text = StartingInputY().ToString();
        }
    }

    int SizeFromInputField(InputField currentInputField, int fallback)
    {
        if (currentInputField != null && !string.IsNullOrEmpty(currentInputField.text))
        {
            if (int.TryParse(currentInputField.text, out int i))
            {
                return i;
            }
        }

        return fallback; //fallback is the default size in case input field is not valid

    }
}
