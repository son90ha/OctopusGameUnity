using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CircleItem
{

    public SpriteRenderer spriteRenderer;
    private float angleFrom { get; set;}
    private float angleTo { get; set;}
    EItemType itemType { get; set;}
    

    public CircleItem(SpriteRenderer sr, Vector3 position, float from, float to, EItemType type)
    {
        angleFrom = from;
        angleTo = to;
        itemType = type;
        spriteRenderer = sr;
        spriteRenderer.color = Game.inst.GetColorByItemType(itemType);
        spriteRenderer.transform.position = position;
    }
}
