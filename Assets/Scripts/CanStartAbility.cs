using System.Linq;
using BehaviorDesigner.Runtime;
using BehaviorDesigner.Runtime.Tasks;
[TaskIcon("Assets/Behavior Designer Tutorials/Tasks/Editor/{SkinColor}SeekIcon.png")]

public class CanStartAbility : Conditional
{
    public SharedGameObject target;
    public SharedString abilityName;
    // Start is called before the first frame update
    public override void OnStart()
    {
        
    }

    // Update is called once per frame
    public override TaskStatus OnUpdate()
    {
        var abilityState = target.Value.GetComponents<AbilityHolder>().First(a => a.ability.name == abilityName.Value).abilityState;
        return abilityState == AbilityHolder.AbilityState.ready && abilityState != AbilityHolder.AbilityState.cooldown ? TaskStatus.Running : TaskStatus.Success;
    }
}
