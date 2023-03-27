using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AlternateObjectActivations : MonoBehaviour
{

    public bool activateObject1 = true;

    public GameObject object1;

    public GameObject object2;

    private void Awake()
    {
        ActivateSingleObject(activateObject1);
    }

    public void ActivateSingleObject(bool isActivateObject1)
    {

        activateObject1 = isActivateObject1;

        if (object1 != null)
        {
            object1.SetActive(isActivateObject1);
        }

        if (object2 != null)
        {
            object2.SetActive(!isActivateObject1);
        }
    }

    public void InverseObjectActivation()
    {
        ActivateSingleObject(!activateObject1);
    }

    public void RefreshObjectActivation()
    {
        ActivateSingleObject(activateObject1);
    }
}
