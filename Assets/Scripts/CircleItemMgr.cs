using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class CircleItemMgr : MonoBehaviour
{

    public GameObject itemContainer;

    public GameObject blackLinePrefab;

    private GameObject[] listItem { get; set;}

    private List<CircleItem> m_listCircleItem = new List<CircleItem>{};

    // Start is called before the first frame update
    void Start()
    {
        createItem();
    }

    // Update is called once per frame
    void Update()
    {

    }
    
    void createItem()
    {
        refreshContainer();
        var listItemType = Game.inst.ListItem;
        const int startAngle = 0;
        const int distance = 2;
        Vector3 startVec = Vector3.forward;
        for (int i = 0, count = listItemType.Count; i < count; i++) {
            var newItem = Instantiate(GamePrefabMgr.inst.itemPrefab);
            newItem.transform.SetParent(itemContainer.transform);
            var curAngle = startAngle + i * 360 / count;
            var angleAxis = Quaternion.AngleAxis(curAngle, startVec);
            var position = angleAxis * (Vector3.right * distance);
            var angleFrom = Utils.ConvertTo360Degree(curAngle - (360 / count / 2));
            var angleTo = Utils.ConvertTo360Degree(curAngle + (360 / count / 2));
            var circleItem = new CircleItem(newItem.GetComponent<SpriteRenderer>(), position, angleFrom, angleTo, listItemType[i]);
            m_listCircleItem.Add(circleItem);

            var blackLine = Instantiate(blackLinePrefab, new Vector3(0, 0, 0), Quaternion.Euler(0, 0, angleFrom));
            blackLine.transform.SetParent(itemContainer.transform);
            blackLine.transform.localScale = new Vector3(0.5f, 0.01f, 1f);
        }

        transform.parent.position = new Vector3(0f, -1.5f, 0f);
    }

    void refreshContainer()
    {
        if (listItem == null) {
            return;
        }
        foreach (var item in listItem)
        {
            GameObject.Destroy(item);
        }
        Array.Clear(listItem, 0, listItem.Length);
    }

    public EItemType getItemFromAngle(float angle) {
        var index = this.m_listCircleItem.FindIndex((item) => 
        {
            return item.AngleFrom <= angle && angle <= item.AngleTo ;
        });

        if (index == -1) {
            Debug.Log("[CircleItemMgr] getItemFromAngle - CANNOT get item from angle = " + angle);
            return EItemType.BLACK;
        }
        
        return m_listCircleItem[index].ItemType;
    }
}
