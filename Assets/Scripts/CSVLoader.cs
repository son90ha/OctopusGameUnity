using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class CSVLoader
{
    public static string[][] ParseArray(string text, string splitText = ",")
    {
        string[] lines = text.Split("\n");
        string[][] elements = new string[lines.Length][];
        for (int i = 0; i < lines.Length; i++)
        {
            elements[i] = lines[i].Split(splitText);
        }

        return elements;
    }
}
