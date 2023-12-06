using System.Collections;
using System.Collections.Generic;
using BehaviorDesigner.Runtime;
using BehaviorDesigner.Runtime.Tasks;
using UnityEngine;

public class CountEnemiesOverlapsed : Action
{
    public BehaviorTree behaviorTree;
    private SharedTransform target;
    public float insightRange;
    // Start is called before the first frame update
    public override void OnStart()
    {
        target = GameObject.FindGameObjectWithTag("Enemy").transform;
    }

    // Update is called once per frame
    public override TaskStatus OnUpdate()
    {
       // CheckDistance();
        return TaskStatus.Running;
    }

    
}
