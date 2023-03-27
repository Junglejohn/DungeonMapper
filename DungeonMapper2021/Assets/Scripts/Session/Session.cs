using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

[System.Serializable]
public class Session {

    public enum SessionUseState { empty, tilesAdded, inUse}

    public SessionUseState InUseCheck()
    {
        if (LoadSpriteList.Count == 0 && BackgroundLoadSpriteList.Count == 0)
        {
            return SessionUseState.empty;
        }


        foreach (LoadSprite sprite in LoadSpriteList )
        {
            if (sprite.LoadTileList.Count > 0)
            {
                return SessionUseState.inUse;
            }
        }

        foreach (LoadSprite sprite in BackgroundLoadSpriteList)
        {
            if (sprite.LoadTileList.Count > 0)
            {
                return SessionUseState.inUse;
            }
        }


        return SessionUseState.tilesAdded;
    }

    public delegate void SessionEvent();
    public SessionEvent OnSaved;
    public SessionEvent OnLoaded;
    
    public string Name = "Session01";

    private const string SaveFileTypeName = ".troll";

    public string SessionFolderDataPath
    {
        get
        {
            return Application.persistentDataPath + "/SessionData";
        }
    }

    public string DataPath { get {
            return SessionFolderDataPath + "/" + Name;
        }
    }

    public string SessionFilePath
    {
        get
        {
            return DataPath + "/" + Name + SaveFileTypeName;
        }
    }

    public Vector2 SessionDimensions = new Vector2(2500,2000);

    public LoadSprite backgroundImg;
    
    public List<LoadSprite> BackgroundLoadSpriteList = new List<LoadSprite>();
    
    public List<LoadSprite> LoadSpriteList = new List<LoadSprite>();

    public FogOfWar fogOfWar;

    public Session(string SessionName)
    {
        Name = SessionName;
    }

    // loadSprites
    public bool AddLoadSprite(string Name, Sprite sprite, bool IsForeground)
    {
        if (!string.IsNullOrEmpty(Name) && sprite != null)
        {
            LoadSprite loadSprite = new LoadSprite();
            loadSprite.Name = Name;

            loadSprite.size = new Vector2(sprite.rect.width, sprite.rect.height);

            byte[] byteArray = sprite.texture.EncodeToPNG();
            Texture2D tex = new Texture2D(1, 1);
            tex.LoadImage(byteArray);
            loadSprite.sprite = Sprite.Create(tex, sprite.rect, new Vector2(0, 0));


            


            if (IsForeground)
            {
                LoadSpriteList.Add(loadSprite);
            } else
            {
                BackgroundLoadSpriteList.Add(loadSprite);
            }
            

            SaveSession();

            if (EventManager.OnSessionRefresh != null)
            {
                EventManager.OnSessionRefresh(this);
            }

            return true;
        }
        else
        {
            return false;
        }


       
    }

    public bool DeleteLoadSprite(LoadSprite currentLoadSprite, bool IsDeleteAllTilesToo)
    {
        if (currentLoadSprite != null && LoadSpriteList.Contains(currentLoadSprite))
        {
            if (IsDeleteAllTilesToo) // Just delete both loadsprite and tiles
            {
                currentLoadSprite.DeleteAllTiles();
                LoadSpriteList.Remove(currentLoadSprite);
            } else //only flag to hide Loadsprite so it does not show up unless it is no longer in use
            {
                if (currentLoadSprite.InUse() == false)
                {
                    LoadSpriteList.Remove(currentLoadSprite);
                }
                else
                {
                    currentLoadSprite.IsShown = false;
                }
            }

            SaveSession(); // save session actually deletes images that are not used by any loadsprites so they are no loanger saved

            return true;

        } else
        {

            Debug.LogError("Loadsprite was not deleted. Either it was null or it was not present in this session");
            return false;
        }


        
    }

    public bool DeleteBackgroundLoadSprite(LoadSprite currentLoadSprite)
    {
        if (currentLoadSprite != null && BackgroundLoadSpriteList.Contains(currentLoadSprite))
        {

            currentLoadSprite.DeleteAllTiles();

            BackgroundLoadSpriteList.Remove(currentLoadSprite);

            SaveSession();

            return true;

        }
        else
        {

            Debug.LogError("Loadsprite was not deleted. Either it was null or it was not present in this session");
            return false;
        }



    }

    public void SaveSession()
    {
        
        try
        {
            if (!Directory.Exists(DataPath))
            {
                Directory.CreateDirectory(DataPath);
            }
                

            SessionData data = new SessionData();
            data.PopulateSessionData(this);

            BinaryFormatter bf = new BinaryFormatter();
            FileStream stream = new FileStream(SessionFilePath, FileMode.Create);

            bf.Serialize(stream, data);
            stream.Close();
        } catch (UnassignedReferenceException e)
        {
            Debug.LogError(e);
        }

        if (OnSaved != null)
        {
            OnSaved.Invoke();
        }

        DebugSession.DebugSessionInfo(this, "Session Saved");

        Debug.LogError("Session named " + Name + " was saved");
        
    }

    public bool LoadSession()
    {
        string path = SessionFilePath;
        if (File.Exists(path))
        {
            LoadSpriteList = new List<LoadSprite>(); //reset loadSprites to prevent double load
            BackgroundLoadSpriteList = new List<LoadSprite>(); //reset BackgroundloadSprites to prevent double load

            //load data for session
            BinaryFormatter bf = new BinaryFormatter();
            FileStream stream = new FileStream(path, FileMode.Open);

            SessionData data = bf.Deserialize(stream) as SessionData;

            fogOfWar = data.fogOfWarData.ConvertToFogOfWar();

            data.PopulateSessionWithData(this);

            DebugSession.DebugSessionInfo(this, "Session Loaded");

            stream.Close();
            
            //tell everyone that listens that a load happened and to update. Subsbribe to this in eventmanager(static).
            if (OnLoaded != null)
            {
                OnLoaded.Invoke();
            }

            Debug.Log("Session named: " + Name + " is finished loading at path: " + path);
            return true;
        }
        else
        {
            Debug.LogError("No file found at: " + path);
            return false;
            
        }

        

        
        

    }

    public void DeleteSession()
    {
        DirectoryInfo di = new DirectoryInfo(DataPath);
        try
        {
            if (di.Exists)
            {

                di.Delete(true);

            } else
            {
                Debug.LogError("Session called: " + Name + ", did not exist at: " + DataPath);
            }
        }
        catch (IOException e)
        {
            Debug.LogError("An error occurred when deleting the session, with message: " + e);
        }
    }


    //for testing
    public void Save(Texture2D texture)
    {


        System.IO.File.WriteAllBytes(Application.persistentDataPath + "/Session", texture.EncodeToPNG());
        Debug.Log("Saved file to: " + Application.persistentDataPath + "/Session");


    }

    public Texture2D Load()
    {
       
        byte[] bytes = System.IO.File.ReadAllBytes(Application.persistentDataPath + "/Session");
        Debug.Log("Loaded file from: " + Application.persistentDataPath + "/Session");

        Texture2D tex = new Texture2D(1, 1);
        tex.LoadImage(bytes);
        return tex;

    }
    
}

[System.Serializable]
public class FogOfWar{

    public bool IsShown = true;
    
    public Vector2 size;
    public Sprite sprite;
}

[System.Serializable]
public class LoadSprite
{
    public bool IsShown = true; //should a tileCreatorTile be shown for this Loadsprite?

    public string Name;
    public string storageName;
    public Sprite sprite;
    public Vector2 pos;
    public Vector2 size;

    [SerializeField]
    public List<LoadTile> LoadTileList = new List<LoadTile>();

    public void AddLoadTile(LoadTile loadTile)
    {
        if (loadTile != null)
        {
            loadTile.loadSpriteInfo = this;
            LoadTileList.Add(loadTile);

        }
    }

    public void DeleteAllTiles()
    {
        foreach (LoadTile tile in LoadTileList)
        {
            if (tile.OnDeleted != null)
            {
                tile.OnDeleted.Invoke();
            }

        }

        LoadTileList = new List<LoadTile>();
    }

    public void DeleteTile(LoadTile currentLoadTile)
    {
        if (LoadTileList.Contains(currentLoadTile))
        {
            currentLoadTile.OnTileObjectsDeleted.Invoke();
            LoadTileList.Remove(currentLoadTile);
        } else
        {
            Debug.LogError("Could not delete loadTile from loadSprite named : "+ Name+" - It was not found in loadTileList");
        }
    }

    public bool InUse()
    {
        if (IsShown != false || LoadTileList.Count > 0)
        {
            return true;
        } else
        {
            return false;
        }
    }
}

[System.Serializable]
public class LoadTile
{

    public delegate void LoadTileEvent();
    public LoadTileEvent OnDeleted;
    public LoadTileEvent OnTileObjectsDeleted;

    public LoadSprite loadSpriteInfo;
    
    public Vector2 pos;

    private Vector2 _size;
    public Vector2 size { get {
            return _size;
        }
        set {
            _size = new Vector2(Mathf.Clamp(value.x, 40, 5000), Mathf.Clamp(value.y, 40, 5000));

        }

    }

    public int Angle;

    public bool isHealthBarShown;
    public int MaxHealth;
    public int Health;


}
