using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CircleItemPowerUp : CircleItemBase
{
    public CircleItemPowerUp(Transform parent, Vector3 pos, float from, float to, EItemType type) : base(parent, pos, from, to, type)
    {
        m_spriteRenderer.gameObject.SetActive(false);
    }
}
