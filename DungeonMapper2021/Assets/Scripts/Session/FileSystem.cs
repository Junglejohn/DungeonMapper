using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

public static class FileSystem {

    public static List<Session> GetAllSessions()
    {
        List<Session> sessionList = new List<Session>();

        //Session tempSession = new Session("none");

        string path = Session.SessionFolderDataPath;

        DirectoryInfo di = new DirectoryInfo(path);
        try
        {
            if (di.Exists)
            {
                DirectoryInfo[] directories = di.GetDirectories();

                foreach (DirectoryInfo directoryInfo in directories)
                {
                    FileInfo[] files = directoryInfo.GetFiles();

                    foreach (FileInfo file in files)
                    {

                        if (File.Exists(file.FullName))
                        {
                            BinaryFormatter bf = new BinaryFormatter();
                            FileStream stream = new FileStream(file.FullName, FileMode.Open);

                            try {
                                SessionData data = bf.Deserialize(stream) as SessionData;
                                Session session = data.ConvertToSession();
                                sessionList.Add(session);
                            } catch
                            {
                                Debug.Log("File named: " + file.Name + ". located at: " + file.FullName + ". Is not a session object");
                            }
                            
                            stream.Close();
                            
                        }
                        else
                        {
                            Debug.LogError("No file found at: " + file.FullName);
                        }


                    }

                }
            }
            else
            {
                di.Create();
            }
        } catch
        {
            Debug.LogError("Could not fetch sessions from path: " + path);
        }

        return sessionList;
    }
    
    public static List<string> GetAllSessionNames()
    {
        List<string> sessionNameList = new List<string>();

        string path = Session.SessionFolderDataPath;

        DirectoryInfo di = new DirectoryInfo(path);
        try
        {
            if (di.Exists)
            {
                DirectoryInfo[] directories = di.GetDirectories();

                foreach (DirectoryInfo directoryInfo in directories)
                {
                    sessionNameList.Add(directoryInfo.Name);
                }
            }
        }
        catch
        {
            Debug.LogError("Could not fetch session names from path: " + path);
        }

        return sessionNameList;
    }

    public static Session GetSessionByName(string Name)
    {

        Session Session = new Session(Name);

        Session.LoadSession();

        return Session;
    }

    public static void SaveSession(Session currentSession)
    {
        BinaryFormatter bf = new BinaryFormatter();
        string path = Application.persistentDataPath + "/Session.troll";
        FileStream stream = new FileStream(path, FileMode.Create);

        SessionData data = new SessionData();
        data.PopulateSessionData(currentSession);


        bf.Serialize(stream, data);
        stream.Close();

    }

    public static Session LoadSession(string sessionName)
    {
        string path = Application.persistentDataPath + "/session.troll";
        if (File.Exists (path))
        {
            BinaryFormatter bf = new BinaryFormatter();
            FileStream stream = new FileStream(path, FileMode.Open);

            SessionData data = bf.Deserialize(stream) as SessionData;

            Session session = data.ConvertToSession();

            stream.Close();

            return session;
        } else
        {
            Debug.LogError("No file found at: " + path);
            return null;
        }
    }

}

[System.Serializable]
public class SessionData
{
    public string Name;

    public float DimensionX;
    public float DimensionY;

    public List<LoadSpriteData> backgroundDataList = new List<LoadSpriteData>();
    public List<LoadSpriteData> LoadSpriteDataList = new List<LoadSpriteData>();
    
    public FogOfWarData fogOfWarData;

    public void PopulateSessionData(Session session)
    {
        Name = session.Name;

        DimensionX = session.AreaSize.x;
        DimensionY = session.AreaSize.y;

        Debug.Log("PopulateSessionData - name of session [" + session.Name + "] Name of data [" + Name + "]. Session Area [" + session.AreaSize + "] DimensionXY [" + DimensionX + "," + DimensionY + "]");

        fogOfWarData = new FogOfWarData(session.fogOfWar);

        foreach (LoadSprite loadSprite in session.BackgroundLoadSpriteList)
        {

            LoadSpriteData backgroundloadSpriteData = new LoadSpriteData(loadSprite);
            backgroundDataList.Add(backgroundloadSpriteData);
        }

        foreach (LoadSprite loadSprite in session.LoadSpriteList)
        {

            LoadSpriteData loadSpriteData = new LoadSpriteData(loadSprite);
            LoadSpriteDataList.Add(loadSpriteData);

        }

    }

    public Session ConvertToSession()
    {
        Session currentSession = new Session(Name);

        currentSession.AreaSize = new Vector2(DimensionX, DimensionY);

        currentSession.fogOfWar = fogOfWarData.ConvertToFogOfWar();

        if (backgroundDataList != null)
        {

            foreach (LoadSpriteData loadSpriteData in backgroundDataList)
            {
                LoadSprite loadSprite = loadSpriteData.ConvertToLoadSprite();
                currentSession.BackgroundLoadSpriteList.Add(loadSprite);
                
            }
        } else
        {
            Debug.LogError("BackgroundImage List was null. No Images was loaded");
        }

        if (LoadSpriteDataList != null)
        {
            foreach (LoadSpriteData loadSpriteData in LoadSpriteDataList)
            {
                LoadSprite loadSprite = loadSpriteData.ConvertToLoadSprite();
                currentSession.LoadSpriteList.Add(loadSprite);
            }
        } else
        {
            Debug.LogError("Image List was null. No Images was loaded");
        }


        return currentSession;
    }
    
    public void PopulateSessionWithData(Session currentSession)
    {

        currentSession.AreaSize = new Vector2(DimensionX, DimensionY);

        if (backgroundDataList != null)
        {
            foreach (LoadSpriteData loadSpriteData in backgroundDataList)
            {
                LoadSprite loadSprite = loadSpriteData.ConvertToLoadSprite();
                currentSession.BackgroundLoadSpriteList.Add(loadSprite);

            }
        }

        if (LoadSpriteDataList != null)
        {
            foreach (LoadSpriteData loadSpriteData in LoadSpriteDataList)
            {
                LoadSprite loadSprite = loadSpriteData.ConvertToLoadSprite();
                currentSession.LoadSpriteList.Add(loadSprite);
            }

        }
        
    }
}

[System.Serializable]
public class FogOfWarData
{
    public bool IsShown;

    public int SizeX;
    public int SizeY;

    public byte[] image;

    public FogOfWarData(FogOfWar fogOfWar)
    {
        if (fogOfWar != null && fogOfWar.sprite != null)
        {
            IsShown = fogOfWar.IsShown;

            SizeX = (int)fogOfWar.sprite.rect.width;
            SizeY = (int)fogOfWar.sprite.rect.height;

            image = fogOfWar.sprite.texture.EncodeToPNG();

        }
    }

    public FogOfWar ConvertToFogOfWar()
    {
        FogOfWar fog = new FogOfWar();

        if (image != null)
        {
            Texture2D tex = new Texture2D(1, 1);
            tex.LoadImage(image);
            fog.sprite = Sprite.Create(tex, new Rect(0, 0, SizeX, SizeY), new Vector2(0, 0));
        }


        return fog;
    }

}

[System.Serializable]
public class LoadSpriteData
{
    public bool IsShown;

    string spritePath;
    string Name;
    public List<LoadTileData> LoadTileDataReferenceList = new List<LoadTileData>();

    public byte[] image;
        
    float SizeX;
    float SizeY;

    public LoadSpriteData(LoadSprite loadSprite)
    {
        if(loadSprite != null && loadSprite.sprite != null)
        {
            IsShown = loadSprite.IsShown;

            spritePath = loadSprite.storageName;
            Name = loadSprite.Name;
            
            image = loadSprite.sprite.texture.EncodeToPNG();

            SizeX = loadSprite.size.x;
            SizeY = loadSprite.size.y;

            if (loadSprite.LoadTileList != null)
            {
                foreach (LoadTile tile in loadSprite.LoadTileList)
                {
                    LoadTileData tileData = new LoadTileData(tile);

                    LoadTileDataReferenceList.Add(tileData);

                    Debug.Log("loadTileData was added to loadSpriteData by the name: " + Name);
                }
            } else { Debug.Log("loadSpriteData by the name: " + Name + " could not add any tileData. The tileDataList was null"); }

        } else { Debug.Log("The loadSprite that was supplied when creating this loadSpriteData was null"); }

    }

    public LoadSprite ConvertToLoadSprite()
    {
        LoadSprite loadSprite = new LoadSprite();

        loadSprite.IsShown = IsShown;

        loadSprite.Name = Name;
        loadSprite.storageName = spritePath;
        
        loadSprite.size.x = SizeX;
        loadSprite.size.y = SizeY;

        if (image != null)
        {
            Texture2D tex = new Texture2D(1,1);
            tex.LoadImage(image);
            loadSprite.sprite = Sprite.Create(tex, new Rect(0, 0 ,SizeX,SizeY), new Vector2(0,0));
        }

        if (LoadTileDataReferenceList != null)
        {
            foreach (LoadTileData tileData in LoadTileDataReferenceList)
            {
                if (tileData != null)
                {
                    LoadTile tile = tileData.ConvertToLoadTile();
                    loadSprite.AddLoadTile(tile);
                    Debug.Log("LoadTile was added to LoadSprite by the name: " + Name);


                } else { Debug.Log("LoadTile was null and was not added to LoadSprite by the name: " + Name); }
            }
        } else { Debug.Log("LoadTileDataReferenceList for Loadsprite called: " + Name + " was null and no tiles was added"); }

        return loadSprite;

    }

}

[System.Serializable]
public class LoadTileData
{
    float positionX;
    float positionY;

    float SizeX;
    float SizeY;

    int Angle;

    bool isHealthBarShown;
    public int currentHealth;
    public int maxHealth;

    public LoadTileData(LoadTile loadTile)
    {
        if (loadTile != null)
        {
            positionX = loadTile.pos.x;
            positionY = loadTile.pos.y;

            SizeX = loadTile.size.x;
            SizeY = loadTile.size.y;

            Angle = loadTile.Angle;

            isHealthBarShown = loadTile.isHealthBarShown;
            currentHealth = loadTile.Health;
            maxHealth = loadTile.MaxHealth;

        } else
        {
            Debug.LogError("supplied loadTile was null");
        }


    }

    public LoadTile ConvertToLoadTile()
    {
        LoadTile loadTile = new LoadTile();

        loadTile.pos.x = positionX;
        loadTile.pos.y = positionY;

        loadTile.size = new Vector2(SizeX,SizeY);

        loadTile.Angle = Angle;

        loadTile.isHealthBarShown = isHealthBarShown;
        loadTile.Health = currentHealth;
        loadTile.MaxHealth = maxHealth;

        if (loadTile.Health != 0 || loadTile.MaxHealth != 0)
        {
            Debug.Log("Loading data from LoadTileData - isShowHealth " + loadTile.isHealthBarShown + ", CurrentHealth " + loadTile.Health + ", maxHealth " + loadTile.MaxHealth);
        }

        return loadTile;

    }

}


