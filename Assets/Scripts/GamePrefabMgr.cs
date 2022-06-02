using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GamePrefabMgr : MonoBehaviour
{

    public static GamePrefabMgr inst { get; private set;}
    public GameObject itemPrefab;

    private void Awake()
    {
        GamePrefabMgr.inst = this;
    }

}
