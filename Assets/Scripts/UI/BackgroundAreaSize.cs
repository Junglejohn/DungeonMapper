using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BackgroundAreaSize : MonoBehaviour {

    public Image backGroundArea;

    private Vector2 areaSize { get { return new Vector2(StaticVariables.AreaWidth, StaticVariables.AreaHeight); } }

    private void Awake()
    {
        //EventManager.OnRefreshSession += UpdateBackgroundArea;
        UpdateBackgroundArea(null);
    }

    private void OnEnable()
    {
        EventManager.OnRefreshSession += UpdateBackgroundArea;
    }

    private void OnDisable()
    {
        EventManager.OnRefreshSession -= UpdateBackgroundArea;
    }


    void UpdateBackgroundArea(Session currentSession)
    {
        Debug.LogWarning("Updating backgroundBorderSize");

        if (backGroundArea != null)
        {
            backGroundArea.rectTransform.sizeDelta = areaSize;
            backGroundArea.transform.position = new Vector2(0,0);

        } else
        {
            Debug.LogWarning("BackgroundArea size was not updated - no area sprite is assigned");
        }

    }

}
