using System.Collections;
using System.Collections.Generic;
using BehaviorDesigner.Runtime.Tasks;
using UnityEngine;

public class Retreat : Action
{
    public float speed;
    public float attack2Distance = 0.75f;
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
        transform.localScale = new Vector3(isFaceRight ? -1 : 1, 1, 1);
        var targetPosition = MainCharacterController.instance.transform.position;
        float newPosX = isFaceRight ? targetPosition.x + attack2Distance : targetPosition.x - attack2Distance;
        var mixPosition = new Vector3(newPosX, transform.position.y, targetPosition.z);
        Debug.Log(mixPosition);
        if (Mathf.Abs(transform.position.x - MainCharacterController.instance.transform.position.x) < attack2Distance)
        {
            ec.SaveEnemyInfo();
            ec.SetStateText("Retreat");
            ec.SetActionText("Retreating...");
            transform.position = Vector3.MoveTowards(transform.position, mixPosition, speed * Time.deltaTime);
            return TaskStatus.Running;
        }
        
        return TaskStatus.Success;
    }
}
