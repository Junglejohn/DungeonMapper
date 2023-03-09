using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SessionSelectionCanvas : MonoBehaviour {

    // Use this for initialization
    private void OnEnable()
    {
        EventManager.OnSessionListUpdate += LoadButtons;

    }

    private void OnDisable()
    {
        EventManager.OnSessionListUpdate -= LoadButtons;
    }

    public Transform ParentObject;
    public SessionButtonScript buttonPrefab;
    public SessionButtonScript[] sessionButtonArray;
	public void LoadButtons(string[] sessionNames)
    {
        
        if (ParentObject != null && buttonPrefab != null)
        {
            for (int i = 0; i < sessionButtonArray.Length;i++)
            {
                if (sessionButtonArray[i] != null)
                {
                    Destroy(sessionButtonArray[i].gameObject);
                }

                
            }


            sessionButtonArray = new SessionButtonScript[sessionNames.Length];
            for (int i = 0; i < sessionNames.Length;i++)
            {
                if (sessionNames[i] != null)
                {
                    GameObject SessionSelectButton = Instantiate(buttonPrefab.gameObject, ParentObject);
                    sessionButtonArray[i] = SessionSelectButton.GetComponent<SessionButtonScript>();
                    sessionButtonArray[i].AssignButtonValues(sessionNames[i]);
                }

            }


        } else
        {
            Debug.LogError("Can't create session load buttons. Assign parent and prefab for SessionButtons");
        }



    }

    public void Exit()
    {
        Application.Quit();
    }

}
