using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class CircleItemMgr : MonoBehaviour
{
    public SpriteRenderer itemPrefab;

    public GameObject itemContainer;

    public GameObject blackLinePrefab;

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
            var newItem = Instantiate(itemPrefab);
            newItem.transform.SetParent(itemContainer.transform);
            var curAngle = startAngle + i * 360 / count;
            var angleAxis = Quaternion.AngleAxis(curAngle, startVec);
            var position = angleAxis * (Vector3.right * distance);
            var angleFrom = Utils.ConvertTo360Degree(curAngle - (360 / count / 2));
            var angleTo = Utils.ConvertTo360Degree(curAngle + (360 / count / 2));
            var circleItem = new CircleItem(newItem, position, angleFrom, angleTo, listItemType[i]);
            listCircleItem.Add(circleItem);

            var blackLine = Instantiate(blackLinePrefab, new Vector3(0, 0, 0), Quaternion.Euler(0, 0, angleFrom));
            blackLine.transform.SetParent(itemContainer.transform);
            blackLine.transform.localScale = new Vector3(0.5f, 0.01f, 1f);
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
