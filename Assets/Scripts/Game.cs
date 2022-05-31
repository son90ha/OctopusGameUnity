using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class Game : MonoBehaviour
{   

    private static Game _inst;
    public static Game inst
    { 
        get { return Game._inst; }
    }

    private List<EItemType> m_listItem;
    public List<EItemType> ListItem
    {   
        get { return m_listItem; }
    }

    void Awake() {
        Game._inst = this;
        resetListItem(4);
    }

    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
    }

    void resetListItem(int count) 
    {
        m_listItem = CONST.ListItemType.GetRange(0, count);
    }
}
