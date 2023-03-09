using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class TileScript : MonoBehaviour {

    public LoadTile currentLoadTile;

    Vector2 _position = Vector2.zero; //not used when saving
    public Vector2 Position { get {
            if (currentLoadTile != null)
            {
                return currentLoadTile.pos;
            } else
            {
                return _position;
            }
                }
        set {

            float horizontal = StaticVariables.AreaWidth / 2 - image.rectTransform.sizeDelta.x / 2;
            float vertical = StaticVariables.AreaHeight / 2 - image.rectTransform.sizeDelta.y / 2;


            Vector2 clampedPos = new Vector2(
                Mathf.Clamp(value.x, -horizontal, horizontal),
                Mathf.Clamp(value.y, -vertical, vertical)
                );


            if (currentLoadTile != null)
            {
                currentLoadTile.pos = clampedPos;
                gameObject.transform.position = clampedPos;

            } else
            {
                _position = clampedPos;
                gameObject.transform.position = clampedPos;
            }
        }
    }

    private bool _aspectAssigned = false;
    private float _xSpriteAspectRatio;
    public float XSpriteAspectRatio { get {

            if (_aspectAssigned)
            {
                return _xSpriteAspectRatio;
            } else
            {
                if (currentLoadTile != null && currentLoadTile.loadSpriteInfo != null && currentLoadTile.loadSpriteInfo.sprite != null)
                {
                    _xSpriteAspectRatio = currentLoadTile.loadSpriteInfo.sprite.texture.width / (float)currentLoadTile.loadSpriteInfo.sprite.texture.height;
                    Debug.Log("Loadtile name: " + currentLoadTile.loadSpriteInfo.Name+ "texture width: " + currentLoadTile.loadSpriteInfo.sprite.texture.width + ". texture height: "+ currentLoadTile.loadSpriteInfo.sprite.texture.height + ". Aspect: " + _xSpriteAspectRatio);

                    _aspectAssigned = true;
                    return XSpriteAspectRatio;
                }
                else
                {
                    _xSpriteAspectRatio = 1;
                    _aspectAssigned = true;

                    return _xSpriteAspectRatio;
                }

            }
            
        }
    }

    int minSize = 40;
    int maxSize = 1000;
    Vector2 _size = new Vector2(1,1);
    public Vector2 Size {
        get {
            if (currentLoadTile != null)
            {
                return currentLoadTile.size;
            } else
            {
                return _size;
            }
        }
        set {

            Vector2 clampedValue = new Vector2(Mathf.Clamp(value.x, minSize, maxSize * XSpriteAspectRatio), Mathf.Clamp(value.y, minSize, maxSize));

            Debug.Log("maxSizeX = " + maxSize * XSpriteAspectRatio + ". clampedValue.x = " + clampedValue.x);

            if (currentLoadTile != null)
            {
                
                currentLoadTile.size = clampedValue;
                image.rectTransform.sizeDelta = clampedValue;
            } else
            {
                _size = clampedValue;
                image.rectTransform.sizeDelta = clampedValue;
            }
        }
    }

    int _angle = 0;
    public int Angle { get {
            if (currentLoadTile != null)
            {
                return currentLoadTile.Angle;
            } else
            {
                return _angle;
            }
            

        } set {

            if (currentLoadTile != null)
            {
                currentLoadTile.Angle = value;
            }
            
            _angle = value;
            gameObject.transform.rotation = Quaternion.Euler(new Vector3(0,0, value));

        }

    }


    private string _name = "";
    public string Name { get { return _name; } set {
            _name = value;
            if (NameText != null)
            {
                NameText.text = _name;
            }
        }
    }
    public Text NameText;

    private Sprite _sprite;
    public Sprite sprite
    {
        get { return _sprite; }
        set
        {
            _sprite = value;
            if (image != null)
            {
                image.sprite = _sprite;
            }
        }
    }
    public Image image;

    public Button DeleteButton;
    public Button ScaleButton;
    public Button RotateButton;

    public void InitializeTile(LoadTile loadTile)
    {
        currentLoadTile = loadTile;

        if (currentLoadTile != null && currentLoadTile.loadSpriteInfo != null)
        {
            Name = currentLoadTile.loadSpriteInfo.Name;
            sprite = currentLoadTile.loadSpriteInfo.sprite;
            Position = loadTile.pos;
            Angle = loadTile.Angle;

            Size = loadTile.size;
            currentLoadTile.OnDeleted += DeleteTile;
            currentLoadTile.OnTileObjectsDeleted += DeleteTileObject;

        } else
        {
            Name = "Missing Ref";
            Position =Camera.main.ViewportToScreenPoint(new Vector2(Screen.width/2, Screen.height/2));
            Size = new Vector2(50, 50);

            Debug.LogError("Supplied loadTile, or the associated loadsprite, was null. Tile was not initialized properly");
        }

    }
    
    public void InitializeTile(string TileName, Sprite TileSprite) //used only for testing - no save available with this
    {
        Name = TileName;
        sprite = TileSprite;
    }
    
    public void MoveTile()
    {
        if (!IsRunning && Camera.main != null)
        {
            Debug.Log("Start Moving Tile");

            StartCoroutine(MoveImage());

        } else
        {
            Debug.LogWarning("No Main camera is assigned. cannot move tile");
        }

    }

    IEnumerator MoveImage()
    {
        if (IsRunning)
        {
            yield break;
        }

        IsRunning = true;

        Vector2 initialMousPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        Vector2 posCorrection = new Vector2(initialMousPos.x - Position.x, initialMousPos.y - Position.y);

        while (Input.GetMouseButton(0))
        {

            //setting position moves gameobject
            Position = (Vector2)Camera.main.ScreenToWorldPoint(Input.mousePosition) - posCorrection;
            yield return null;
        }

        IsRunning = false;
    }

    public void DeleteTile()
    {
        if (!IsRunning)
        {
            Debug.Log("Deleting Tile");

            if (currentLoadTile != null && currentLoadTile.loadSpriteInfo != null && currentLoadTile.loadSpriteInfo.LoadTileList.Contains(currentLoadTile))
            {
                currentLoadTile.loadSpriteInfo.DeleteTile(currentLoadTile);

                try
                {
                    currentLoadTile.OnDeleted -= DeleteTile;
                } catch { }
             
            }

            Destroy(gameObject);
        }
        
    }

    public void ScaleTile()
    {
        Debug.Log("Scaling Tile");

        StartCoroutine(scaleImage());
    }

    public static bool IsRunning;
    private IEnumerator scaleImage()
    {
        if (IsRunning)
        {
            yield break;
        }

        IsRunning = true;

        

        if (Camera.main != null)
        {

            Vector2 initialMousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            Vector2 initialSize = new Vector2(Size.x, Size.y);

            while (IsRunning)
            {
                if (Input.GetMouseButtonDown(1))
                {
                    IsRunning = false;
                }

                Vector2 currentMousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);

                //setting Size scales tile
                Size = new Vector2(
                    initialSize.x + (currentMousePos.x - initialMousePos.x) * Camera.main.aspect,
                    initialSize.y - (currentMousePos.y - initialMousePos.y)*2
                    );

                yield return null;
            }
        } else
        {
            Debug.LogWarning("No main camera in scene. Scale tile function aborted");
        }



        IsRunning = false;
    }

    public void RotateTile()
    {
        StartCoroutine(RotateImage());
    }
    
    private IEnumerator RotateImage()
    {
        if (IsRunning)
        {
            yield break;
        }

        IsRunning = true;

        if (Camera.main != null)
        {

            int startAngle = Angle;
            Vector3 mousePos = Input.mousePosition;
            while (IsRunning)
            {
                if (Input.GetMouseButtonDown(1))
                {
                    IsRunning = false;
                }

                Debug.Log(Input.mousePosition);
               
                //setting Angle Rotates tile
                Angle = startAngle - (int)Vector2.SignedAngle(
                    Input.mousePosition - Camera.main.WorldToScreenPoint(gameObject.transform.position),
                    mousePos - Camera.main.WorldToScreenPoint(gameObject.transform.position)
                    );

                yield return null;
            }
        } else
        {
            Debug.LogWarning("No main camera in scene Rotate tile function aborted");
        }



        IsRunning = false;
    }
    
    public void ShowName()
    {
        NameText.enabled = true ;
        if (DeleteButton != null)
        {
            DeleteButton.gameObject.SetActive(true);
        }

        if (ScaleButton != null)
        {
            ScaleButton.gameObject.SetActive(true);
        }

        if (RotateButton != null)
        {
            RotateButton.gameObject.SetActive(true);
        }
    }

    public void HideName()
    {
        NameText.enabled = false;

        if (DeleteButton != null)
        {
            DeleteButton.gameObject.SetActive(false);
        }

        if (ScaleButton != null)
        {
            ScaleButton.gameObject.SetActive(false);
        }

        if (RotateButton != null)
        {
            RotateButton.gameObject.SetActive(false);
        }
    }

    public void DeleteTileObject()
    {

        currentLoadTile.OnTileObjectsDeleted -= DeleteTileObject;
        Destroy(this.gameObject);
    }

    public enum TileInteractionState { Active, NoInteraction, Hidden, SemiTransparentNoInteraction}
    public TileInteractionState currentInteractionState = TileInteractionState.Active;

    public Animator tileAnim;
    public EventTrigger tileEventSystem;
    public void SetTileInteractionState(TileInteractionState currentState)
    {
        

        Debug.Log("Button interaction for button named:" +Name+" was changed to " + currentState.ToString());

        if (tileAnim != null && currentInteractionState != currentState)
        {
            switch (currentState)
            {
                case TileInteractionState.Active:

                    if (tileEventSystem != null)
                    {
                        tileEventSystem.enabled = true;
                    }

                    tileAnim.SetTrigger("Activate");
                    Debug.Log("TileInteractinState set to Active");

                    break;

                case TileInteractionState.NoInteraction:

                    if (tileEventSystem != null)
                    {
                        tileEventSystem.enabled = false;
                    }


                    tileAnim.SetTrigger("NoInteraction");



                    Debug.Log("TileInteractinState set to NoInteraction");

                    break;

                case TileInteractionState.Hidden:

                    tileAnim.SetTrigger("Hide");

                    if (tileEventSystem != null)
                    {
                        tileEventSystem.enabled = false;
                    }

                    Debug.Log("TileInteractinState set to Hidden");

                    break;

                case TileInteractionState.SemiTransparentNoInteraction:

                    tileAnim.SetTrigger("SemiTransparent");

                    if (tileEventSystem != null)
                    {
                        tileEventSystem.enabled = false;
                    }

                    Debug.Log("TileInteractinState set to SemiTransparent");

                    break;

            }



            
        }

        currentInteractionState = currentState;
    }
    
}
