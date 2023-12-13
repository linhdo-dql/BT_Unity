using System.Collections;
using System.Collections.Generic;
using BehaviorDesigner.Runtime;
using BehaviorDesigner.Runtime.Tasks;
using UnityEngine;

public class FlipCharacter : Action
{
    public BehaviorTree behaviorTree;
    private SharedTransform target;
    public override void OnStart()
    {
        target = ((SharedGameObject) behaviorTree.GetVariable("theFirstEnemy")).Value.transform;
    }
    // Start is called before the first frame update
    public override TaskStatus OnUpdate()
    {
        Flip(target.Value);
        return TaskStatus.Success;
    }
    private void Flip(SharedTransform collision)
    {
        bool isFaceToFace =
            (transform.position.x > collision.Value.position.x && transform.localScale.x * collision.Value.localScale.x < 0)
            ||
            (transform.position.x < collision.Value.position.x && transform.localScale.x * collision.Value.localScale.x > 0);
        Debug.Log(isFaceToFace);
        if (!isFaceToFace)
        {
            transform.localScale = new Vector3(transform.localScale.x * -1, 1, 1);
        }
    }
}
