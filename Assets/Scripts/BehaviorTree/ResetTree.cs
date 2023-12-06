using System.Collections;
using System.Collections.Generic;
using BehaviorDesigner.Runtime;
using BehaviorDesigner.Runtime.Tasks;
using UnityEngine;

public class ResetTree : Action
{
    private BehaviorTree behaviourTree;
    public override void OnAwake()
    {
        behaviourTree = GetComponent<BehaviorTree>();
    }

    public override void OnStart()
    {
        behaviourTree.enabled = false;
        behaviourTree.enabled = true;
    }
    // Start is called before the first frame update
    public override TaskStatus OnUpdate()
    {
        
        return TaskStatus.Failure;
    }
}
