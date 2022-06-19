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
}
