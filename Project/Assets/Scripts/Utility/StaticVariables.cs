using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class StaticVariables {

    public static int AreaSize = 2500;
    
    public static float AreaHeight { get {
            return AreaSize;
        }
    }

    public static float AreaWidth{ get {
            return AreaSize * Camera.main.aspect;
        }
    }
}
