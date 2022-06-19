using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class CircleRotate : MonoBehaviour
{
    public GameObject circleIcon;

    private float curSpeed = 150;
    private const float k_defaultAngle = 90;
    private bool m_inPowerupArea = false;
    void Awake()
    {
        GameEvent.Character_DataChanged.AddListener(OnCharacterDataChanged);
        GameEvent.Game_OrderWrong.AddListener(OnOrderWrong);
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

        if (Game.inst.IsPlaying)
        {
            float rotateAngle = Time.deltaTime * curSpeed;
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

        if (Game.inst.IsPlaying)
        {
            GameEvent.CircleRotate_Pick.Invoke(Utils.ConvertTo360Degree(transform.localEulerAngles.z));
        }
    }

    private void ResetRotateAngle()
    {
        transform.localEulerAngles = new Vector3(0, 0, k_defaultAngle);
    }

    private void OnCharacterDataChanged(OctopusData data)
    {
        this.curSpeed = data.tentacleSpeed;
    }

    private void OnOrderWrong()
    {
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
}
