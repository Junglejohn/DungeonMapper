using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Manager : MonoBehaviour {

    public static Manager currentManager;

    //public Session session;

    private void Awake()
    {
        if (currentManager != null)
        {
            if (currentManager != this)
            {
                Destroy(this);
            }
            
        }

        currentManager = this;
        
    }

    public void StartSession(Session currentSession)
    {

        currentSession.LoadSession();

        StaticVariables.currentSession = currentSession;

        //session = currentSession;

        EventManager.IsForegroundActive = true;

        
        /*
        StaticVariables.AreaSize = session.AreaSize;

        EventManager.OnSessionRefresh(session);
        */

    }

}
