using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ToggleStarterTextScript : MonoBehaviour {

    public GameObject starterHelpTextHierearchy;
    public GameObject NextStarterHelpTextHierarchy;

    private void OnEnable()
    {
        EventManager.OnSessionRefresh += SetHelpTextActivation;
    }

    private void OnDisable()
    {
        EventManager.OnSessionRefresh -= SetHelpTextActivation;
    }

    void SetHelpTextActivation(Session currentSession)
    {
        if (currentSession == null)
        {
            starterHelpTextHierearchy.SetActive(true);
            NextStarterHelpTextHierarchy.SetActive(false);

            return;
        }


        if (starterHelpTextHierearchy == null || NextStarterHelpTextHierarchy == null)
        {

            Debug.Log("No assigned helper text objects for ToggleStartTextScript");
            return;
        }

        switch (currentSession.InUseCheck())
        {
            case Session.SessionUseState.empty:

                starterHelpTextHierearchy.SetActive(true);
                NextStarterHelpTextHierarchy.SetActive(false);

                break;

            case Session.SessionUseState.tilesAdded:

                starterHelpTextHierearchy.SetActive(false);
                NextStarterHelpTextHierarchy.SetActive(true);

                break;

            case Session.SessionUseState.inUse:

                starterHelpTextHierearchy.SetActive(false);
                NextStarterHelpTextHierarchy.SetActive(false);

                break;
                
        }
    }


}
