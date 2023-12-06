using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu (menuName = "Abilities/DashAbility")]
public class DashAbility : Ability
{
    public float dashVelocity;

    public override void Activate(GameObject obj)
    {
        obj.GetComponent<MainCharacterController>().PushAll();
    }

    public override void TriggerAbility()
    {
        throw new System.NotImplementedException();
    }
}
