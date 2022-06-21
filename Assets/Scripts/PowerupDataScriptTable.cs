using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Data", menuName = "ScriptableObjects/SpawnManagerScriptableObject", order = 1)]
public class PowerupDataScriptTable : ScriptableObject
{
    public float extraPatienceTime;
    public float slowTimeTime;
    public float simplifyOrderTime;
    public float scoreMultiplierTime;
    public float increaseIngredientWheelSizeTime;
}
