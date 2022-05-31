using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class CircleItemMgr : MonoBehaviour
{
    public CircleItem circleItemPrefab;

    public GameObject itemContainer;

    private GameObject[] listItem { get; set;}
    private List<CircleItem> listCircleItem = new List<CircleItem>{};

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
            var newCircleItem = Instantiate(circleItemPrefab);
            var curAngle = startAngle + i * 360 / count;
            var angleAxis = Quaternion.AngleAxis(curAngle, startVec);
            var position = angleAxis * (Vector3.right * distance);
            var angleFrom = Utils.ConvertTo360Degree(curAngle - (360 / count / 2));
            var angleTo = Utils.ConvertTo360Degree(curAngle + (360 / count / 2));
            newCircleItem.Init(position, angleFrom, angleTo, listItemType[i]);
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
 
            listCircleItem.Add(newCircleItem);
        }
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
}
