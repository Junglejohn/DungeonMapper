using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class tileCanvasScript : MonoBehaviour {

    public TileScript tilePrefab;

    public Transform ForegroundTileParent;
    public List<TileScript> ForegroundTileList = new List<TileScript>();

    public Transform BackgroundTileParent;
    public List<TileScript> BackgroundTileList = new List<TileScript>();

    bool IsReferencesAssigned()
    {
        if (tilePrefab != null && ForegroundTileParent != null && BackgroundTileParent != null)
        {
            return true;
        } else
        {

            Debug.LogError("Null references for tileCanvasScript. Assign to create tiles");
            return false;
        }
    }
    
    private void OnEnable()
    {
        EventManager.OnSessionRefresh += RefreshTiles;
        EventManager.OnCreateLoadTileForLoadSprite += CreateTile;
        EventManager.OnIsForegroundActivation += SetForegroundBackgroundTilesActivation;
    }

    private void OnDisable()
    {
        EventManager.OnSessionRefresh -= RefreshTiles;
        EventManager.OnCreateLoadTileForLoadSprite -= CreateTile;
        EventManager.OnIsForegroundActivation -= SetForegroundBackgroundTilesActivation;
    }
    
    void CreateTile(LoadSprite currentLoadSprite, bool IsForeground)
    {
        if (!IsReferencesAssigned()) { return; }

        Debug.LogError("LoadSprite is being created");

        if (currentLoadSprite != null)
        {
            
            LoadTile loadTile = new LoadTile();
            loadTile.loadSpriteInfo = currentLoadSprite;
            currentLoadSprite.AddLoadTile(loadTile);

            TileScript tile = CreateTileObject(loadTile, IsForeground);

            if (Camera.main != null)
            {
                tile.Position = Camera.main.ScreenToWorldPoint(new Vector2(Screen.width / 2, Screen.height / 2));
            } else
            {
                tile.Position = new Vector2(Screen.width / 2, Screen.height / 2);
            }
            
            Debug.LogError("LoadSprite was created");

        }
        else
        {
            Debug.LogError("no LoadSprite is assigned - This tile was not created");
        }
    }

    void RefreshTiles(Session currentSession)
    {
        if (!IsReferencesAssigned() || currentSession == null) { return; }
        
        foreach (TileScript tile in ForegroundTileList)
        {
            Destroy(tile.gameObject);
        }
        ForegroundTileList = new List<TileScript>();

        foreach (LoadSprite sprite in currentSession.LoadSpriteList)
        {
            foreach (LoadTile loadTile in sprite.LoadTileList)
            {
                
                CreateTileObject(loadTile, true);

            }
        }



        foreach (TileScript tile in BackgroundTileList)
        {
            Destroy(tile.gameObject);
        }
        BackgroundTileList = new List<TileScript>();
        
        foreach (LoadSprite sprite in currentSession.BackgroundLoadSpriteList)
        {
            foreach (LoadTile loadTile in sprite.LoadTileList)
            {
                CreateTileObject(loadTile, false);
            }
        }

        SetForegroundBackgroundTilesActivation(EventManager.IsForegroundActive);
    }

    TileScript CreateTileObject(LoadTile currentLoadTile, bool IsForeground)
    {
        Transform parent;

        if (IsForeground)
        {
            parent = ForegroundTileParent;
        } else
        {
            parent = BackgroundTileParent;
        }


        GameObject tileObject = Instantiate(tilePrefab.gameObject, parent, true) as GameObject;
        TileScript tile = tileObject.GetComponent<TileScript>();
        tile.InitializeTile(currentLoadTile);

        if (IsForeground)
        {
            ForegroundTileList.Add(tile);
        } else
        {
            BackgroundTileList.Add(tile);
        }

        return tile;
    }

    public void SetForegroundBackgroundTilesActivation(bool IsForegroundActive)
    {
        Debug.Log("Changing Button interaction states");

        foreach (TileScript tile in ForegroundTileList)
        {
            if (IsForegroundActive)
            {
                tile.SetTileInteractionState(TileScript.TileInteractionState.Active);
            } else
            {
                tile.SetTileInteractionState(TileScript.TileInteractionState.SemiTransparentNoInteraction);
            }
        }

        foreach (TileScript tile in BackgroundTileList)
        {
            if (IsForegroundActive)
            {
                tile.SetTileInteractionState(TileScript.TileInteractionState.NoInteraction);
            }
            else
            {
                tile.SetTileInteractionState(TileScript.TileInteractionState.Active);
            }
        }

    }
}
