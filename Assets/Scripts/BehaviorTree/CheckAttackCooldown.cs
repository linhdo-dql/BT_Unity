using System.Collections;
using System.Collections.Generic;
using BehaviorDesigner.Runtime;
using BehaviorDesigner.Runtime.Tasks;
using UnityEngine;

public class CheckAttackCooldown : Conditional
{
    public SharedInt attackNumber;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    public override TaskStatus OnUpdate()
    {
        var ec = transform.GetComponent<EnemyController>();
        if(attackNumber.Value == 1)
        {
            ec.Attack(1);
        }
        else
        {
            ec.Attack(2);
        }
        return (attackNumber.Value == 2 ? ec.isAttack2CooldownDone : ec.isAttack1CooldownDone ) ? TaskStatus.Failure : TaskStatus.Success;
    }
}
