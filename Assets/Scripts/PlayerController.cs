using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using static AbilityHolder;

public class PlayerController : MonoBehaviour
{

    List<string> abilities = new List<string>() { "Dash", "SwordBlade" };
    // Start is called before the first frame update
    void Start()
    {
        InitSkill();
    }

    private void InitSkill()
    {
        foreach(var ability in abilities)
        {
            var ah = gameObject.AddComponent<AbilityHolder>();
            ah.ability = Resources.Load("Abilities/" + ability) as Ability;
            var skillBlock = GameObject.Find("SkillBlock");
            var skillButtonPrefab = Resources.Load("Prefabs/SkillButton");
            GameObject skillButton = Instantiate(skillButtonPrefab, skillBlock.transform) as GameObject;
            skillButton.GetComponent<SkillButtonController>().Populate(ah.ability);
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
