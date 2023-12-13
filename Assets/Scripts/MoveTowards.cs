using UnityEngine;
using BehaviorDesigner.Runtime;
using BehaviorDesigner.Runtime.Tasks;

public class MoveTowards : Action
{
    // The speed of the object
    public float speed = 0;
    // The transform that the object is moving towards
    public float distance;

    public override TaskStatus OnUpdate()
    {
        var ec = transform.GetComponent<EnemyController>();
        ec.SetStateText("MoveTowards");
        ec.SetActionText("Moving to the target...");
        // Return a task status of success once we've reached the target
        if (Mathf.Abs(transform.position.x - MainCharacterController.instance.transform.position.x) < distance)
        {
            ec.SetStateText("Attack");
            ec.SetActionText("Attacking...");
            return TaskStatus.Success;
        }
        // We haven't reached the target yet so keep moving towards it
        var targetPosition = MainCharacterController.instance.transform.position;
        var mixPosition = new Vector3(targetPosition.x, transform.position.y, targetPosition.z);
        transform.position = Vector3.MoveTowards(transform.position, mixPosition,  speed * Time.deltaTime);
        return TaskStatus.Running;
    }
}