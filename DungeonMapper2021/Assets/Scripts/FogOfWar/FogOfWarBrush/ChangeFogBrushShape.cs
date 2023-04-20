using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ChangeFogBrushShape : MonoBehaviour
{
    [SerializeField]
    int _startIndex = 0;


    public int startIndex { get {
            return _startIndex; 
        } set
        {
            _startIndex = value;
            if (useGlobalPreferences)
            {
                StaticVariables.Preferences.BrushIndex = value;
            }

        } 
    }

    public bool useGlobalPreferences = true;

    public BrushListAsset currentBrushes;

    public RawImage displayImage;
    private void UpdateDisplayImage(Texture2D currentBrushTex)
    {
        if (displayImage != null)
        {
            displayImage.texture = currentBrushTex;
        }
    }


    public void Start()
    {
        if (useGlobalPreferences)
        {
            startIndex = StaticVariables.Preferences.BrushIndex;
        }

        AssignNextBrush(0);
    }

    public void AssignNextBrush(int Increment)
    {
        if (EventManager.OnBrushAssigned == null)
        {
            return;
        }

        if (currentBrushes == null)
        {
            Debug.Log("No brushList asset is assigned for Changing fog Brushes. Create from asset menu and assign");
            EventManager.OnBrushAssigned(null);
            UpdateDisplayImage(null);
        } else
        {
            startIndex = getNextIndex(startIndex + Increment);
            EventManager.OnBrushAssigned(getBrushByIndex(startIndex));
            UpdateDisplayImage(getBrushByIndex(startIndex));
        }
        
    }


    public Texture2D getBrushByIndex(int index)
    {
        return currentBrushes.brushes[Mathf.Clamp(index, 0, currentBrushes.brushes.Length - 1)];
    }

    public int getNextIndex(int index)
    {
        int clampedIndex = Mathf.Clamp(index, 0, currentBrushes.brushes.Length);

        if (index >= currentBrushes.brushes.Length)
        {
            return 0;
        }
        else
        {
            return clampedIndex;
        }
    }

}
