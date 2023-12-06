using System;
using System.Threading.Tasks;
using DaimahouGames.Core.Runtime.VisualScripting;
using DaimahouGames.Runtime.Core.Common;
using DaimahouGames.Runtime.Core;
using GameCreator.Runtime.Common;
using GameCreator.Runtime.VisualScripting;
using UnityEngine;

namespace DaimahouGames.Runtime.Abilities.VisualScripting
{
    [Version(1, 0, 0)]
    
    [Title("Cast Ability")]
    [Category("Abilities/Cast")]
    
    [Image(typeof(IconAbility), ColorTheme.Type.Blue)]
    [Description("Instruct the Character to cast an ability")]

    [Parameter("Target Input", "The intended target location. Leave to none to have the ability " +
                               "process the input directly. Set to a value to skip the input phase.")]
    [Parameter("Ability", "The ability to be cast")]

    [Serializable]
    public class InstructionAbilitiesCast : Instruction
    {
        [SerializeField]
        private PropertyGetGameObject m_Caster = GetGameObjectPlayer.Create();

        [SerializeField] private PropertyArgTarget m_ManualTargetInput;

        [SerializeField]
        private Ability m_Ability;
        
        [SerializeField]
        private bool m_WaitToComplete;

        public override string Title => string.Format(
            "{0} Cast {1}",
            m_Caster,
            m_Ability != null ? TextUtils.Humanize(m_Ability.name) : "(none)" 
        );

        protected override Task Run(Args args)
        {
            m_ManualTargetInput.Set(ref args);
            
            var caster = m_Caster.Get<Caster>(args).RequiredOn(m_Caster.Get(args));
            var task = caster.Cast(m_Ability, ExtendedArgs.Upgrade(ref args));
            
            return m_WaitToComplete ? task : Task.CompletedTask;
        }
    }
}