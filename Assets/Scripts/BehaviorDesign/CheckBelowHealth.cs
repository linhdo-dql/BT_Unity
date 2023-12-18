using BehaviorDesigner.Runtime.Tasks;
using Sisus.Debugging;

public class CheckBelowHealth : Conditional
{
    public Utils.ObjectType objectType;
    public Utils.PropCompare prop = Utils.PropCompare.Less;
    public float healthPoint;
    private EnemyController ec;
    public override void OnStart()
    {
        ec = GetComponent<EnemyController>();
    }
    public override TaskStatus OnUpdate()
    {
        Debug.Log("[" + ec.id + "] Current Health: " + GetCurrentHP());
        var val = prop == Utils.PropCompare.Less ? GetCurrentHP() > healthPoint : GetCurrentHP() < healthPoint;
        return val ? TaskStatus.Success : TaskStatus.Failure;
    }

    public float GetCurrentHP()
    {
        switch(objectType)
        {
            case Utils.ObjectType.Enemy: return GetComponent<EnemyController>().CurrentPersentHP();
            case Utils.ObjectType.Character: return GetComponent<MainCharacterController>().currentHP;
            default: return 0;
        }
    }
}
