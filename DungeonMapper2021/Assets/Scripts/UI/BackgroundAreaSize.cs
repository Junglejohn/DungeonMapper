using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BackgroundAreaSize : MonoBehaviour {

    public Image boardArea;
    public Image backgroundArea;
    public Image borderArea;

    public int borderSize = 50;

    private void Awake()
    {
        UpdateBackgroundArea(StaticVariables.AreaSize);
    }

    private void OnEnable()
    {
        EventManager.OnBackgroundSizeChanged += UpdateBackgroundArea;
    }

    private void OnDisable()
    {
        EventManager.OnBackgroundSizeChanged -= UpdateBackgroundArea;
    }


    void UpdateBackgroundArea(Vector2 currentAreaSize)
    {

        if (boardArea != null)
        {
            boardArea.rectTransform.sizeDelta = currentAreaSize;
            boardArea.transform.position = new Vector2(0,0);

            //display world corners in worldspace
            Vector3[] v = new Vector3[4];
            boardArea.rectTransform.GetWorldCorners(v);
            for (var i = 0; i < 4; i++)
            {
                Debug.Log("World Corner " + i + " : " + v[i]);
            }

            Debug.Log("Updating backgroundBorderSize. New Size is: " + boardArea.rectTransform.rect);

        } else
        {
            Debug.Log("BackgroundArea size was not updated - no area sprite is assigned");
        }

        if (backgroundArea != null)
        {
            backgroundArea.rectTransform.sizeDelta = new Vector2(currentAreaSize.x + StaticVariables.borderSize * 2, currentAreaSize.y + StaticVariables.borderSize * 2);
            backgroundArea.transform.position = new Vector2(0, 0);
        }

        if (borderArea != null)
        {
            borderArea.rectTransform.sizeDelta = new Vector2((currentAreaSize.x + borderSize) / borderArea.gameObject.transform.lossyScale.x, (currentAreaSize.y + borderSize) / borderArea.gameObject.transform.lossyScale.y);
            borderArea.transform.position = new Vector2(0, 0);
        }



    }

}
