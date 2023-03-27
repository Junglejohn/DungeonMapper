using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;


public class TileMenuCanvasScript : MonoBehaviour {

	public void Exit(bool IsSaveOnExit)
    {
        if (IsSaveOnExit)
        {
            Manager.currentManager.session.SaveSession();
        }


        SceneManager.LoadScene(GameManager.MenuSceneName);


    }

    public void OnEnable()
    {
        EventManager.OnSessionRefresh += RefreshTileCreatorButtons;
        
        //EventManager.OnIsForegroundActivation += SetRefreshTileCreatorButtonsWithCurrentSessionData;

    }

    public void OnDisable()
    {
        EventManager.OnSessionRefresh -= RefreshTileCreatorButtons;
        //EventManager.on
        //EventManager.OnIsForegroundActivation -= SetRefreshTileCreatorButtonsWithCurrentSessionData;
    }
    
    public void ToggleWindowedMode()
    {
        Screen.fullScreen = !Screen.fullScreen;
    }
    
    public TileCreatorTileScript tileCreatorPrefab;
    public Transform TileCreatorParent;
    public List<TileCreatorTileScript> tileCreatorButtons = new List<TileCreatorTileScript>();
    public void RefreshTileCreatorButtons(Session currentSession)
    {

        ResetTileCreatorButtons();

        if (currentSession != null)
        {
            if (tileCreatorPrefab != null && tileCreatorPrefab != null)
            {

                //check whether to use foregrund or background list
                List<LoadSprite> tempLoadSpriteList = new List<LoadSprite>();
                if (EventManager.IsForegroundActive)
                {
                    Debug.Log("tilecreatorTiles foregroundIsActive");


                    tempLoadSpriteList = currentSession.LoadSpriteList;
                }
                else
                {

                    Debug.Log("tilecreatorTiles backgroundIsActive");
                    tempLoadSpriteList = currentSession.BackgroundLoadSpriteList;
                }

                //spawn tiles
                foreach (LoadSprite sprite in tempLoadSpriteList)
                {

                    Debug.Log("creating sprite by the name of " + sprite.Name);

                    GameObject tileCreatorTile = Instantiate(tileCreatorPrefab.gameObject, TileCreatorParent, false) as GameObject;
                    TileCreatorTileScript tileComponent = tileCreatorTile.GetComponent<TileCreatorTileScript>();
                    tileComponent.InitializeTile(sprite);
                    tileCreatorButtons.Add(tileComponent);
                }
                
            } else
            {
                Debug.LogError("object references for tilecreatortile creation was null");
            }


        } else
        {
            Debug.LogError("Session provided for refreshing tile creator tiles was null");
        }

    }

    public void SetRefreshTileCreatorButtonsWithCurrentSessionData()
    {
        if (Manager.currentManager != null && Manager.currentManager.session != null)
        {
            RefreshTileCreatorButtons(Manager.currentManager.session);
        } else
        {
            Debug.Log("tileMenu not updated. No Manager and no Session is assigned. tileMenu cannot use this data to update tiles");
        }

        
    }




    void ResetTileCreatorButtons()
    {
        foreach (TileCreatorTileScript tileCreator in tileCreatorButtons)
        {
            if (tileCreator != null)
            {
                Destroy(tileCreator.gameObject);
            }
        }

        tileCreatorButtons = new List<TileCreatorTileScript>();
    }

    public void Save()
    {
        if (Manager.currentManager != null && Manager.currentManager.session != null) {
            Manager.currentManager.session.SaveSession();
        } 
    }

    public Animator menuAnim;
    public void ToggleHideMenu()
    {
        if (menuAnim == null)
        {
            Debug.Log("Can't hide/show tileMenuCanvas. No Animator has been assigned");
            return;
        }

        menuAnim.SetBool("IsActive", !menuAnim.GetBool("IsActive"));


    }
}
