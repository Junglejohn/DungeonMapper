using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using TMPro;

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
                    //Debug.Log("Loadtile name: " + currentLoadTile.loadSpriteInfo.Name+ "texture width: " + currentLoadTile.loadSpriteInfo.sprite.texture.width + ". texture height: "+ currentLoadTile.loadSpriteInfo.sprite.texture.height + ". Aspect: " + _xSpriteAspectRatio);

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

            //Debug.Log("maxSizeX = " + maxSize * XSpriteAspectRatio + ". clampedValue.x = " + clampedValue.x);

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
    public TMP_Text NameText;
    public GameObject nameFrame;

    public void setShowName(bool isShow)
    {
        if (nameFrame != null)
        {
            nameFrame.SetActive(isShow);
        }

        if (NameText != null)
        {
            NameText.enabled = isShow;
        }
    }

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

    public Button MenuButton;
    public Button ScaleButton;
    public Button RotateButton;



    public GameObject MenuParent;
    public void SetTileLocalMenuOptionsActivation(bool isActive)
    {
        if (MenuParent != null)
        {
            MenuParent.SetActive(isActive);
        }
    }

    bool isAllowShowEditorButtons = true;
    public void SetEditorButtonsActivation(bool isActive)
    {
        if (!isAllowShowEditorButtons)
        {
            return;
        }


        if (MenuButton != null)
        {
            MenuButton.gameObject.SetActive(isActive);
        }

        if (ScaleButton != null)
        {
            ScaleButton.gameObject.SetActive(isActive);
        }

        if (RotateButton != null)
        {
            RotateButton.gameObject.SetActive(isActive);
        }
    }


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

            IsShowHealthBar = loadTile.isHealthBarShown;
            MaxHealth = loadTile.MaxHealth;
            CurrentHealth = loadTile.Health;

            OrderInLayer = loadTile.orderInLayer;

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

        IsShowHealthBar = false;
        MaxHealth = 100;
        CurrentHealth = 100;

        setShowName(false);
    }

    public void ActivateEditing(bool isActivate)
    {
        if (isActivate)
        {
            StartCoroutine(Editing());
        }
        else
        {
            IsRunningEditing = false;
        }
    }

    private bool IsRunningEditing = false;
    public IEnumerator Editing()
    {
        if (IsRunningEditing)
        {
            yield break;
        }

        IsRunningEditing = true;

        setShowName(true);

        SetEditorButtonsActivation(true);


        while (IsRunningEditing)
        {

            yield return null;

            if (Input.GetButtonDown(StaticVariables.MoveButtonName))
            {
                EndShowAllHelpText();
                MoveTile();
            } else if (Input.GetButtonDown(StaticVariables.ScaleButtonName))
            {
                EndShowAllHelpText();
                ScaleTile();
            } else if(Input.GetButtonDown(StaticVariables.RotateButtonName))
            {
                EndShowAllHelpText();
                RotateTile();
            }

            if (IsRunning)
            {
                SetEditorButtonsActivation(false);
                yield return new WaitWhile(() => IsRunning);
                SetEditorButtonsActivation(true);
            }

        }

        setShowName(false);

        SetEditorButtonsActivation(false);

        IsRunningEditing = false;
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

        while (Input.GetMouseButton(0) || Input.GetButton(StaticVariables.MoveButtonName))
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

        ShowHelpText(5);
        ShowHelpText(6);

        if (Camera.main != null)
        {

            Vector2 initialMousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            Vector2 initialSize = new Vector2(Size.x, Size.y);

            while (IsRunning)
            {
                if (Input.GetButtonDown("Cancel"))
                {
                    IsRunning = false;
                }

                Vector2 currentMousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);

                //setting Size scales tile
                if (Input.GetButton(StaticVariables.IncrementButtonName) && sprite != null)
                {
                    //Debug.Log("Scale, Keep Aspect");

                    Size = new Vector2(
                    initialSize.x + (currentMousePos.x - initialMousePos.x) * Camera.main.aspect,
                    initialSize.x + (currentMousePos.x - initialMousePos.x) * Camera.main.aspect * (sprite.rect.height / sprite.rect.width)
                    );
                } else
                {
                    //Debug.Log("Scale, Free Aspect");
                    Size = new Vector2(
                    initialSize.x + (currentMousePos.x - initialMousePos.x) * Camera.main.aspect,
                    initialSize.y - (currentMousePos.y - initialMousePos.y) * 2
                    );
                }

                

                yield return null;
            }
        } else
        {
            Debug.LogWarning("No main camera in scene. Scale tile function aborted");
        }

        EndShowHelpText(5);
        EndShowHelpText(6);


        IsRunning = false;
    }

    public void RotateTile()
    {
        StartCoroutine(RotateImage());
    }

    private int incrementAngle = 15;
    private IEnumerator RotateImage()
    {
        if (IsRunning)
        {
            yield break;
        }

        IsRunning = true;

        ShowHelpText(2);
        ShowHelpText(3);


        if (Camera.main != null)
        {

            int startAngle = Angle;
            Vector3 mousePos = Input.mousePosition;
            while (IsRunning)
            {
                if (Input.GetButtonDown("Cancel"))
                {
                    IsRunning = false;
                }

                Debug.Log(Input.mousePosition);

                int currentAngle = startAngle - (int)Vector2.SignedAngle(
                    Input.mousePosition - Camera.main.WorldToScreenPoint(gameObject.transform.position),
                    mousePos - Camera.main.WorldToScreenPoint(gameObject.transform.position)
                    );

                if (Input.GetButton(StaticVariables.IncrementButtonName))
                {
                    currentAngle = Mathf.RoundToInt(currentAngle / incrementAngle) * incrementAngle;
                }

                //setting Angle Rotates tile
                Angle = currentAngle;

                yield return null;
            }
        } else
        {
            Debug.LogWarning("No main camera in scene Rotate tile function aborted");
        }

        EndShowHelpText(2);
        EndShowHelpText(3);


        IsRunning = false;
    }
    

    bool isRunningEditorStartListener = false;
    private IEnumerator EditorStartListener()
    {
        if (isRunningEditorStartListener)
        {
            yield break;
        }

        isRunningEditorStartListener = true;

        while (isRunningEditorStartListener)
        {
            

            if (Input.GetButtonDown("Accept"))
            {
                isRunningEditorStartListener = false;
                StartCoroutine(Editing());
            }

            yield return null;
        }

        isRunningEditorStartListener = false;
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

    //health bar---

    bool _isShowSlider = true;
    public bool IsShowHealthBar
    {
        get { return _isShowSlider; }
        set
        {
            _isShowSlider = value;
            if (HealthBarSlider != null)
            {
                HealthBarSlider.gameObject.SetActive(value);
            }
        }
    }
    public void ToggleShowHealthbar()
    {
        IsShowHealthBar = !IsShowHealthBar;
    }

    public Slider HealthBarSlider;

    public void OnSliderValueChanged()
    {
        if (HealthBarSlider != null)
        {
            CurrentHealth = (int)HealthBarSlider.value;
        }
    }

    bool InputFieldAssignedCheck(TMP_InputField currentInputField)
    {
        if (currentInputField != null && currentInputField.text != null)
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    void AssignInputFieldValue(TMP_InputField currentInputField, int currentValue)
    {
        if (InputFieldAssignedCheck(currentInputField))
        {
            currentInputField.text = currentValue.ToString();
        }
    }

    private int _maxHealth = 100;
    public int MaxHealth
    {
        get { return _maxHealth; }
        set
        {
            _maxHealth = value;
            if (value < CurrentHealth)
            {
                CurrentHealth = value;

                AssignInputFieldValue(currentHealthInputField, value);

            }

            if (HealthBarSlider != null)
            {
                HealthBarSlider.maxValue = value;
            }

            AssignInputFieldValue(maxHealthInputField, value);
            SetHealthBarSliderColor();
        }
    }
    public TMP_InputField maxHealthInputField;
    public void OnMaxHealthInputFieldEndEdit()
    {
        if (InputFieldAssignedCheck(maxHealthInputField))
        {
            try
            {
                MaxHealth = int.Parse(maxHealthInputField.text);
            }
            catch
            {
                Debug.Log("Could not change inputField. Only integer values accepted");
            }
        }
    }

    private int _currentHealth = 100;
    public int CurrentHealth
    {
        get { return _currentHealth; }
        set
        {
            _currentHealth = value;
            if (HealthBarSlider != null)
            {
                HealthBarSlider.value = Mathf.Clamp(value, 0, MaxHealth);
            }

            AssignInputFieldValue(currentHealthInputField, value);
            SetHealthBarSliderColor();
        }
    }
    //public InputField currentHealthInputField;
    public TMP_InputField currentHealthInputField;

    public void OncurrentHealthInputFieldEndEdit()
    {
        if (InputFieldAssignedCheck(currentHealthInputField))
        {
            try
            {
                CurrentHealth = int.Parse(currentHealthInputField.text);
            }
            catch
            {
                Debug.Log("Could not change inputField. Only integer values accepted");
            }


        }
    }

    public Image HeathBarSliderFillImage;
    public Gradient HealthBarColorGradient;
    private void SetHealthBarSliderColor()
    {
        if (HeathBarSliderFillImage != null)
        {
            HeathBarSliderFillImage.color = HealthBarColorGradient.Evaluate(Mathf.Clamp(CurrentHealth / Mathf.Clamp(MaxHealth, 0.001f, MaxHealth), 0, 1));
        }
    }

    //Healthbar end

    public GameObject MenuObj;

    private bool _isShowMenu = false;
    public bool isShowMenu
    {
        get { return _isShowMenu; }
        set
        {
            _isShowMenu = value;
            if (MenuObj != null)
            {
                isAllowShowEditorButtons = !value;

                MenuObj.SetActive(_isShowMenu);

                SetEditorButtonsActivation(!value);
            }

        }
    }

    public void SetShowMenu(bool isShow)
    {
        isShowMenu = isShow;
    }

    //Frame
    public GameObject FrameObj;

    private bool _isShowFrame = false;
    public bool isShowFrame { get { return _isShowFrame; } set
        {
            _isShowFrame = value;
            if (FrameObj != null)
            {
                FrameObj.SetActive(_isShowFrame);
            }

        } 
    }

    public void ToggleShowFrame()
    {
        isShowFrame = !isShowFrame;
    }




    //Order of tiles (needs rework)
    public int OrderInLayer
    {
        get { return transform.GetSiblingIndex(); }
        set
        {
            transform.SetSiblingIndex(value);

            if (currentLoadTile != null)
            {
                currentLoadTile.orderInLayer = value;
            }

        }
    }

    public void PushToFront()
    {
        transform.SetAsLastSibling();
        OrderInLayer = transform.GetSiblingIndex();
    }

    //Help Text
    public HelpTextArray currentHelpTextArray;


    public void ShowHelpText(int index)
    {
        if (currentHelpTextArray != null)
        {
            currentHelpTextArray.ShowHelp(index);
        }
    }

    public void EndShowHelpText(int index)
    {
        if (currentHelpTextArray != null)
        {
            currentHelpTextArray.EndShowHelp(index);
        }
    }

    public void EndShowAllHelpText()
    {
        if (currentHelpTextArray != null)
        {
            currentHelpTextArray.EndShowAllHelp();
        }
    }

}
