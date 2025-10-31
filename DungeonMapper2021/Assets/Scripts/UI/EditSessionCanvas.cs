using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EditSessionCanvas : CanvasScript {

    public CanvasActivation canvasActivation;
    public int returnCanvasIndex;

    public UISessionNameInputField nameInput;

    public UIAreaSizeInputFields AreaSizeInput;

    protected override void O_ShowCanvas()
    {
        base.O_ShowCanvas();
        
        {
            if (nameInput != null && nameInput.nameInputField != null)
            {
                nameInput.nameInputField.text = StaticVariables.currentSessionName;
            }

            if (AreaSizeInput != null && AreaSizeInput.XSizeInput != null)
            {
                AreaSizeInput.XSizeInput.text = StaticVariables.AreaWidth.ToString();
            }

            if (AreaSizeInput != null && AreaSizeInput.YSizeInput != null)
            {
                AreaSizeInput.YSizeInput.text = StaticVariables.AreaHeight.ToString();
            }

        }

    }



    public void CreateSession()
    {

        if (nameInput != null)
        {
            if (Session.isNameValid(nameInput.GetNameFromInputField()))
            {
                Session session = new Session(nameInput.GetNameFromInputField());

                session.AreaSize = AreaSizeInput.GetCurrentAreaSizeFromInputFields();

                session.SaveSession();

                if (EventManager.OnSessionListUpdate != null)
                {
                    EventManager.OnSessionListUpdate.Invoke(FileSystem.GetAllSessionNames().ToArray());
                }

                backToReturnCanvas();

            }
            else
            {
                nameInput.nameInputField.Select();

                Debug.Log("no valid name was typed");
            }

        }
        else
        {
            Debug.LogError("No nametext was assigned for naming the Created Session");
        }


    }

    public void AssignCurrentSessionChanges()
    {
        AssignCurrentAreaSize();
        AssignCurrentSessionName();

        Debug.Log("AssignSessionChanges - new name: [" + StaticVariables.currentSession.Name + "] new areasize: [" + StaticVariables.currentSession.AreaSize + "]");

        if (EventManager.OnSessionRefresh != null && StaticVariables.currentSession != null)
        {
            EventManager.OnSessionRefresh.Invoke(StaticVariables.currentSession);
        }

    }

    public void AssignCurrentAreaSize()
    {
        //changing StaticVariables.AreaSize automatically invokes the EventManager.OnBackgroundSizeChanged
        StaticVariables.AreaSize = AreaSizeInput.GetCurrentAreaSizeFromInputFields();

    }

    public void AssignCurrentSessionName()
    {
        //changing StaticVariables.currentSessionName automatically invokes the EventManager.OnNameChanged
        if (nameInput != null && nameInput.nameInputField != null)
        {
            StaticVariables.currentSessionName = nameInput.nameInputField.text;
            Debug.Log("Name was assigned as " + StaticVariables.currentSessionName);
        }

        //StaticVariables.currentSessionName = nameInput.GetNameFromInputField();

    }


    public void backToReturnCanvas()
    {
        if (canvasActivation != null)
        {
            canvasActivation.ActivateSingleCanvasByIndex(returnCanvasIndex);
        }
    }


}
