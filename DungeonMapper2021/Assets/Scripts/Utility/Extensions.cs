using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public static class Extensions 
{

    public static bool isObjectPrefab(this Object currentObject)
    {
        if (PrefabUtility.GetPrefabAssetType(currentObject) == PrefabAssetType.NotAPrefab)
        {
            return false;
        } else
        {
            return true;
        }
    }
}
