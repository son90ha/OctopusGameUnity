using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IOnPickPowerup
{
    public void OnPickPowerUp(EPowerupType powerupType);
}

public interface LogTag
{
    public string LOG_TAB { get; }
}