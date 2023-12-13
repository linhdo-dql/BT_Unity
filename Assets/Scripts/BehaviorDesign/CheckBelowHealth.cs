using BehaviorDesigner.Runtime.Tasks;

public class CheckBelowHealth : Conditional
{
    public Utils.ObjectType objectType;
    public Utils.PropCompare prop = Utils.PropCompare.Less;
    public float healthPoint;
    public override void OnStart()
    {
        
    }
    public override TaskStatus OnUpdate()
    {
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
