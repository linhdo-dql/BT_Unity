using System.Collections;
using System.Collections.Generic;
using BehaviorDesigner.Runtime.Tasks;
using UnityEngine;

public class FaceDirectionTarget : Action
{
    // Update is called once per frame
    public override TaskStatus OnUpdate()
    {
        var enemyController = transform.GetComponent<EnemyController>();
        enemyController.SetStateText("FaceDirectionTarget");
        var isLeft = transform.position.x < MainCharacterController.instance.transform.position.x;
        transform.localScale = new Vector3(isLeft ? -1 : 1, 1, 1);
        Debug.Log("[" + enemyController.id + "] Faced");
        enemyController.SaveEnemyInfo();
        return TaskStatus.Success;
    }
}
