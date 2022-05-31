using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class CircleItemMgr : MonoBehaviour
{
    public GameObject itemPrefab;

    public GameObject itemContainer;

    private GameObject[] m_listItem;

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
        Debug.Log(Game.inst);
        Debug.Log(Game.inst.ListItem.Count);
        var listItemType = Game.inst.ListItem;
        const int startAngle = 0;
        Vector3 startVec = Vector3.forward;
        // const int distance = 180;

        for (int i = 0, count = listItemType.Count; i < count; i++) {
            var newItem = Instantiate(itemPrefab);
            var curAngle = startAngle + i * 360 / count;
            var angleAxis = Quaternion.AngleAxis(curAngle, startVec);
            Debug.Log(angleAxis);
            newItem.transform.position = angleAxis * Vector3.right;
            // newItem.setParent(this.itemContainer);
            // const curAngle = startAngle + i * 360 / count;
            // const pos = startVec.rotate(cc.misc.degreesToRadians(curAngle), cc.v2()).mul(distance);
            // newItem.setPosition(pos);
            // const itemType = listItemType[i];
            // const color = cc.Color[itemType];
            // newItem.color = color;
            // const angleFrom = Utils.convertTo360Degree(curAngle - (360 / count / 2));
            // const angleTo = Utils.convertTo360Degree(curAngle + (360 / count / 2));
            // const circleItem: CircleItem = {
            //     sprite: newItem.getComponent(cc.Sprite),
            //     angleFrom: angleFrom,
            //     angleTo: angleTo,
            //     itemType: itemType
            // }
 
            // const blackLine = cc.instantiate(INST.AssetsManager.getBlackLinePrefab());
            // blackLine.setParent(this.blackLineContainer);
            // blackLine.width = 250;
            // blackLine.angle = angleFrom;
 
            // this.listCircleItem.push(circleItem);
        }
    }

    void refreshContainer()
    {
        if (m_listItem == null) {
            return;
        }
        foreach (var item in m_listItem)
        {
            GameObject.Destroy(item);
        }
        Array.Clear(m_listItem, 0, m_listItem.Length);
    }
}
