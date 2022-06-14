using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

enum ECircleRotateState {
    ROTATING,
    STOP,
}

public class CircleRotate : MonoBehaviour
{
    public GameObject circleIcon;

    private float curSpeed = 150;
    private ECircleRotateState curState = ECircleRotateState.STOP;
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

    private void OnOrderWrong()
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
        
        if (curState == ECircleRotateState.ROTATING) {
            float rotateAngle = Time.deltaTime * curSpeed;
            transform.Rotate(Vector3.forward, -rotateAngle);
        }
    }

    void StartRotate()
    {
        curState = ECircleRotateState.ROTATING;
    }

    void StopRotate()
    {
        curState = ECircleRotateState.STOP;
        GameEvent.CircleRotate_Stop.Invoke(Utils.ConvertTo360Degree(transform.localEulerAngles.z));
    }

    void OnClick()
    {
        if (Game.inst.IsPlaying)
        {
            if (curState == ECircleRotateState.STOP)
            {
                StartRotate();
            }
            else
            {
                StopRotate();
            }

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
}
