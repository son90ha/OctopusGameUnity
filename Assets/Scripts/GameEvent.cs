using System.Collections.Generic;
using UnityEngine.Events;



public static class GameEvent
{
    // CircleRotate Event
    public static UnityEvent<float> CircleRotate_Stop = new UnityEvent<float>();

    // Character Event
    public static UnityEvent<List<EItemType>> Character_GetAnItem = new UnityEvent<List<EItemType>>();
    public static UnityEvent<int> Character_ScoreChanged = new UnityEvent<int>();

    // Game Event
    public static UnityEvent Game_OrderFinish = new UnityEvent();
    public static UnityEvent Game_OrderWrong = new UnityEvent();

    // Customer Event
    public static UnityEvent Customer_TimeOut = new UnityEvent();
}
