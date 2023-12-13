using System;
using System.Collections;
using System.Collections.Generic;
using BehaviorDesigner.Runtime;
using BehaviorDesigner.Runtime.Tasks;
using UnityEngine;

public class CheckAttackCooldown : Conditional
{
    public SharedInt attackNumber;
    private EnemyController ec;
    // Start is called before the first frame update
    public override void OnStart()
    {
        ec = transform.GetComponent<EnemyController>();
    }

    // Update is called once per frame
    public override TaskStatus OnUpdate()
    {
        SetAttack(attackNumber.Value);
        bool value = GetCooldownOfAttack(attackNumber.Value);
        return value ? TaskStatus.Failure : TaskStatus.Success;
    }

    private bool GetCooldownOfAttack(int value)
    {
        return value switch
        {
            1 => ec.isAttack1CooldownDone,
            2 => ec.isAttack2CooldownDone,
            3 => ec.isAttack3CooldownDone,
            4 => ec.isAttack4CooldownDone,
            5 => ec.isAttack5CooldownDone,
            _ => true,
        };
    }

    public void SetAttack(int attackNum)
    {
        ec.Attack(attackNum);
    }
}
