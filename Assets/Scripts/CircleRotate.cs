using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class CircleRotate : MonoBehaviour
{
    public GameObject circleIcon;
    private float m_baseSpeed = 150;
    private float m_bonusSpeed = 0;
    private const float k_defaultAngle = 90;
    private bool m_inPowerupArea = false;
    private bool m_isStopped = false;
    void Awake()
    {
        GameEvent.Character_DataChanged.AddListener(OnCharacterDataChanged);
        GameEvent.Game_OrderWrong.AddListener(OnOrderWrong);
        GameEvent.Powerup_ActiveChanged.AddListener(OnPowerupActiveChanged);
    }

    // Start is called before the first frame update
    void Start()
    {
        ResetRotateAngle();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            OnClick();
        }

        if (Game.inst.IsPlaying && !m_isStopped)
        {
            float rotateAngle = Time.deltaTime * GetCurSpeed();
            transform.Rotate(Vector3.forward, -rotateAngle);
            CheckInPowerupArea(Utils.ConvertTo360Degree(transform.localEulerAngles.z));
        }
    }

    void OnClick()
    {
        if (EventSystem.current.currentSelectedGameObject != null && EventSystem.current.currentSelectedGameObject.GetComponent<UnityEngine.UI.Button>() != null)
        {
            return;
        }

        if (m_isStopped)
        {
            m_isStopped = false;
        }
        else
        {
            if (Game.inst.IsPlaying)
            {
                GameEvent.CircleRotate_Pick.Invoke(Utils.ConvertTo360Degree(transform.localEulerAngles.z));
            }
        }
    }

    private void ResetRotateAngle()
    {
        transform.localEulerAngles = new Vector3(0, 0, k_defaultAngle);
    }

    private void OnCharacterDataChanged(OctopusData data)
    {
        this.m_baseSpeed = data.tentacleSpeed;
    }

    private void OnOrderWrong()
    {
        m_isStopped = true;
        ResetRotateAngle();
    }

    private void CheckInPowerupArea(float angle)
    {
        CircleItemBase powerupItem = Game.inst.circleItemMgr.PowerupItem;
        bool isInPowerupArea = false;
        if (powerupItem.AngleFrom > powerupItem.AngleTo)
        {
            isInPowerupArea = powerupItem.AngleFrom <= angle || angle <= powerupItem.AngleTo;
        }
        else
        {
            isInPowerupArea = powerupItem.AngleFrom <= angle && angle <= powerupItem.AngleTo;
        }

        if (m_inPowerupArea != isInPowerupArea)
        {
            m_inPowerupArea = isInPowerupArea;
            if (m_inPowerupArea == false)
            {
                GameEvent.CircleRotate_ThroughPowerup.Invoke();
            }
        }
    }

    private void OnPowerupActiveChanged(EPowerupType powerupType, bool active)
    {
        if (powerupType == EPowerupType.SLOW_TIME)
        {
            m_bonusSpeed = active ? -Game.inst.powerupData.slowTimeValue * m_baseSpeed : 0;
            if (active)
            {
                // Apply slow time powerup
            }
        }
    }

    private float GetCurSpeed()
    {
        return m_baseSpeed + m_bonusSpeed;
    }
}
