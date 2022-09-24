using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Manager : MonoBehaviour {

    public static Manager currentManager;

    public Session session;

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
        session = currentSession;

        EventManager.IsForegroundActive = true;

        //session.OnLoaded += SessionLoadedRefresh;
        

        session.LoadSession();

        EventManager.OnSessionRefresh(session);

        //session.OnSaved += SessionLoadedRefresh;
        //session.SaveSession();

        //EventManager.IsForegroundActive = true;

    }

    /*

    void SessionLoadedRefresh()
    {
        if (EventManager.OnSessionRefresh != null)
        {
            EventManager.OnSessionRefresh.Invoke(session);
        }
    }

    */

    
}
