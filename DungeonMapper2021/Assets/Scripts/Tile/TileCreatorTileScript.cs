using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TileCreatorTileScript : MonoBehaviour {

    public LoadSprite currentloadSprite;

    public Button button;

    [SerializeField]
    private string _name = "default";
    public string Name
    {
        get { return _name; }
        set
        {
            _name = value;
            if (NameText != null)
            {
                NameText.text = _name;
            }
        }
    }
    [SerializeField]
    Text NameText;

    
    public Vector2 Size;
    
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

    private void Start()
    {
        Name = _name;
    }

    public void InitializeTile(LoadSprite loadSprite)
    {
        if (loadSprite != null)
        {
            if (loadSprite.IsShown == false)
            {
                DeleteTileCreator();
            }

            Name = loadSprite.Name;
            sprite = loadSprite.sprite;
        }


        currentloadSprite = loadSprite;

    }

    public void CreateTile()
    {
        if (EventManager.OnCreateLoadTileForLoadSprite != null)
        {
            EventManager.OnCreateLoadTileForLoadSprite.Invoke(currentloadSprite, EventManager.IsForegroundActive);

            Debug.Log("creating tile for the loadsprite called " + Name);
        }
    }

    public void DeleteTileCreator()
    {

        currentloadSprite.IsShown = false;
        Destroy(gameObject);
    }
	
}
