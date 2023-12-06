using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Csv;
using UnityEditor;
using UnityEngine;

public class ConvertTSVtoSO : MonoBehaviour
{
    public string[] listFilePaths;
    private Dictionary<string, System.Type> objectClassMap;
    private int monsterIndex;

    // Start is called before the first frame update
    void Start()
    {
        objectClassMap = new Dictionary<string, System.Type>()
        {
            { "Monster", typeof(Monster) },
            { "Item", typeof(Nullable) }, // Add more object classes here
        };  
        GetData();
    }

    private void GetData()
    {
       
        GetMonsterData();
    }

    private void GetMonsterData()
    {
        //CleanFolder("Assets/Resources/MonstersSO/");
        listFilePaths = BetterStreamingAssets.GetFiles("Data/Monsters", "*.tsv", SearchOption.AllDirectories);
        print("C" + listFilePaths.Count());
        monsterIndex = 0;
        foreach(var monsterPath in listFilePaths)
        {
            //ConvertTSVToScriptableObjects(monsterPath);
//            AssetGenerator.CreateAssets<Monster>(monsterPath, "Assets/Resources/MonstersSO");
        }
    }
    

    public void ConvertTSVToScriptableObjects(string filePath)
    {
        string[] lines = BetterStreamingAssets.ReadAllLines(filePath);
        string objectClassName = lines[0];
        string[] fieldNames = lines[1].Split('\t');
        foreach (string line in lines.Skip(3))
        { 
            var scriptableObject = ScriptableObject.CreateInstance(objectClassMap[objectClassName]);
        
        
            var fields = scriptableObject.GetType().GetFields();
            for (int i = 0; i < fields.Length; i++)
            {
                var field = fields[i];
                var value = line.Split('\t')[i];
                if (value.Contains(".") || value is not string)
                {
                    try
                    {
                        field.SetValue(scriptableObject, float.Parse(value));
                    }
                    catch (Exception ex)
                    {
                        // Handle the conversion error
                        field.SetValue(scriptableObject, value.ToString());
                    }
                    
                }
                else
                {
                    field.SetValue(scriptableObject, value.ToString());
                }
                
            }
//            AssetDatabase.CreateAsset(scriptableObject, "Assets/Resources/MonstersSO/" + line.Split('\t')[0] + ".asset");
        }
           

        // Đọc tệp TSV

        // Tạo danh sách Scriptable Object
        //List<ScriptableObject> objects = new List<ScriptableObject>();

        //// Lặp qua các hàng của tệp TSV
        //foreach (string line in lines.Skip(3))
        //{
        //    // Tách các cột của hàng
        //    string[] columns = line.Split('\t');

        //    // Lấy lớp Scriptable Object tương ứng với cột đầu tiên
        //    System.Type objectClass = objectClassMap[objectClassName];
        //    ScriptableObject obj = (ScriptableObject)Activator.CreateInstance(objectClassMap[objectClassName]);
        //    // Tạo một đối tượng mới

        //    // Đặt giá trị cho các thuộc tính của đối tượng
        //    for (int i = 0; i < columns.Length; i++)
        //    {
        //        if (obj != null)
        //        {
        //            obj.GetType().GetField(fieldNames[i]).SetValue(obj, columns[i].ToString());
        //        }
        //    }
        //    AssetDatabase.CreateAsset(obj, "Assets/Resources/MonstersSO/" + columns[0] + ".asset");
        //}

        //Array.Sort(fieldNames, (string a, string b) => a.CompareTo(b));
        //// Assuming the first field identifies the object class
        //foreach (string line in lines.Skip(3))
        //{
        //    string[] fields = line.Split('\t');
        //    Monster obj = ScriptableObject.CreateInstance<Monster>();
        //    obj.monster_id = fields[0];
        //    obj.monster_name = fields[1];
        //    obj.monster_model_id = fields[2];
        //    obj.monster_HP = (float)ParseValue(fields[3]);
        //    obj.monster_HP_up = (float)ParseValue(fields[4]);
        //    obj.monster_AP = (float)ParseValue(fields[5]);
        //    obj.monster_AP_up = (float)ParseValue(fields[6]);
        //    obj.monster_DP = (float)ParseValue(fields[7]);
        //    obj.monster_DP_up = (float)ParseValue(fields[8]);
        //    obj.monster_MAP = (float)ParseValue(fields[9]);
        //    obj.monster_MAP_up = (float)ParseValue(fields[10]);
        //    obj.monster_MDP = (float)ParseValue(fields[11]);
        //    obj.monster_MDP_up = (float)ParseValue(fields[12]);
        //    obj.monster_phys_resistance_rate = (float)ParseValue(fields[13]);
        //    obj.monster_mag_resistance_rate = (float)ParseValue(fields[14]);
        //    AssetDatabase.CreateAsset(obj, "Assets/Resources/MonstersSO/" + obj.monster_id + ".asset");
        //}

    }

}
