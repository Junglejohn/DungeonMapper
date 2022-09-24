using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FogOfWarCanvas : MonoBehaviour {

    public delegate void FogOfWarEvent(FogOfWarCanvas currentFog);
    public FogOfWarEvent OnFogValueChanged;
    
    public CameraScript cam;
    
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
        //cam = Camera.main;

        if (cam == null)
        {
            cam = FindObjectOfType<CameraScript>();

            if (cam == null)
            {
                Debug.Log("No cameraScript in scene. Assign cameraScript to use fogOfWar");
                return;
            }
        }

        EventManager.OnSessionRefresh += CreateFog;
        
    }

    private int _brushSize = 45;
    public int BrushSize { get {
            return _brushSize;
        } set {
            _brushSize = (int)Mathf.Clamp(value, 5, 105);

        }
    }
    public void ChangeBrushSize(bool IsAdd)
    {
        if (IsAdd)
        {
            BrushSize += 5;
        } else
        {
            BrushSize -= 5;
        }
    }

    //public ContactFilter2D fogOfWarFilter;
    //public LayerMask fogOfWarMask;

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
                RectTransformUtility.ScreenPointToLocalPointInRectangle(fogImage.rectTransform, Input.mousePosition, Camera.main, out localPoint);

                int px = Mathf.Clamp(0, (int)(((localPoint.x - r.x) * tex.width) / r.width), tex.width);
                int py = Mathf.Clamp(0, (int)(((localPoint.y - r.y) * tex.height) / r.height), tex.height);

                for (int y = py - BrushSize; y < py + BrushSize; y++)
                {
                    for (int x = px - BrushSize; x < px + BrushSize; x++)
                    {
                        tex.SetPixel(x, y, currentColor);
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

//spritefog
/*
 public void CreateFog(Session currentSession)
    {
        
        if (currentSession.fogOfWar == null)
        {
            currentSession.fogOfWar = new FogOfWar();
        }
        
        spriteRend = gameObject.AddComponent<SpriteRenderer>();
        spriteRend.sortingOrder = 1;

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

            spriteRend.sprite = sprite;

            Debug.Log("Created sprite. Size is: " + sprite.rect);

            texture.Apply();

            SetFogVisibility(false);

        } else
        {

            sprite = Sprite.Create(sprite.texture, new Rect(new Vector2(0, 0), new Vector2((int)StaticVariables.AreaWidth, (int)StaticVariables.AreaHeight)), new Vector2(0.5f, 0.5f), 1);

            spriteRend.sprite = sprite;

            Debug.Log("Loaded sprite from session: "+currentSession.Name+". Size is: " + sprite.rect);

            texture.Apply();

            SetFogVisibility(currentSession.fogOfWar.IsShown);
        }
        
        gameObject.AddComponent<BoxCollider2D>();
        
    }

*/

    //spriteFogPos
/*
 
        void Update()
    {

        if (!AllowFogOfWarEditing() || !Input.GetMouseButton(1))
            return;

        //Debug.Log("Hit");

        RaycastHit2D[] hit = new RaycastHit2D[1];
        if (Physics2D.Raycast(Input.mousePosition, new Vector2(), fogOfWarFilter, hit) <= 0)
            return;

        

        SpriteRenderer rend = null;
        int index = 0;

        for (int i = 0; i < hit.Length; i++)
        {
            rend = hit[i].transform.GetComponent<SpriteRenderer>();
            if (rend != null)
            {
                index = i;

                continue;
            }

        }

        if (rend == null)
        {
            return;
        }

        //Debug.Log("Hit at " + hit[index].point);
        
        Rect rect = rend.sprite.rect;

        Vector2 LocalPos = cam.CameraPos - new Vector2(rect.center.x, rect.center.y);

        float xCamPosAspect = ((Input.mousePosition.x / cam.cam.pixelWidth) * (cam.cam.orthographicSize * cam.cam.aspect));
        float yCamPosAspect = ((Input.mousePosition.y / cam.cam.pixelHeight) * (cam.cam.orthographicSize));



        float camWidth = (cam.cam.orthographicSize * cam.cam.aspect);

        Vector2 pos = new Vector2(xCamPosAspect * 2 + LocalPos.x, yCamPosAspect * 2 + LocalPos.y);

        

        Debug.Log("fogOfWar hit at " +hit[index].point + ". Bounds Center is: "+ rect.center + ".  LocalPos: " +LocalPos);
        //Debug.Log(Input.mousePosition);


        Texture2D tex = rend.sprite.texture;

        for (int y = (int)pos.y - BrushSize; y < (int)pos.y + BrushSize; y++)
        {
            for (int x = (int)pos.x - BrushSize; x < (int)pos.x + BrushSize; x++)
            {
                tex.SetPixel(x, y, currentColor);
            }
            
        }
        

        tex.Apply();

        //Debug.Log("Hit4");
    }

  */

/*
void Update()
{
    if (!Input.GetMouseButtonDown(0))
        return;

    Debug.Log("Hit");

    RaycastHit hit;
    if (!Physics.Raycast(cam.ScreenPointToRay(Input.mousePosition), out hit))
        return;

    Debug.Log("Hit2");

    Renderer rend = hit.transform.GetComponent<Renderer>();
    MeshCollider meshCollider = hit.collider as MeshCollider;

    if (rend == null || rend.sharedMaterial == null || rend.sharedMaterial.mainTexture == null || meshCollider == null)
        return;

    Debug.Log("Hit3");

    Texture2D tex = rend.material.mainTexture as Texture2D;
    Vector2 pixelUV = hit.textureCoord;
    pixelUV.x *= tex.width;
    pixelUV.y *= tex.height;

    tex.SetPixel((int)pixelUV.x, (int)pixelUV.y, Color.black);
    tex.Apply();

    Debug.Log("Hit4");
}

*/



