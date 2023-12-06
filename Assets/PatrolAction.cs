using BehaviorDesigner.Runtime.Tasks;
using UnityEngine;

public class PatrolAction : Action
{
    public float patrolSpeed = 2f;
    public Transform[] patrolPoints;

    private int currentPatrolPointIndex = 0;

    public override void OnStart()
    {
        // Set the initial position to the first patrol point
        transform.position = patrolPoints[0].position;
    }

    public override TaskStatus OnUpdate()
    {
        // Move towards the current patrol point
        Vector3 targetPosition = patrolPoints[currentPatrolPointIndex].position;
        transform.position = Vector3.MoveTowards(transform.position, targetPosition, patrolSpeed * Time.deltaTime);

        // Check if the patrol point has been reached
        if (Vector3.Distance(transform.position, targetPosition) < 0.1f)
        {
            // Increment the patrol point index
            currentPatrolPointIndex = (currentPatrolPointIndex + 1) % patrolPoints.Length;

            // If the end of the patrol path has been reached, return Success
            if (currentPatrolPointIndex == 0)
            {
                return TaskStatus.Success;
            }
        }

        // If the patrol point has not been reached, return Running
        return TaskStatus.Running;
    }
}