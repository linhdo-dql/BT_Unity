using System.Collections;
using System.Collections.Generic;
using BehaviorDesigner.Runtime.Tasks;
using UnityEngine;

public class CheckHaveAnotherEnemy : Conditional
{
    private GameObject target;
    private string enemyID;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    public override TaskStatus OnUpdate()
    {
        enemyID = GetComponent<EnemyController>().id;
        bool isHave = MainCharacterController.instance.enemies.Find(e => e.GetComponent<EnemyController>().id != enemyID) != null;
        return isHave ? TaskStatus.Failure : TaskStatus.Success;
    }
}
