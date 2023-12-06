using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using UnityEditor;
using UnityEngine;
using static Unity.VisualScripting.Member;

public static class AssetGenerator
{

//    public static void CreateAssets<T>(string csvFilePath, string outputFolder) where T : ScriptableObject
//    {

//        string[] lines = BetterStreamingAssets.ReadAllLines(csvFilePath);
//        int n = 0;
//        //var props = lines[0].Split('\t');
//        foreach (string line in lines.Skip(2))
//        {

//           n++;
//            Debug.Log(n);
//            T asset = ScriptableObject.CreateInstance<T>();


//            PropertyInfo[] props = typeof(T).GetProperties();

//            string[] values = line.Split('\t');

//            for (int i = 0; i < props.Length-1; i++)
//            {

//                var prop = props[i];

//                if (prop.PropertyType == typeof(string))
//                {
////                    prop.SetValue(asset, values[i]);
//                }
//                else if (prop.PropertyType == typeof(HideFlags))
//                {
//                    HideFlags flags = ParseHideFlags(values[i]);
//                    prop.SetValue(asset, flags);
//                }
//                else
//                {
//                    prop.SetValue(asset, Convert.ChangeType(values[i], prop.PropertyType));
//                }
//            }

//            string assetName = values[0].Replace(" ", "");

//            string assetPath = Path.Combine(outputFolder, assetName + ".asset");

//            AssetDatabase.CreateAsset(asset, assetPath);
//        }

//        AssetDatabase.Refresh();

//    }

//    private static HideFlags ParseHideFlags(string v)
//    {
//        if (v == "None") return HideFlags.None;
//        else if (v == "NotEditable") return HideFlags.NotEditable;

//        // ...

//        return HideFlags.None;
//    }
}



