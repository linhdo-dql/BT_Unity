using System;
using System.Threading.Tasks;
using DaimahouGames.Runtime.Abilities;
using DaimahouGames.Runtime.Characters;
using DaimahouGames.Runtime.Core;
using DaimahouGames.Runtime.Core.Common;
using GameCreator.Runtime.Characters;
using GameCreator.Runtime.Common;
using UnityEngine;
using State = GameCreator.Runtime.Characters.State;
using Target = DaimahouGames.Runtime.Core.Common.Target;

namespace DaimahouGames.Abilities.Runtime.Activators
{
    [Category("Activation: Channeled")]
    
    [Serializable]
    public class AbilityActivatorChanneled : AbilityActivator
    {
        //============================================================================================================||
        // ※  Variables: --------------------------------------------------------------------------------------------|
        // ---| Exposed State -------------------------------------------------------------------------------------->|
        
        [SerializeField] private ReactiveState m_GestureState;

        [SerializeField] private PropertyGetDecimal m_TickPerSecond = new(6);
        [SerializeField] private float m_Delay;
        
        // ---| Internal State ------------------------------------------------------------------------------------->|
        // ---| Dependencies --------------------------------------------------------------------------------------->|
        // ---| Properties ----------------------------------------------------------------------------------------->|

        public override string Title => $"Channeled {(m_GestureState ? m_GestureState.name : "(none)")}";

        // ---| Events --------------------------------------------------------------------------------------------->|
        // ※  Initialization Methods: -------------------------------------------------------------------------------|
        // ※  Public Methods: ---------------------------------------------------------------------------------------|
        
        public override async Task Activate(ExtendedArgs args)
        {
            var ability = args.Get<RuntimeAbility>();
            var caster = ability.Caster;

            _ = caster.Get<Character>().PlayGestureState(m_GestureState, args);
            
            var inputTask = ability.Targeting.ProcessInput(args);

            var startTime = Time.time;
            var entryDuration = m_GestureState.EntryClip ? m_GestureState.EntryClip.length : 0;
            
            await Awaiters.Until(() =>
            {
                UpdateFacing(args);
                var elapsedTime = Time.time - startTime;
                return elapsedTime > entryDuration && elapsedTime > m_Delay;
            });

            var receipt = caster.Pawn.Message.Subscribe<MessageAbilityActivation>(_ => ability.OnTrigger.Send(args));
            {
                var tickPerSecond = (float) m_TickPerSecond.Get(args);
                var tickTime = tickPerSecond;
                
                ability.CommitRequirements(args);
                
                await Awaiters.Until(() =>
                {
                    tickTime += Time.deltaTime;
                    if (tickPerSecond > 0 && tickTime > 1 / tickPerSecond)
                    {
                        tickTime = 0;
                        ability.OnTrigger.Send(args);
                    }
                    
                    UpdateFacing(args);
                    return inputTask.IsCompleted;
                });
            }
            receipt.Dispose();
            ability.Caster.Get<Character>().StopState(args);

            if (FaceTarget) ability.Caster.Get<Character>()?.StopFacingLocation();
        }
        
        // ※  Virtual Methods: --------------------------------------------------------------------------------------|
        // ※  Private Methods: --------------------------------------------------------------------------------------|

        private void UpdateFacing(ExtendedArgs args)
        {
            var ability = args.Get<RuntimeAbility>();
            if (FaceTarget && args.Has<Target>())
            {
                ability.Caster.Get<Character>().FaceLocation(args.Get<Target>().GetLocation());
            }
        }
        
        //============================================================================================================||
    }
}