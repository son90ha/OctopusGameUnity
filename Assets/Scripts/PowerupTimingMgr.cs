using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PowerupTiming
{
    private float m_tinming = 0;
    private EPowerupType m_powerupType;
    private PowerupTimingMgr m_mgr;
    public EPowerupType PowerupType { get { return m_powerupType; } }
    private ArrowFillProgress m_arrowFillProgress;
    public PowerupTiming(float duration, EPowerupType powerupType, PowerupTimingMgr mgr)
    {
        m_mgr = mgr;
        m_tinming = duration;
        m_powerupType = powerupType;
        var newProgressObject = GameObject.Instantiate(GamePrefabMgr.inst.arrowFillProgress, Game.inst.localCharacter.powerupStatusTrans);
        m_arrowFillProgress = newProgressObject.GetComponent<ArrowFillProgress>();
        m_arrowFillProgress.setTime(duration);

        GameEvent.Powerup_ActiveChanged.Invoke(m_powerupType, true);
    }
    public void Update()
    {
        if (m_tinming > 0)
        {
            m_tinming -= Time.deltaTime;
            if (m_tinming <= 0)
            {
                m_tinming = 0;
                m_mgr.PowerupExpired(this);
                GameEvent.Powerup_ActiveChanged.Invoke(m_powerupType, false);
            }
        }
    }

    public void refreshTiming(float duration)
    {
        m_tinming = duration;
        m_arrowFillProgress.setTime(duration);
    }
}

public class PowerupTimingMgr
{
    private static readonly float s_extraPatienceDuration = 10.0f;
    private static readonly float s_slowTimeDuration = 10.0f;
    private static readonly float s_simplifyOrderDuration = 10.0f;
    private static readonly float s_scoreMultiplierDuration = 10.0f;
    private static readonly float s_increaseIngredientSizeDuration = 10.0f;
    private List<PowerupTiming> m_powerupTimingList = new List<PowerupTiming>();
    private Dictionary<EPowerupType, float> m_powerupTypeDurationDic;
    private Dictionary<EPowerupType, bool> m_powerupStatusDic = new Dictionary<EPowerupType, bool>();
    public PowerupTimingMgr()
    {
        GameEvent.Game_PickPowerup.AddListener(OnPickPowerUp);
        m_powerupTypeDurationDic = new Dictionary<EPowerupType, float>()
        {
            { EPowerupType.EXTRA_PATIENCE, s_extraPatienceDuration },
            { EPowerupType.INCREASE_INGREDIENT_WHEEL_SIZE, s_slowTimeDuration },
            { EPowerupType.SCORE_MULTIPLIER, s_simplifyOrderDuration },
            { EPowerupType.SIMPLIFY_ORDER, s_scoreMultiplierDuration },
            { EPowerupType.SLOW_TIME, s_increaseIngredientSizeDuration },
        };
    }

    public void Update()
    {
        for (int i = m_powerupTimingList.Count - 1; i >= 0; i--)
        {
            m_powerupTimingList[i].Update();
        }
    }

    public void OnPickPowerUp(EPowerupType powerupType)
    {
        switch (powerupType)
        {
            case EPowerupType.EXTRA_PATIENCE:
            case EPowerupType.INCREASE_INGREDIENT_WHEEL_SIZE:
            case EPowerupType.SCORE_MULTIPLIER:
            case EPowerupType.SIMPLIFY_ORDER:
            case EPowerupType.SLOW_TIME:
                {
                    float duration = 0;
                    m_powerupTypeDurationDic.TryGetValue(powerupType, out duration);
                    int index = m_powerupTimingList.FindIndex((e) =>
                    {
                        return e.PowerupType == powerupType;
                    });
                    if (index >= 0)
                    {
                        m_powerupTimingList[index].refreshTiming(duration);
                    }
                    else
                    {
                        var newPowerTiming = new PowerupTiming(duration, powerupType, this);
                        m_powerupTimingList.Add(newPowerTiming);
                    }
                    refreshStatus();
                    break;
                }
            default: return;
        }
    }
    public void PowerupExpired(PowerupTiming powerupTiming)
    {
        m_powerupTimingList.Remove(powerupTiming);
        refreshStatus();
    }

    private void refreshStatus()
    {
        Dictionary<EPowerupType, bool> newStatus = new Dictionary<EPowerupType, bool>();
        foreach (var item in m_powerupTimingList)
        {
            newStatus.Add(item.PowerupType, true);
        }

        m_powerupStatusDic = newStatus;
    }

    public bool IsPowerupActive(EPowerupType powerupType)
    {
        bool result = false;
        if (m_powerupStatusDic.TryGetValue(powerupType, out result))
        {
            return result;
        }
        return false;
    }
}
