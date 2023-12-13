using System.Collections;
using System.Collections.Generic;
using BehaviorDesigner.Runtime.Tasks;
using UnityEngine;

public class Retreat : Action
{
    public float speed;
    public float attackDistance = 0.75f;
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
        if(Mathf.Abs(targetPosition.x - transform.position.x) > attackDistance)
        {
            transform.localScale = new Vector3(transform.localScale.x * -1, 1, 1);
        }
        var newPos = isFaceRight ? targetPosition  + new Vector3(attackDistance, 0, 0) : targetPosition - new Vector3(attackDistance, 0 ,0);
        var mixPos = new Vector3(newPos.x, transform.position.y, newPos.z);
        if(transform.position == mixPos)
        {
            return TaskStatus.Success;
        }
        ec.SaveEnemyInfo();
        ec.SetStateText("Retreat");
        ec.SetActionText("Retreating...");
        transform.position = Vector3.MoveTowards(transform.position, mixPos, speed * Time.deltaTime);
        return TaskStatus.Running;
    }
}
