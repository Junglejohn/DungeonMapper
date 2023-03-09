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

        /*
        if (StaticObjectReferences.currentReferences.TilePrefab != null)
        {
            
            if (currentloadSprite != null )
            {
                LoadTile loadTile = new LoadTile();
                loadTile.loadSpriteInfo = currentloadSprite;
                currentloadSprite.AddLoadTile(loadTile);

                Debug.Log("Creating Tile");
                GameObject tile = Instantiate(StaticObjectReferences.currentReferences.TilePrefab.gameObject, StaticObjectReferences.currentReferences.TileParent, false) as GameObject;
                
             
                TileScript tileScript = tile.gameObject.GetComponent<TileScript>();
                
                tileScript.InitializeTile(loadTile);
                tileScript.Position = new Vector2(Screen.width/2, Screen.height/2);
                
            } else
            {
                Debug.LogError("Creating Tile, but no LoadSprite is assigned - This tile will not be saved");
                GameObject tile = Instantiate(StaticObjectReferences.currentReferences.TilePrefab.gameObject, StaticObjectReferences.currentReferences.TileParent, false) as GameObject;
                tile.gameObject.GetComponent<TileScript>().InitializeTile(Name, image.sprite);
            }
        }

        */
    }
    
    public void RefreshTiles()
    {
        if (currentloadSprite != null)
        {
            foreach (LoadTile tile in currentloadSprite.LoadTileList)
            {
                if (tile.OnTileObjectsDeleted != null)
                {
                    tile.OnTileObjectsDeleted.Invoke();
                }
            }

            foreach (LoadTile loadTile in currentloadSprite.LoadTileList)
            {
                Debug.Log("Creating Tile");
                GameObject tile = Instantiate(StaticObjectReferences.currentReferences.TilePrefab.gameObject, StaticObjectReferences.currentReferences.TileParent, false) as GameObject;
                TileScript tileScript = tile.gameObject.GetComponent<TileScript>();

                //tileScript.currentLoadTile.loadSpriteInfo
                tileScript.InitializeTile(loadTile);
                
            }

        } else
        {
            Debug.LogError("No loadsprite assigned. Cannot refresh tiles");
        }

    }

    public void DeleteTileCreator()
    {

        currentloadSprite.IsShown = false;
        Destroy(gameObject);
    }
	
}
