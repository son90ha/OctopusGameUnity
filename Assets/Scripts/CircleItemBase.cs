
using UnityEngine;

public abstract class CircleItemBase
{
    public static CircleItemBase CreateCircleItem(Transform parent, Vector3 pos, float from, float to, EItemType type)
    {
        switch (type)
        {
            case EItemType.POWER_UP: return new CircleItemPowerUp(parent, pos, from, to, type);
            default: return new CircleItemIngredient(parent, pos, from, to, type);
        }
    }

    protected SpriteRenderer m_spriteRenderer;
    protected float m_angleFrom;
    public float AngleFrom { get { return m_angleFrom; } }
    protected float m_angleTo;
    public float AngleTo { get { return m_angleTo; } }
    protected EItemType m_itemType;
    public EItemType ItemType { get { return m_itemType; } }

    public CircleItemBase(Transform parent, Vector3 pos, float from, float to, EItemType type)
    {
        m_angleFrom = from;
        m_angleTo = to;
        m_itemType = type;

        var newItem = UnityEngine.Object.Instantiate(GamePrefabMgr.inst.itemPrefab);
        newItem.transform.SetParent(parent);
        m_spriteRenderer = newItem.GetComponent<SpriteRenderer>();
    }

    virtual public EPowerupType GetPowerupInfo()
    {
        Debug.LogError("[CircleItemBase] GetPowerupInfo - Method not implement for itemType: " + m_itemType.ToString());
        return EPowerupType.NONE;
    }
}