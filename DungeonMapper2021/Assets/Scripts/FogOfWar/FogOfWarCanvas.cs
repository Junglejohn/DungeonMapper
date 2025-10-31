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

            if (StaticVariables.currentSession != null)
            {
                return StaticVariables.currentSession.fogOfWar.sprite;
            } else
            {
                return _sprite;
            }
        }
        set {

            if (StaticVariables.currentSession != null)
            {
                StaticVariables.currentSession.fogOfWar.sprite = value;
            }

            _sprite = value;

        }
    }

    public bool AllowFogOfWarEditing()
    {
        if (IsShowFog && EventManager.IsCanvasControllable && !EventManager.IsEditingTile)
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
            if (StaticVariables.currentSession.fogOfWar != null)
            {
                StaticVariables.currentSession.fogOfWar.IsShown = value;
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

    public void FillFogArea(bool isHiddenColor)
    {

        if (sprite == null)
        {
            Debug.Log("No sprite is assigned or generated for fog of war. Cannot fill the area");

            return;
        }

        Color[] colors = new Color[(sprite.texture.width) * (sprite.texture.height)];

        //choose color
        Color currentColor = revealedColor;
        if (isHiddenColor)
        {
            currentColor = hiddenColor;
        }
        
        //assign color to pixels
        for (int i = 0; i < colors.Length; ++i)
        {
            colors[i] = currentColor;
        }

        //assign pixels to texture
        sprite.texture.SetPixels(colors);

        //make texture show new pixels
        sprite.texture.Apply();
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

        //subscribe to brushes being changed.
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


    private int editMouseButtonIndex = 2;
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
            if (Input.GetMouseButtonDown(editMouseButtonIndex))
            {
                while (IsShowFog && AllowFogOfWarEditing() && Input.GetMouseButton(editMouseButtonIndex))
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


                    yield return null;
                }
            }


            yield return null;

        }

        editFogIsRunning = false;


    }
    
    public void CreateFog(Session currentSession)
    {

        if (currentSession == null)
        {
            Debug.Log("No Session assigned. Could not create fog of war");

            return;
        }
        
        if (currentSession.fogOfWar == null)
        {
            currentSession.fogOfWar = new FogOfWar();
        }
        
        Texture2D texture = new Texture2D(1, 1);

        //if the current session not already have generated a fog texture to load, then generate one
        if (currentSession.fogOfWar.sprite == null) 
        {
            texture = new Texture2D((int)currentSession.AreaSize.x, (int)currentSession.AreaSize.y);

            Color[] colors = new Color[(texture.width) * (texture.height)];

            for (int i = 0; i < colors.Length; ++i)
            {
                colors[i] = hiddenColor;
            }

            texture.SetPixels(colors);

            sprite = Sprite.Create(texture, new Rect(new Vector2(0, 0), new Vector2((int)currentSession.AreaSize.x, (int)currentSession.AreaSize.y)), new Vector2(0.5f, 0.5f), 1);

            fogImage.sprite = sprite;
            
            fogImage.rectTransform.sizeDelta = new Vector2((int)currentSession.AreaSize.x, (int)currentSession.AreaSize.y);

            Debug.LogError("Created sprite. Size is: " + sprite.rect);

            texture.Apply();

            SetFogVisibility(false);

        } else
        {

            //if session area was resized. Fog of war needs to be resized to fit. 'Trim excess'/'add extra fog' pixels of image but keep fog in the same place
            if(currentSession.fogOfWar.sprite.texture.width != (int)currentSession.AreaSize.x || currentSession.fogOfWar.sprite.texture.height != (int)currentSession.AreaSize.y)
            {
                Debug.LogError("fog of war was adjusted to fit area size. Size of Area: " + currentSession.AreaSize);

                texture = new Texture2D((int)currentSession.AreaSize.x, (int)currentSession.AreaSize.y);

                Color[] colors = new Color[(int)currentSession.AreaSize.x * (int)currentSession.AreaSize.y];


                int differenceX = (int)currentSession.AreaSize.x - currentSession.fogOfWar.sprite.texture.width;
                int differenceY = (int)currentSession.AreaSize.y - currentSession.fogOfWar.sprite.texture.height;

                int pixelShiftX = differenceX / 2;
                int pixelShiftY = differenceY / 2;


                for (int y = 0; y < (int)currentSession.AreaSize.y; y++)
                {
                    for (int x = 0; x < (int)currentSession.AreaSize.x; x++)
                    {
                        if (x >= pixelShiftX && y >= pixelShiftY && x < (int)currentSession.AreaSize.x - pixelShiftX && y < (int)currentSession.AreaSize.y - pixelShiftY)
                        {
                            colors[(int)currentSession.AreaSize.x * y + x] = currentSession.fogOfWar.sprite.texture.GetPixel(x - pixelShiftX, y - pixelShiftY);
                        }
                        else
                        {
                            //if Area size is larger than the fog of war sprite resolution fill in the remainder with 'hidden color'
                            colors[(int)currentSession.AreaSize.x * y + x] = hiddenColor;
                        }
                    }
                }

                texture.SetPixels(colors);

                sprite = Sprite.Create(texture, new Rect(new Vector2(0, 0), new Vector2((int)currentSession.AreaSize.x, (int)currentSession.AreaSize.y)), new Vector2(0.5f, 0.5f), 1);

                

            } else //if fog was not resized just use the fog of war image from the current session
            {
                Debug.LogError("fog of war not adjusted. Size of Area: " + currentSession.AreaSize + ". Sprite size is: " + new Vector2(currentSession.fogOfWar.sprite.texture.width, currentSession.fogOfWar.sprite.texture.height));

                sprite = Sprite.Create(sprite.texture, new Rect(new Vector2(0, 0), new Vector2(sprite.texture.width, sprite.texture.height)), new Vector2(0.5f, 0.5f), 1);

            }

            fogImage.sprite = sprite;

            fogImage.rectTransform.sizeDelta = new Vector2((int)currentSession.AreaSize.x, (int)currentSession.AreaSize.y);

            Debug.Log("Loaded sprite from session: "+currentSession.Name+". Size is: " + sprite.rect);

            texture.Apply();

            SetFogVisibility(currentSession.fogOfWar.IsShown);
        }

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




