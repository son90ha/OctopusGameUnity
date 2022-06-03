using System;
using System.Collections.Generic;

public class Utils
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

    private static Random rng = new Random();
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
}