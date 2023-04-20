using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HelpMenu : MonoBehaviour {

	private bool _isShowHelp = true;
	public bool isShowHelp { get { return _isShowHelp; } set {
			_isShowHelp = value;
			gameObject.SetActive(value);
		} 
	}

	void Awake()
    {
		EventManager.OnSetHelpActive += SetHelpMenuActivation;
    }

	void OnDestroy()
    {
		EventManager.OnSetHelpActive -= SetHelpMenuActivation;
	}

	public void SetHelpMenuActivation(bool isActive)
    {
		isShowHelp = isActive;
    }

	public Transform HelpTileParentTransform;
	public HelpTileScript helpTilePrefab;

	public List<HelpTileScript> helpTileList;

	void OnEnable()
    {
		EventManager.OnDisplayHelp += AddHelpTile;
		EventManager.OnEndDisplayHelp += DestroyHelpTileByHelpText;
    }

	void OnDisable()
	{
		EventManager.OnDisplayHelp -= AddHelpTile;
		EventManager.OnEndDisplayHelp -= DestroyHelpTileByHelpText;

		DestroyAllHelpTiles();
	}

	public void AddHelpTile(HelpText currentHelpText)
    {
        if (currentHelpText == null)
        {
			Debug.Log("Supplied help text was null. Ignored by DisplayHelpMenu.");
			return;
        }

		if (HelpTileParentTransform == null)
        {
			Debug.Log("No HelpTileParentTransform has been assigned for helpTileMenuScript. HelpTile was not shown");
			return;
        }

		HelpTileScript currentHelpTile = Instantiate(helpTilePrefab, HelpTileParentTransform);
		currentHelpTile.InitializeHelpTile(currentHelpText);

		helpTileList.Add(currentHelpTile);

	}

	public void DestroyHelpTileByHelpText(HelpText currentHelpText)
    {

		HelpTileScript tempHelpTile = null;

		foreach (HelpTileScript currentHelpTile in helpTileList)
        {
            if (currentHelpText == currentHelpTile.CurrentHelpText)
            {
				tempHelpTile = currentHelpTile;

            }
        }


        if (tempHelpTile != null)
        {
            try {
				helpTileList.Remove(tempHelpTile);
				Destroy(tempHelpTile.gameObject);
			} catch
            {
				Debug.Log("Issue with removing helpTextTile");
            }
			
		}
		
	}

	public void DestroyAllHelpTiles()
	{
		foreach (HelpTileScript currentHelpTile in helpTileList)
		{
				Destroy(currentHelpTile.gameObject);
		}
	}
}
