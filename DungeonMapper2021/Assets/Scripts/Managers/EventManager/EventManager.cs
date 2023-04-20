using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class EventManager {

    //sessions
    public delegate void SessionRefreshEvent(Session currentSession, bool isForegroundActive);
    public static SessionEvent OnRefreshSession;

    public delegate void SessionEvent(Session currentSession);
    public static SessionEvent OnSessionRefresh;
    public static SessionEvent OnSessionSelected;
    public static SessionEvent OnSessionDeleted;
    public static SessionEvent OnSessionAdded;

    public delegate void SessionNameEvent(string name);
    public static SessionNameEvent OnSessionNameUpdate;

    public delegate void backgroundSizeEvent(Vector2 size);
    public static backgroundSizeEvent OnBackgroundSizeChanged;

    public delegate void SessionListEvent(string[] sessionList);
    public static SessionListEvent OnSessionListUpdate;

    //LoadSprites
    public delegate void LoadspriteEvent(LoadSprite currentLoadSrite, bool IsForeground);
    public static LoadspriteEvent OnCreateLoadTileForLoadSprite;

    //foreground/Background
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
            }
        }
    }

    public static bool IsCanvasControllable = true;


    //tiles
    public delegate void TileInteractionStateEvent(TileScript.TileInteractionState State);
    public static TileInteractionStateEvent OnForegroundTilesInteractionStateChanged;
    public static TileInteractionStateEvent OnBackgroundTilesInteractionStateChanged;

    //Help text
    private static bool _isHelpEnabled = true;
    public static bool isHelpEnabled { get { return _isHelpEnabled; } set
        {
            if (value != _isHelpEnabled && OnSetHelpActive != null)
            {
                OnSetHelpActive.Invoke(value);
            }

            _isHelpEnabled = value;


        } 
    }
    
    public delegate void EnableDisplayHelpEvent(bool isEnable);
    public static EnableDisplayHelpEvent OnSetHelpActive;

    public delegate void DisplayHelpEvent(HelpText currentHelpText);
    public static DisplayHelpEvent OnDisplayHelp;
    public static DisplayHelpEvent OnEndDisplayHelp;

    //Brush
    public delegate void BrushEvent(Texture2D brush);
    public static BrushEvent OnBrushAssigned;

}
