using System;
using Sirenix.OdinInspector;
using UnityEngine;
namespace Csv
{
    [Serializable]
    public class Monster : SerializedScriptableObject
    {
        public MonsterItem[] _dataMonster;
        [Serializable]
        public class MonsterItem
        {
            public string monsterId;
            public string monsterName;
            public string monsterModelId;
            public float monsterHp;
            public float monsterHpUp;
            public float monsterAp;
            public float monsterApUp;
            public float monsterDp;
            public float monsterDpUp;
            public float monsterMap;
            public float monsterMapUp;
            public float monsterMdp;
            public float monsterMdpUp;
            public float monsterPhysResistanceRate;
            public float monsterMagResistanceRate;
        }
    }
}