using System.Collections;
using System.Collections.Generic;
using BehaviorDesigner.Runtime.Tasks;
using UnityEngine;

public class CheckInRange : Conditional
{
    private GameObject target;
    public float attackRange;
    // Start is called before the first frame update
    public void Start()
    {
        target = GameObject.FindGameObjectWithTag("Player");
    }

    // Update is called once per frame
    public override TaskStatus OnUpdate()
    {
        var distance = Mathf.Abs(transform.position.x - MainCharacterController.instance.gameObject.transform.position.x);
        return distance < attackRange ? TaskStatus.Success : TaskStatus.Failure;
    }
}
