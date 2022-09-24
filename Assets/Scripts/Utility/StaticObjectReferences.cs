using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StaticObjectReferences : MonoBehaviour {

    public static StaticObjectReferences currentReferences;

    private void Awake()
    {
        if (currentReferences == null)
        {
            currentReferences = this;
        }
    }


    [SerializeField]
    Transform _TileParent;
    public Transform TileParent { get { return _TileParent; } }

    [SerializeField]
    TileScript _TilePrefab;
    public TileScript TilePrefab { get { return _TilePrefab; } }

    [SerializeField]
    Transform _TileCreatorTileParent;
    public Transform TileCreatorTileParent { get { return _TileCreatorTileParent; } }


    [SerializeField]
    TileCreatorTileScript _TileCreatorPrefab;
    public TileCreatorTileScript TileCreatorPrefab { get { return _TileCreatorPrefab; } }

    //public Image backgroundImage;

    

}
