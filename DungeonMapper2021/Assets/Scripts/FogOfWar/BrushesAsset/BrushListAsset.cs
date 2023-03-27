using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "brushListAsset", menuName = "BrushList")]
public class BrushListAsset : ScriptableObject
{

    public Texture2D[] brushes;

    public Texture2D getBrushByIndex(int index)
    {
        return brushes[Mathf.Clamp(index, 0, brushes.Length - 1 )];
    }

    public Texture2D getNextBrush(int index)
    {
        int clampedIndex = Mathf.Clamp(index, 0, brushes.Length);

        if (index >= brushes.Length)
        {
            return brushes[0];
        } else
        {
            return brushes[clampedIndex];
        }
    }

}
