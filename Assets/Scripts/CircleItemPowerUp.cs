using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CircleItemPowerUp : CircleItemBase
{
    private EPowerupType m_curPowerupType = EPowerupType.NONE;
    private static readonly float s_rateAppearMin = 0.1f;
    private static readonly float s_rateAppearMax = 1.0f;
    private static readonly float s_rateIncrease = 0.1f;
    private float m_curAppearRate = CircleItemPowerUp.s_rateAppearMin;
    private List<EPowerupType> m_listPowerupType;
    private TextMesh m_textMesh;
    public CircleItemPowerUp(Transform parent, Vector3 pos, float from, float to, EItemType type) : base(parent, pos, from, to, type)
    {
        GameEvent.CircleRotate_ThroughPowerup.AddListener(OnCircleRotateThrough);

        m_spriteRenderer.gameObject.SetActive(false);
        m_listPowerupType = Utils.GetListFromEnum<EPowerupType>();
        m_listPowerupType.RemoveAt(m_listPowerupType.IndexOf(EPowerupType.NONE));

        var newGameObject = new GameObject("PowerupText");
        newGameObject.transform.SetParent(parent);
        m_textMesh = newGameObject.AddComponent<TextMesh>();
        m_textMesh.color = Color.black;
        m_textMesh.text = "";
        m_textMesh.anchor = TextAnchor.MiddleRight;
        m_textMesh.alignment = TextAlignment.Right;
        m_textMesh.characterSize = 0.05f;
        m_textMesh.fontSize = 12;
        newGameObject.transform.localPosition = pos + new Vector3(0.05f, 0, 0);
        newGameObject.transform.localScale = new Vector3(0.5f, 0.5f, 0.5f);
    }

    public override EPowerupType GetPowerupInfo()
    {
        return EPowerupType.EXTRA_HP;
    }

    private void OnCircleRotateThrough()
    {
        if (m_curPowerupType != EPowerupType.NONE) { return; }

        if (CheckSpawnPowerup())
        {
            m_curPowerupType = Utils.GetRandomElementFromList(m_listPowerupType);
            m_textMesh.text = m_curPowerupType.ToString();
        }
    }

    private bool CheckSpawnPowerup()
    {
        float randNum = Random.Range(0.0f, 1.0f);
        bool result = randNum <= m_curAppearRate;
        if (result)
        {
            m_curAppearRate = s_rateAppearMin;
        }
        else
        {
            m_curAppearRate = Mathf.Min(m_curAppearRate + s_rateIncrease, s_rateAppearMax);
        }

        return result;
    }
}
