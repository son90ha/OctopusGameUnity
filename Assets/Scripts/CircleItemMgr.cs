using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class CircleItemMgr : MonoBehaviour, ELogTag
{

    public GameObject itemContainer;

    public GameObject blackLinePrefab;

    private List<CircleItemBase> m_listCircleItem = new List<CircleItemBase> { };
    private CircleItemBase m_powerupItem = null;
    public CircleItemBase PowerupItem
    {
        get { return m_powerupItem; }
    }

    void Awake()
    {
        GameEvent.Powerup_ActiveChanged.AddListener(OnPowerupActiveChanged);
    }

    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void createItem(bool hasIncreSizePowerup = false)
    {
        refreshContainer();
        var listItemType = Game.inst.ListItem;
        float startAngle = 45;
        const float distance = 0.4f;
        Vector3 startVec = Vector3.forward;
        int count = listItemType.Count;
        float angleRange = 360 / count;
        for (int i = 0; i < count; i++)
        {
            float angleFrom = startAngle;
            float angleTo = angleFrom + angleRange;
            if (hasIncreSizePowerup)
            {
                if (listItemType[i] == EItemType.POWER_UP)
                {
                    angleTo -= (Game.inst.powerupData.increaseIngredientWheelSize * (count - 1));
                    if (angleTo < angleFrom)
                    {
                        Debug.LogWarning($"{LOG_TAB} - Increase size EXCEED 360 degree");
                        angleTo = angleFrom;
                    }
                }
                else
                {
                    angleTo += Game.inst.powerupData.increaseIngredientWheelSize;
                }
            }
            if (angleTo > 360)
            {
                angleTo = Utils.ConvertTo360Degree(angleTo);
                if (angleTo > startAngle)
                {
                    Debug.LogWarning(LOG_TAB + "Why over one circle");
                    angleTo = startAngle;
                }
            }
            float curAngle = (angleFrom + angleTo) * 0.5f;
            if (angleFrom > angleTo)
            {
                curAngle = Utils.ConvertTo360Degree((angleFrom + angleTo + 360) * 0.5f);
            }
            var angleAxis = Quaternion.AngleAxis(curAngle, startVec);
            var position = angleAxis * (Vector3.right * distance);
            var circleItem = CircleItemBase.CreateCircleItem(itemContainer.transform, position, angleFrom, angleTo, listItemType[i]);
            if (listItemType[i] == EItemType.POWER_UP)
            {
                m_powerupItem = circleItem;
            }
            m_listCircleItem.Add(circleItem);

            var blackLine = Instantiate(blackLinePrefab, new Vector3(0, 0, 0), Quaternion.Euler(0, 0, angleFrom));
            blackLine.transform.SetParent(itemContainer.transform);
            blackLine.transform.localPosition = new Vector3(0, 0, 0);
            blackLine.transform.localScale = new Vector3(0.5f, 0.01f, 1f);

            startAngle = angleTo;
        }
    }

    void refreshContainer()
    {
        for (int i = 0, count = itemContainer.transform.childCount; i < count; i++)
        {
            GameObject.Destroy(itemContainer.transform.GetChild(i).gameObject);
        }
        m_listCircleItem.ForEach(e => e.OnDestroy());
        m_listCircleItem.Clear();
        m_powerupItem = null;
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
            Debug.LogWarning("[CircleItemMgr] getItemFromAngle - CANNOT get item from angle = " + angle);
            return EItemType.BLACK;
        }
        
        return m_listCircleItem[index].ItemType;
    }

    private void OnPowerupActiveChanged(EPowerupType powerupType, bool active)
    {
        if (powerupType == EPowerupType.INCREASE_INGREDIENT_WHEEL_SIZE)
        {
            createItem(active);
        }
    }

    public string LOG_TAB => $"[{this.GetType().Name}]";
}
