using System.Collections;
using System.Collections.Generic;
using BehaviorDesigner.Runtime;
using BehaviorDesigner.Runtime.Tasks;
using UnityEngine;

public class OvalapseMainCharacter : Conditional
{
    public BehaviorTree behaviorTree;
    private SharedTransform target;
    public float insightRange;
    public SharedVariable isOverlapsedEnemy;
    private bool isTheFirst;

    // Start is called before the first frame update
    public override void OnStart()
    {
        target = GameObject.FindGameObjectWithTag("Enemy").transform;
        isTheFirst = true;
    }

    // Update is called once per frame
    public override TaskStatus OnUpdate()
    {
        isOverlapsedEnemy.SetValue(CheckDistance());
        return CheckDistance() ? TaskStatus.Success : TaskStatus.Running;
    }

    public bool CheckDistance()
    {   
        float distance = Vector3.Distance(transform.position, target.Value.position);
        if (distance < insightRange)
        {
            if(isTheFirst)
            {
                behaviorTree.GetVariable("theFirstEnemy").SetValue(target.Value.gameObject);
                isTheFirst = false;
            }
            return true; // Character is still within the insight range
        }
        return false; // Character is outside the insight range
    }
    

}
