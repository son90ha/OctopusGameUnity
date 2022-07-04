using System;
using System.Collections.Generic;
using System.Reflection;
using System.Linq;
using UnityEngine;

public static class Utils
{
    public static float ConvertTo360Degree(float angle)
    {
        var result = angle;

        if (System.Math.Abs(angle) >= 360) {
            result = result % 360;
        }
    
        if (result < 0) {
            result = 360 + (result % 360);
        } else if (result >= 360) {
            result = result % 360;
        }
    
        return result;
    }

    private static System.Random rng = new System.Random();

    public static List<T> Shuffle<T>(List<T> list)  
    {  
        int n = list.Count;  
        while (n > 1) {  
            n--;  
            int k = rng.Next(n + 1);  
            T value = list[k];  
            list[k] = list[n];  
            list[n] = value;  
        }  

        return list;
    }

    public static T GetRandomElementFromList<T>(List<T> list)
    {
        int randNum = UnityEngine.Random.Range(0, list.Count);
        return list[randNum];
    }

    public static int convertToInt(string str)
    {
        int result = 0;
        if (!Int32.TryParse(str, out result))
        {
            Debug.LogError(str + " CANNOT parse to INT");
        }

        return result;
    }

    public static float convertToFloat(string str)
    {
        float result = 0f;
        if (!float.TryParse(str, out result))
        {
            Debug.LogError(str + " CANNOT parse to INT");
        }

        return result;
    }

    public static List<ENUM> GetListFromEnum<ENUM>() where ENUM : struct, IConvertible
    {
        return new List<ENUM>((ENUM[])Enum.GetValues(typeof(ENUM)));
    }

    public static int PickRandIndexInWeight(EWeight[] weight)
    {
        int total = 0;
        foreach (var item in weight)
        {
            total += item.Weight;
        }
        int randNum = UnityEngine.Random.Range(0, total);
        int upBound = 0;
        int result = -1;
        for (int i = 0; i < weight.Length; i++)
        {
            upBound += weight[i].Weight;
            if (randNum < upBound)
            {
                result = i;
                break;
            }
        }

        if (result == -1)
        {
            Debug.LogError($"[Utils] PickRandIndexInWeightArr - CANNOT find weight: {JsonUtility.ToJson(weight, true)}");
        }

        return result;
    }
    public static int PickRandIndexInWeight(List<EWeight> weight)
    {
        return Utils.PickRandIndexInWeight(weight.ToArray());
    }

    public static int PickRandIndexInWeight(IEnumerable<EWeight> weight)
    {
        return Utils.PickRandIndexInWeight(weight.ToArray());
    }

    /// <summary>
    /// Loop all property in class and show in debug console
    /// </summary>
    public static void CheckAllValueOfClass<T>(T t)
    {
        foreach (var field in typeof(T).GetFields(BindingFlags.Instance |
                                                 BindingFlags.NonPublic |
                                                 BindingFlags.Public))
        {
            // Console.WriteLine("{0} = {1}", field.Name, field.GetValue(t));
            Debug.Log($"{field.Name} = {field.GetValue(t)}");
        }
    }
}