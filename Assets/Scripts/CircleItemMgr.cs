using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class CircleItemMgr : MonoBehaviour
{

    public GameObject itemContainer;

    public GameObject blackLinePrefab;

    private List<CircleItemBase> m_listCircleItem = new List<CircleItemBase> { };

    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void createItem()
    {
        refreshContainer();
        var listItemType = Game.inst.ListItem;
        const int startAngle = 0;
        const float distance = 0.4f;
        Vector3 startVec = Vector3.forward;
        for (int i = 0, count = listItemType.Count; i < count; i++)
        {
            var curAngle = startAngle + i * 360 / count;
            var angleAxis = Quaternion.AngleAxis(curAngle, startVec);
            var position = angleAxis * (Vector3.right * distance);
            var angleFrom = Utils.ConvertTo360Degree(curAngle - (360 / count / 2));
            var angleTo = Utils.ConvertTo360Degree(curAngle + (360 / count / 2));
            var circleItem = CircleItemBase.CreateCircleItem(itemContainer.transform, position, angleFrom, angleTo, listItemType[i]);
            m_listCircleItem.Add(circleItem);

            var blackLine = Instantiate(blackLinePrefab, new Vector3(0, 0, 0), Quaternion.Euler(0, 0, angleFrom));
            blackLine.transform.SetParent(itemContainer.transform);
            blackLine.transform.localPosition = new Vector3(0, 0, 0);
            blackLine.transform.localScale = new Vector3(0.5f, 0.01f, 1f);
        }
    }

    void refreshContainer()
    {
        for (int i = 0, count = itemContainer.transform.childCount; i < count; i++)
        {
            GameObject.Destroy(itemContainer.transform.GetChild(i).gameObject);
        }
        m_listCircleItem.Clear();
    }

    public EItemType getItemFromAngle(float angle) {
        var index = m_listCircleItem.FindIndex((item) => 
        {   
            if (item.AngleFrom > item.AngleTo) {
                return item.AngleFrom <= angle || angle <= item.AngleTo;
            }
            return item.AngleFrom <= angle && angle <= item.AngleTo ;
        });

        if (index == -1) {
            Debug.Log("[CircleItemMgr] getItemFromAngle - CANNOT get item from angle = " + angle);
            return EItemType.BLACK;
        }
        
        return m_listCircleItem[index].ItemType;
    }
}
