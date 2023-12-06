using System.Collections;
using System.Collections.Generic;
using Csv;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public Monster dataMonsters;
    // Start is called before the first frame update
    void Start()
    {
        BetterStreamingAssets.Initialize();
        print(dataMonsters._dataMonster[20].monsterName);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
