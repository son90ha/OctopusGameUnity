using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class CircleItemPowerUp : CircleItemBase
{
    private EPowerupType m_curPowerupType = EPowerupType.NONE;
    private float m_curAppearRate = 0.0f;
    private TextMesh m_textMesh;
    public CircleItemPowerUp(Transform parent, Vector3 pos, float from, float to, EItemType type) : base(parent, pos, from, to, type)
    {
        m_curAppearRate = Game.inst.powerupData.powerupSpawnRate.rateAppearMin;

        m_spriteRenderer.gameObject.SetActive(false);
        var newGameObject = new GameObject("PowerupText");
        newGameObject.transform.SetParent(parent);
        m_textMesh = newGameObject.AddComponent<TextMesh>();
        m_textMesh.color = Color.black;
        m_textMesh.text = "";
        m_textMesh.anchor = TextAnchor.MiddleCenter;
        m_textMesh.alignment = TextAlignment.Center;
        m_textMesh.characterSize = 0.05f;
        m_textMesh.fontSize = 12;
        newGameObject.transform.localPosition = pos;
        newGameObject.transform.localScale = new Vector3(0.5f, 0.5f, 0.5f);

        GameEvent.CircleRotate_ThroughPowerup.AddListener(OnCircleRotateThrough);
        GameEvent.Game_PickPowerup.AddListener(OnPickPowerUp);
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
            var newList = Game.inst.powerupData.powerupWeightData.Where(e => e.pType != EPowerupType.EXTRA_HP || hasExtraLives).ToArray();
            m_curPowerupType = newList[Utils.PickRandIndexInWeight(newList)].pType;
            m_textMesh.text = m_curPowerupType.ToString();
        }
    }

    private bool CheckSpawnPowerup()
    {
        float randNum = Random.Range(0.0f, 1.0f);
        bool result = randNum <= m_curAppearRate;
        if (result)
        {
            m_curAppearRate = Game.inst.powerupData.powerupSpawnRate.rateAppearMin;
        }
        else
        {
            m_curAppearRate = Mathf.Min(m_curAppearRate + Game.inst.powerupData.powerupSpawnRate.rateIncrease, Game.inst.powerupData.powerupSpawnRate.rateAppearMax);
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
