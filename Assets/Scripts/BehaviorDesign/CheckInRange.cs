using System.Collections;
using System.Collections.Generic;
using BehaviorDesigner.Runtime.Tasks;
using UnityEngine;
using static Utils;

public class CheckInRange : Conditional
{
    private GameObject target;
    public PropCompare prop = PropCompare.Less;
    public float attackRange;
    private EnemyController ec;
    // Start is called before the first frame update
    public override void OnStart()
    {
        target = GameObject.FindGameObjectWithTag("Player");
        ec = GetComponent<EnemyController>();
    }

    // Update is called once per frame
    public override TaskStatus OnUpdate()
    {
        var distance = Mathf.Abs(transform.position.x - MainCharacterController.instance.gameObject.transform.position.x);
        Debug.Log("["+ec.id+"] Distance: "+distance);
        bool value = prop == PropCompare.Less ? distance <= attackRange : distance >= attackRange;
        return value ? TaskStatus.Success : TaskStatus.Failure;
    }
}
