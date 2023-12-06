using UnityEngine;
using BehaviorDesigner.Runtime;
using BehaviorDesigner.Runtime.Tasks;

public class WithinSight : Conditional
{
    // How wide of an angle the object can see
    public float fieldOfViewAngle;
    // The tag of the targets
    public string targetTag;
    // Set the target variable when a target has been found so the subsequent tasks know which object is the target
    private SharedTransform target;
    //
    public float insightRange = 1;

    // A cache of all of the possible targets
    private Transform[] possibleTargets;
    private bool isFirstTime = true;

    public override void OnAwake()
    {
        // Cache all of the transforms that have a tag of targetTag
        target = MainCharacterController.instance.transform;
    }

    public override TaskStatus OnUpdate()
    {
        if (WithinSightCharacter(MainCharacterController.instance.transform, fieldOfViewAngle))
        {
            transform.GetComponent<EnemyController>().SetStateText("WithinSight");
            // Set the target so other tasks will know which transform is within sight
            target.Value = MainCharacterController.instance.transform;
            return TaskStatus.Success;

        }
        return TaskStatus.Failure;
    }

    // Returns true if targetTransform is within sight of current transform
    public bool WithinSightCharacter(Transform targetTransform, float fieldOfViewAngle)
    {
        Vector3 direction = targetTransform.position - transform.position;
        // An object is within sight if the angle is less than field of view
        return Vector3.Angle(direction, transform.forward) < fieldOfViewAngle;
    }
}