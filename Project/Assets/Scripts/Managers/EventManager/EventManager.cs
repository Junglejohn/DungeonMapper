using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class EventManager {

    public delegate void SessionRefreshEvent(Session currentSession, bool isForegroundActive);
    public static SessionEvent OnRefreshSession;

    public delegate void SessionEvent(Session currentSession);
    public static SessionEvent OnSessionRefresh;
    public static SessionEvent OnSessionSelected;
    public static SessionEvent OnSessionDeleted;
    public static SessionEvent OnSessionAdded;

    public delegate void SessionListEvent(string[] sessionList);
    public static SessionListEvent OnSessionListUpdate;

    public delegate void LoadspriteEvent(LoadSprite currentLoadSrite, bool IsForeground);
    public static LoadspriteEvent OnCreateLoadTileForLoadSprite;


    public delegate void ForegroundBackgroundActivationEvent(bool isForegroundActive);
    public static ForegroundBackgroundActivationEvent OnIsForegroundActivation;

    private static bool _isForegroundActive = true;
    public static bool IsForegroundActive {
        get
        {
            return _isForegroundActive;
        }
        set
        {
            if (value != _isForegroundActive)
            {
                
                _isForegroundActive = value;

                Debug.Log("isforegroundActive was changed");

                if (OnIsForegroundActivation != null)
                {
                    OnIsForegroundActivation.Invoke(_isForegroundActive);
                }
                /*
                if (OnSessionRefresh != null && Manager.currentManager != null && Manager.currentManager.session != null)
                {
                    OnSessionRefresh.Invoke(Manager.currentManager.session);
                } else
                {
                    Debug.LogError("Refresh session was not invoked by IsForegroundActive change. Reason: no current session is assigned or no subscriptions to OnRefreshSession(session) exist");
                }

                */


            }
        }
    }

    public static bool IsCanvasControllable = true;

    public delegate void TileInteractionStateEvent(TileScript.TileInteractionState State);
    public static TileInteractionStateEvent OnForegroundTilesInteractionStateChanged;
    public static TileInteractionStateEvent OnBackgroundTilesInteractionStateChanged;


}
