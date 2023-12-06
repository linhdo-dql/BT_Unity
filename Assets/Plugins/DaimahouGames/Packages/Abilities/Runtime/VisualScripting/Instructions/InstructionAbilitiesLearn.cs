using System;
using System.Threading.Tasks;
using DaimahouGames.Runtime.Core.Common;
using DaimahouGames.Runtime.Core;
using GameCreator.Runtime.Common;
using GameCreator.Runtime.VisualScripting;
using UnityEngine;

namespace DaimahouGames.Runtime.Abilities.VisualScripting
{
    [Version(1, 0, 0)]
    
    [Title("Learn Ability")]
    [Category("Abilities/Learn")]
    
    [Image(typeof(IconAbility), ColorTheme.Type.Blue)]
    [Description("Learn Ability")]

    [Parameter("Target", "The caster game object with a Pawn/Character component")]
    [Parameter("Ability", "The ability to be learned")]

    [Serializable]
    public abstract class InstructionAbilitiesLearn : Instruction
    {
        [SerializeField]
        private PropertyGetGameObject m_Caster = GetGameObjectPlayer.Create();
        
        [SerializeField]
        private Ability m_Ability;
        
        public override string Title => string.Format(
            "{0} Learns {1}",
            m_Caster,
            m_Ability != null ? TextUtils.Humanize(m_Ability.name) : "(none)" 
        );

        protected override Task Run(Args args)
        {
            var caster = m_Caster.Get<Caster>(args);
            //caster.Learn(m_Ability);
            return Task.CompletedTask;
        }
    }
}