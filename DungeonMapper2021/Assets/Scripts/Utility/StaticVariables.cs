using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class StaticVariables {


    #region BoardArea

    public static int AreaSize = 2500;
    
    public static float AreaHeight { get {
            return AreaSize;
        }
    }

    public static float AreaWidth{ get {
            return AreaSize * Camera.main.aspect;
        }
    }

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
