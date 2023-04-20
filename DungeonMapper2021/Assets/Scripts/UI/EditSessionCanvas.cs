using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EditSessionCanvas : MonoBehaviour {

    public CanvasActivation canvasActivation;
    public int returnCanvasIndex;

    public UISessionNameInputField nameInput;

    public UIAreaSizeInputFields AreaSizeInput;


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
        StaticVariables.currentSessionName = nameInput.GetNameFromInputField();

    }


    public void backToReturnCanvas()
    {
        if (canvasActivation != null)
        {
            canvasActivation.ActivateSingleCanvasByIndex(returnCanvasIndex);
        }
    }


}
