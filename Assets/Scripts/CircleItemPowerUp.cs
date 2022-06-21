using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CircleItemPowerUp : CircleItemBase, IOnPickPowerup
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
        GameEvent.Game_PickPowerup.AddListener(OnPickPowerUp);

        m_spriteRenderer.gameObject.SetActive(false);
        m_listPowerupType = Utils.GetListFromEnum<EPowerupType>();

        // Remove NONE type
        int indexOfNone = m_listPowerupType.IndexOf(EPowerupType.NONE);
        if (indexOfNone >= 0)
        {
            m_listPowerupType.RemoveAt(m_listPowerupType.IndexOf(EPowerupType.NONE));
        }

        // Remove ExtraLives type to make a condition after.
        int indexOfExtraHp = m_listPowerupType.IndexOf(EPowerupType.EXTRA_HP);
        if (indexOfExtraHp >= 0)
        {
            m_listPowerupType.RemoveAt(indexOfExtraHp);
        }

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
        return m_curPowerupType;
    }

    private void OnCircleRotateThrough()
    {
        if (m_curPowerupType != EPowerupType.NONE) { return; }

        if (CheckSpawnPowerup())
        {
            var local = Game.inst.localCharacter;
            bool hasExtraLives = local.Lives < local.octopusData.octopusLives;
            List<EPowerupType> newList = new List<EPowerupType>(m_listPowerupType);
            if (hasExtraLives)
            {
                newList.Add(EPowerupType.EXTRA_HP);
            }
            m_curPowerupType = Utils.GetRandomElementFromList(newList);
            // m_curPowerupType = EPowerupType.SLOW_TIME;
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

        // return true;
        return result;
    }

    public void OnPickPowerUp(EPowerupType powerupType)
    {
        m_curPowerupType = EPowerupType.NONE;
        m_textMesh.text = "";
    }

    public override void OnDestroy()
    {
        GameEvent.CircleRotate_ThroughPowerup.RemoveListener(OnCircleRotateThrough);
        GameEvent.Game_PickPowerup.RemoveListener(OnPickPowerUp);
    }
}
