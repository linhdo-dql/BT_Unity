using System.Collections;
using System.Collections.Generic;
using Csv;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public Dictionary<Utils.ObjectType, System.Type> controllers;
    public Monster dataMonsters;
    public static GameManager instance;
    private void Awake()
    {
        instance = this;
        controllers = new Dictionary<Utils.ObjectType, System.Type>()
        {
            {Utils.ObjectType.Enemy, typeof(EnemyController)},
            {Utils.ObjectType.Character, typeof(MainCharacterController)}
        };
    }
    // Start is called before the first frame update
    void Start()
    {
        BetterStreamingAssets.Initialize();
    }
    // Update is called once per frame
    void Update()
    {
        
    }


}
