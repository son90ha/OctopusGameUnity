using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CircleItem
{

    public SpriteRenderer spriteRenderer;
    private float m_angleFrom;
    public float AngleFrom { get { return m_angleFrom; } }
    private float m_angleTo;
    public float AngleTo {get { return m_angleTo; } }
    EItemType m_itemType;
    public EItemType ItemType { get { return m_itemType; } }


    public CircleItem(SpriteRenderer sr, Vector3 position, float from, float to, EItemType type)
    {
        m_angleFrom = from;
        m_angleTo = to;
        m_itemType = type;

        if (type == EItemType.POWER_UP)
        {
            GameObject.Destroy(sr);
        }
        else
        {
            spriteRenderer = sr;
            spriteRenderer.color = Game.inst.GetColorByItemType(m_itemType);
            spriteRenderer.transform.localPosition = position;
        }

    }
}
