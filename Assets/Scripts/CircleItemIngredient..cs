using UnityEngine;

public class CircleItemIngredient : CircleItemBase
{
    public CircleItemIngredient(Transform parent, Vector3 pos, float from, float to, EItemType type) : base(parent, pos, from, to, type)
    {
        m_spriteRenderer.color = Game.inst.GetColorByItemType(m_itemType);
        m_spriteRenderer.transform.localPosition = pos;
    }
}