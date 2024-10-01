using System;
using System.Linq;
using System.Text;
using UnityEditor;
using UnityEngine;

public class Utilities
{
    public static T GetOrAddComponent<T>(GameObject obj) where T : Component
    {
        return obj.GetComponent<T>() ?? obj.AddComponent<T>();
    }


    #region NumberConvert
    public static string ConvertToString(int number) => Convert(number);
    public static string ConvertToString(long number) => Convert(number);
    private static string Convert(long number)
    {
        //만 자리 이하일 경우 그냥 반환
        if (number < 1_000)
        {
            return number.ToString();
        }
        string[] numSymbol = { "", "A ", "B  ", "C ", "D ", "E ", "F", "G", "H", "I", "J", "K" };
        int magnitudeIndex = (int)Mathf.Log10(number) / 3;
        StringBuilder sb = new StringBuilder()

            .Append((number * Mathf.Pow(0.001f, magnitudeIndex)).ToString("N2"))
            .Append(numSymbol[magnitudeIndex]);

        return sb.ToString();
    }

    #endregion
}
