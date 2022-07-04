using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[System.Serializable]
public class PowerupWeightData : EWeight
{
    public EPowerupType pType;
    public int weight;

    public int Weight => weight;
}
[CreateAssetMenu(fileName = "Data", menuName = "ScriptableObjects/SpawnManagerScriptableObject", order = 1)]
public class PowerupDataScriptTable : ScriptableObject
{
    public float extraPatienceTime;
    public float slowTimeValue;
    public int simplifyOrderDecreValue;
    public float scoreMultiplierValue;
    public float increaseIngredientWheelSize;
    public PowerupWeightData[] powerupWeightData;
    [System.Serializable]
    public class PowerupSpawnRate
    {
        public float rateAppearMax = 1.0f;
        public float rateAppearMin = 0.1f;
        public float rateIncrease = 0.1f;
    }
    public PowerupSpawnRate powerupSpawnRate;
}
