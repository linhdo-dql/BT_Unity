using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using DaimahouGames.Runtime.Core;
using DaimahouGames.Runtime.Core.Common;
using DaimahouGames.Runtime.Pawns;
using GameCreator.Runtime.Common;
using UnityEngine;

namespace DaimahouGames.Runtime.Abilities
{
    [Category("Abilities/Caster")]
    
    [Image(typeof(IconAbility), ColorTheme.Type.Blue)]
    
    [Serializable]
    public sealed class Caster : Feature
    {
        //============================================================================================================||

        [Serializable]
        public class KnownAbility : IGenericItem
        {
            [SerializeField] private Ability m_Ability;
            public Ability Ability => m_Ability;
            
            #region EditorInfo
#if UNITY_EDITOR
            [SerializeField] private bool m_IsExpanded;
            public virtual string Title => m_Ability ? m_Ability.name : "(none)";
            public virtual Color Color => ColorTheme.Get(ColorTheme.Type.TextNormal);
            public bool IsExpanded { get => m_IsExpanded; set => m_IsExpanded = value; }
            public virtual string[] Info { get; } = Array.Empty<string>();
#endif
            #endregion
        }
        
        // ※  Variables: --------------------------------------------------------------------------------------------|
        // ---| Exposed State -------------------------------------------------------------------------------------->|
        
        [SerializeReference] private List<KnownAbility> m_AbilitySlots;
        
        // ---| Internal State ------------------------------------------------------------------------------------->|
        
        private readonly Dictionary<int, RuntimeAbility> m_Abilities = new();
        private CastState m_CastState;
        private Message<Ability> m_CastAbilityMessage;

        // ---| Dependencies --------------------------------------------------------------------------------------->|
        // ---| Properties ----------------------------------------------------------------------------------------->|

        public override string Title => "Caster";
        
        // ---| Events --------------------------------------------------------------------------------------------->|

        private Message<Ability> CastAbilityMessage => m_CastAbilityMessage ??= new Message<Ability>();
        public MessageReceipt OnCast(Action<Ability> onCast) => CastAbilityMessage.Subscribe(onCast);
        
        // ※  Initialization Methods: -------------------------------------------------------------------------------|

        protected override void Start()
        {
            m_CastState = Pawn.GetState<CastState>();
        }

        // ※  Public Methods: ---------------------------------------------------------------------------------------|

        public T TryGet<T>() where T : Feature => Pawn.TryGetFeature(out T feature) ? feature : null;
        public new T Get<T>() where T : Feature => Pawn.Get<T>();

        public Task<bool> Cast(Ability ability) => Cast(ability, new ExtendedArgs(GameObject));
        
        public async Task<bool> Cast(Ability ability, ExtendedArgs args)
        {
            if (!CanCancel()) return false;

            CastAbilityMessage.Send(ability);
            
            args.ChangeSelf(GameObject);
            args.Set(GetRuntimeAbility(ability));

            var success = m_CastState.TryEnter(args);
            await m_CastState.WaitUntilComplete();
            return success;
        }
        
        public void StartCast(int slot)
        {
            if (!IsValidSlot(slot)) return;
            Cast(m_AbilitySlots[slot].Ability);
        }

        public void EndCast(int slot)
        {
            if (!IsValidSlot(slot)) return;
            GetRuntimeAbility(m_AbilitySlots[slot].Ability).EndCast();
        }

        public bool CanCancel()
        {
            return !m_CastState.IsActive || !m_CastState.CanExit(m_CastState);
        }
        
        public bool CanCast(int slot)
        {
            if (!IsValidSlot(slot)) return false;
            var runtimeAbility = GetRuntimeAbility(m_AbilitySlots[slot].Ability);

            var args = new ExtendedArgs(GameObject);
            args.Set(runtimeAbility);
            
            return runtimeAbility.CanUse(args, out _);
        }
        
        public Ability GetSlottedAbility(int slot)
        {
            if (IsValidSlot(slot)) return m_AbilitySlots[slot].Ability;
            return default;
        }

        // ※  Virtual Methods: --------------------------------------------------------------------------------------|
        // ※  Private Methods: --------------------------------------------------------------------------------------|

        private bool IsValidSlot(int slot)
        {
            return slot >= 0 && slot < m_AbilitySlots.Count;
        }

        private RuntimeAbility GetRuntimeAbility(Ability ability)
        {
            if (ability == null) return default;
            
            var hashKey = ability.ID.Hash;
            return m_Abilities.ContainsKey(hashKey)
                ? m_Abilities[hashKey]
                : m_Abilities[hashKey] = new RuntimeAbility(this, ability);
        }

        //============================================================================================================||
    }
}