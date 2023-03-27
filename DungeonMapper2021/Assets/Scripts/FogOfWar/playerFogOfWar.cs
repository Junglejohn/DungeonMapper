using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class playerFogOfWar : MonoBehaviour {

    public FogOfWarCanvas fogOfWar;

    public Canvas playerFogCanvas;

    private Image fogImage;

    private int fogLayerIndex = 9;

	public void CreateFog()
    {
        if (fogImage != null)
        {
            Debug.LogError("Player Fog is Already Created");

            return;
        }


        if (fogOfWar == null)
        {
            fogOfWar = gameObject.GetComponent<FogOfWarCanvas>();

            if (fogOfWar == null)
            {
                Debug.LogError("PlayerFog: No fogOfWar is assigned. cant create player copy");
                return;
            }
        }

        GameObject fogObject = Instantiate(new GameObject(), fogOfWar.fogImage.transform.position, fogOfWar.fogImage.transform.rotation, playerFogCanvas.transform);

        fogObject.transform.localScale = fogOfWar.fogImage.transform.localScale;

        fogObject.AddComponent<RectTransform>();

        fogImage = fogObject.AddComponent<Image>();
        fogImage.rectTransform.sizeDelta = fogOfWar.fogImage.rectTransform.sizeDelta;
        fogImage.raycastTarget = false;

        fogObject.layer = fogLayerIndex;
        
        UpdateFog(fogOfWar);

        fogOfWar.OnFogValueChanged += UpdateFog;

    }

    public void UpdateFog(FogOfWarCanvas currentFog)
    {
        fogImage.sprite = currentFog.fogImage.sprite;

        fogImage.enabled = currentFog.IsShowFog;


    }


}
