using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FogOfWarCanvas : MonoBehaviour {

    public delegate void FogOfWarEvent(FogOfWarCanvas currentFog);
    public FogOfWarEvent OnFogValueChanged;
    
    public Camera cam;
    
    public Image fogImage;

    private Sprite _sprite;
    public Sprite sprite { get {

            if (Manager.currentManager != null && Manager.currentManager.session != null)
            {
                return Manager.currentManager.session.fogOfWar.sprite;
            } else
            {
                return _sprite;
            }
        }
        set {

            if (Manager.currentManager != null && Manager.currentManager.session != null)
            {

                Manager.currentManager.session.fogOfWar.sprite = value;
            }

            _sprite = value;

        }
    }

    public bool AllowFogOfWarEditing()
    {
        if (IsShowFog && EventManager.IsCanvasControllable)
        {
            return true;
        } else
        {
            return false;
        }
    }

    private bool _isShowFog = false;
    public bool IsShowFog { get {
            return _isShowFog;
        } set {


            _isShowFog = value;
            if (Manager.currentManager != null && Manager.currentManager.session.fogOfWar != null)
            {
                Manager.currentManager.session.fogOfWar.IsShown = value;
            }

            if (fogImage != null)
            {
                fogImage.enabled = value;
            }

            if (value == true)
            {
                StartCoroutine(EditFogCoroutine());
            }

            if (OnFogValueChanged != null)
            {
                OnFogValueChanged.Invoke(this);
            }

        }
    }

    public void SetFogVisibility(bool IsShow)
    {
        IsShowFog = IsShow;

    }

    public void ToggleFogVisibility()
    {
        IsShowFog = !IsShowFog;
    }

    bool IsPaintReveal = true;
    public void ToggleColor()
    {
        IsPaintReveal = !IsPaintReveal;
    }

    public Color currentColor { get
        {
            if (IsPaintReveal)
            {
                return revealedColor;
            } else
            {
                return hiddenColor;
            }
        }
    }
    public Color hiddenColor = new Color(0,0,0, 255);
    public Color revealedColor = new Color(0, 0, 0, 0);
    
    void Awake()
    {

        //subscribe to brushes being changed
        EventManager.OnBrushAssigned += AssignBrush;

        //figure out a camera to use for editing fog. The assigned Camera if able, otherwise try the main camera.
        if (cam == null)
        {
            if (Camera.main != null)
            {
                cam = Camera.main;
            } else
            {
                Debug.Log("No cameraScript in scene. Assign cameraScript to use fogOfWar");
                return;
            }
        }

        EventManager.OnSessionRefresh += CreateFog;
        
    }

    

    public Texture2D brushImage;
    public void AssignBrush(Texture2D newBrushTexture)
    {
        brushImage = newBrushTexture;
    }


    [Range(0, 1)]
    public float AlphaCutoff = 0.1f;
    bool brushTexturePixel(int brushSize, int xCoord, int yCoord)
    {
        if (xCoord < 0 || xCoord > brushSize || yCoord < 0 || yCoord > brushSize)
        {
            return false;
        }

        if (brushImage == null)
        {
            return true;
        }

        float brushRelationX = (float)xCoord / (float)brushSize;
        float brushRelationY = (float)yCoord / (float)brushSize;


        float imageCoordX = brushRelationX * brushImage.width;
        float imageCoordY = brushRelationY * brushImage.height;
        //Debug.Log(brushImage.width);
        //Debug.Log("Brush Size: " + brushSize + ", brushCoord: "+ new Vector2(xCoord,yCoord) +", brushRelation: " + new Vector2(brushRelationX,brushRelationY) + ", imageCoord: " + new Vector2( imageCoordX, imageCoordY));


        Color pixelColor = brushImage.GetPixel((int)imageCoordX,(int)imageCoordY);

        if (pixelColor.a > AlphaCutoff)
        {
            return true;
        } else
        {
            return false;
        }

    }

    bool editFogIsRunning = false;
    IEnumerator EditFogCoroutine()
    {
        if (editFogIsRunning)
        {
            yield break;
        }

        editFogIsRunning = true;

        while (IsShowFog)
        {
            //Debug.Log("fog can be edited");

            if (AllowFogOfWarEditing() && Input.GetMouseButton(1))
            {
                Texture2D tex = sprite.texture;

                Rect r = fogImage.rectTransform.rect;

                Vector2 localPoint;
                RectTransformUtility.ScreenPointToLocalPointInRectangle(fogImage.rectTransform, Input.mousePosition, cam, out localPoint);

         
                int px = Mathf.Clamp((int)(((localPoint.x - r.x) * tex.width) / r.width), 0, tex.width);
                int py = Mathf.Clamp((int)(((localPoint.y - r.y) * tex.height) / r.height), 0, tex.height);

                //coordinates on fog of war texture to start and end looping. Clamped because coordinates below zero and higher than fogTexture.width causes unwanted behavior.
                int clampedStartPointX = Mathf.Clamp(px - StaticVariables.Preferences.BrushSize, 0, tex.width);
                int clampedEndPointX = Mathf.Clamp(px + StaticVariables.Preferences.BrushSize, 0, tex.width);

                int clampedStartPointY = Mathf.Clamp(py - StaticVariables.Preferences.BrushSize, 0, tex.height);
                int clampedEndPointY = Mathf.Clamp(py + StaticVariables.Preferences.BrushSize, 0, tex.height);


                for (int y = clampedStartPointY; y < clampedEndPointY; y++)
                {
                    for (int x = clampedStartPointX; x < clampedEndPointX; x++)
                    {
                        if (brushTexturePixel(StaticVariables.Preferences.BrushSize * 2, x + StaticVariables.Preferences.BrushSize - px, y + StaticVariables.Preferences.BrushSize - py))
                        {
                            tex.SetPixel(x, y, currentColor);
                        }

                        
                    }

                }

                tex.Apply();

                if (OnFogValueChanged != null)
                {
                    OnFogValueChanged.Invoke(this);
                }

                //Color32 col = tex.GetPixel(px, py);
            }


            yield return null;

        }

        editFogIsRunning = false;


    }
    
    public void CreateFog(Session currentSession)
    {
        
        if (currentSession.fogOfWar == null)
        {
            currentSession.fogOfWar = new FogOfWar();
        }
        
        Texture2D texture = new Texture2D(1, 1);

        if (currentSession.fogOfWar.sprite == null)
        {
            texture = new Texture2D((int)StaticVariables.AreaWidth, (int)StaticVariables.AreaHeight);

            Color[] colors = new Color[(texture.width) * (texture.height)];

            for (var i = 0; i < colors.Length; ++i)
            {
                colors[i] = hiddenColor;
            }

            texture.SetPixels(colors);

            sprite = Sprite.Create(texture, new Rect(new Vector2(0, 0), new Vector2((int)StaticVariables.AreaWidth, (int)StaticVariables.AreaHeight)), new Vector2(0.5f, 0.5f), 1);

            fogImage.sprite = sprite;
            
            fogImage.rectTransform.sizeDelta = new Vector2((int)StaticVariables.AreaWidth, (int)StaticVariables.AreaHeight);

            //spriteRend.sprite = sprite;

            Debug.Log("Created sprite. Size is: " + sprite.rect);

            texture.Apply();

            SetFogVisibility(false);

        } else
        {

            sprite = Sprite.Create(sprite.texture, new Rect(new Vector2(0, 0), new Vector2(sprite.texture.width, sprite.texture.height)), new Vector2(0.5f, 0.5f), 1);

            //spriteRend.sprite = sprite;

            fogImage.sprite = sprite;

            fogImage.rectTransform.sizeDelta = new Vector2((int)StaticVariables.AreaWidth, (int)StaticVariables.AreaHeight);

            Debug.Log("Loaded sprite from session: "+currentSession.Name+". Size is: " + sprite.rect);

            texture.Apply();

            SetFogVisibility(currentSession.fogOfWar.IsShown);
        }

        //gameObject.AddComponent<BoxCollider2D>();

        if (OnFogValueChanged != null)
        {
            OnFogValueChanged.Invoke(this);
        }

    }
    
    private void OnDisable()
    {
        EventManager.OnSessionRefresh -= CreateFog;
    }

}




