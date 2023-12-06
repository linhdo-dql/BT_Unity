using System.Collections;
using System.Collections.Generic;
using BehaviorDesigner.Runtime;
using BehaviorDesigner.Runtime.Tasks;
using UnityEngine;

public class Attack : Action
{
    public SharedInt attackNumber;
    public override TaskStatus OnUpdate()
    {
        transform.GetComponent<EnemyController>().Attack(attackNumber.Value);
        return TaskStatus.Success;
    }
}
