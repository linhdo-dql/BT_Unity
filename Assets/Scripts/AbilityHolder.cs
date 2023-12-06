using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class AbilityHolder : MonoBehaviour
{
    public Ability ability;
    float cooldownTime;
    float activeTime;

    public enum AbilityState
    {
        ready, active, cooldown
    }

    public AbilityState abilityState = AbilityState.ready;



    // Update is called once per frame
    void Update()
    {
        switch (abilityState)
        {
            case AbilityState.ready:
                break;
            case AbilityState.active:
                if (activeTime > 0)
                {
                    activeTime -= Time.deltaTime;
                    Debug.Log(ability.name + " active time: " + activeTime);
                }
                else
                {
                    abilityState = AbilityState.cooldown;
                    cooldownTime = ability.coolDownTime;
                    Debug.Log("<color=#FF0000>" + ability.name + " active done</color>");
                }
                break;
            case AbilityState.cooldown:
                if (cooldownTime > 0)
                {
                    cooldownTime -= Time.deltaTime;
                    Debug.Log(ability.name + " cooldown time: " + cooldownTime);
                }
                else
                {
                    abilityState = AbilityState.ready;
                    Debug.Log("<color=#FF00FF>" + ability.name + " finish cooldown</color>");
                }
                break;
        }

    }

    internal void Action()
    {
        ability.Activate(gameObject);
        abilityState = AbilityState.active;
        activeTime = ability.activeTime;
        Debug.Log("<color=#FFFF00>" + ability.name + " READY!!!</color>");
    }
}