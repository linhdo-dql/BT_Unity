using System.Collections;
using System.Collections.Generic;
using BehaviorDesigner.Runtime.Tasks;
using UnityEngine;

public class RetreatWaitAttackCooldown : Action
{
    public float speed;
    public int attackNumber;
    private EnemyController ec;
    private bool isFaceRight;

    public override void OnAwake()
    {
        base.OnAwake();
        ec = GetComponent<EnemyController>();
        
    }
    public override void OnStart()
    {

        isFaceRight = transform.localScale.x == 1;

    }
    // Update is called once per frame
    public override TaskStatus OnUpdate()
    {
        var targetPosition = MainCharacterController.instance.transform.position;
        if(isFaceRight && targetPosition.x > transform.position.x || !isFaceRight && targetPosition.x < transform.position.x)
        {
            isFaceRight = transform.localScale.x == -1;
        }
        transform.localScale = new Vector3(isFaceRight ? -1 : 1, 1, 1);
        var newPos = isFaceRight ? transform.position  + new Vector3(3, 0, 0) : transform.position - new Vector3(3, 0 ,0);
        var mixPos = new Vector3(newPos.x, transform.position.y, newPos.z);
        if(GetCooldownOfAttack(attackNumber))
        {
            return TaskStatus.Success;
        }
        ec.SaveEnemyInfo();
        ec.SetStateText("Retreat");
        ec.SetActionText("Retreating...");
        transform.position = Vector3.MoveTowards(transform.position, mixPos, speed * Time.deltaTime);
        return TaskStatus.Running;
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
}
