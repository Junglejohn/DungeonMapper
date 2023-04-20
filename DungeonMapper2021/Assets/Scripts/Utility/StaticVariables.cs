using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public static class StaticVariables {

    #region ButtonNameRefs

    public static string AcceptButtonName = "Accept";

    public static string CancelButtonName = "Cancel";

    public static string IncrementButtonName = "Increment";

    public static string MoveButtonName = "Move";

    public static string RotateButtonName = "Rotate";

    public static string ScaleButtonName = "Scale";

    #endregion


    #region Session

    public static void SaveCurrentSession()
    {
        if (currentSession != null)
        {
            currentSession.SaveSession();
        }
    }

    private static Session _currentSession = null;
    public static Session currentSession { get { return _currentSession; } set
        {
            _currentSession = value;

            currentSessionName = _currentSession.Name;

            AreaSize = _currentSession.AreaSize;


            /*
            if (EventManager.OnRefreshSession != null)
            {
                EventManager.OnRefreshSession.Invoke(_currentSession);
            }
            */

            if (EventManager.OnSessionRefresh != null)
            {
                EventManager.OnSessionRefresh.Invoke(_currentSession);
            }

        } 
    }

    private const string _defaultSessionName = "default_Session";
    public static string currentSessionName { get {
            if (currentSession != null)
            {
                return currentSession.Name;
            } else
            {
                return _defaultSessionName;
            }
        } set {

            if (currentSession != null)
            {
                currentSession.Name = _defaultSessionName;
            }

            if (EventManager.OnSessionNameUpdate != null)
            {
                EventManager.OnSessionNameUpdate.Invoke(value);
            }
        } 
    }

    #endregion

    #region BoardArea

    public const int defaultAreaWidth = 2500;
    public const int defaultAreaHeight = 1000;

    public const int minAreaHeight = 500;
    public const int minAreaWidth = 1500;

    public const int maxAreaHeight = 5000;
    public const int maxAreaWidth = 5000;

    public static int clampedAreaWidth(int currentWidth)
    {
        return Mathf.Clamp(currentWidth, minAreaWidth, maxAreaWidth);
    }

    public static int clampedAreaHeight(int currentHeight)
    {
        return Mathf.Clamp(currentHeight, minAreaHeight, maxAreaHeight);
    }

    public static Vector2 clampedAreaSize(Vector2 currentAreaSize)
    {
        return new Vector2(
                clampedAreaWidth((int)currentAreaSize.x),
                clampedAreaHeight((int)currentAreaSize.y)
                );
    }

    private static Vector2 _areaSize = new Vector2(2500, 1000);
    public static Vector2 AreaSize { get {
            if (currentSession != null)
            {
                return currentSession.AreaSize;
            } else
            {
                return _areaSize;
            }
            
        } set
        {
            Vector2 clampedValue = clampedAreaSize(value);

            if (currentSession != null)
            {
                currentSession.AreaSize = clampedValue;
            }

            _areaSize = clampedValue;

            if (EventManager.OnBackgroundSizeChanged != null)
            {
                EventManager.OnBackgroundSizeChanged.Invoke(clampedValue);
            }


        }
    }

    public static float AreaHeight { get {
            return AreaSize.y;
        }
    }

    public static float AreaWidth{ get {
            return AreaSize.x;
        }
    }
    

    public static int borderSize = 100;

    #endregion

    #region PlayerPrefs

    static PreferencesData _preferences = null;
    public static PreferencesData Preferences { get
        {
            if (_preferences == null)
            {
                _preferences = PreferencesData.GetPreferences();
            }

            return _preferences;
        } set
        {
            _preferences = value;

        }
    }

    #endregion

}
