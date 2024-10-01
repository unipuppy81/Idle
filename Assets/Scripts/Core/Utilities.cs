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
}
