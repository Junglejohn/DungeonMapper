using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EditSessionCanvas : MonoBehaviour {

    public CanvasActivation canvasActivation;
    public int returnCanvasIndex;

    public InputField nameTextInput;

	public void CreateSession()
    {

        if (nameTextInput != null)
        {
            if (!string.IsNullOrEmpty(nameTextInput.text))
            {
                Session session = new Session(nameTextInput.text);
                session.SaveSession();


                nameTextInput.text = "";

                if (EventManager.OnSessionListUpdate != null)
                {
                    EventManager.OnSessionListUpdate.Invoke(FileSystem.GetAllSessionNames().ToArray());
                }

                backToReturnCanvas();

            }
            else
            {
                nameTextInput.Select();

                Debug.Log("no valid name was typed");
            }




        }
        else
        {
            Debug.LogError("No nametext was assigned for naming the Created Session");
        }


    }

    public void backToReturnCanvas()
    {
        if (canvasActivation != null)
        {
            canvasActivation.ActivateSingleCanvasByIndex(returnCanvasIndex);
        }
    }
}
