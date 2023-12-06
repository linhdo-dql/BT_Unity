using DaimahouGames.Core.Runtime.Common;
using DaimahouGames.Runtime.Core;
using DaimahouGames.Runtime.Pawns;
using GameCreator.Runtime.Common.UnityUI;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace DaimahouGames.Runtime.Abilities.UI
{
    [AddComponentMenu("Game Creator/UI/Abilities/Ability Slot")]
    [Icon(DaimahouPaths.GIZMOS + "GizmoAbility.png")]
    public class AbilitySlotUI : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerClickHandler
    {
        //============================================================================================================||
        // ※  Variables: --------------------------------------------------------------------------------------------|
        // ---| Exposed State -------------------------------------------------------------------------------------->|

        [SerializeField] private GameObject m_Highlight;
        [SerializeField] private TextReference m_ShortcutText;
        [SerializeField] private Image m_Icon;
        
        [SerializeField] private Controller m_Controller;

        [SerializeField] private bool m_OverrideSlot;
        [SerializeField] private int m_Slot = -1;

        // ---| Internal State ------------------------------------------------------------------------------------->|

        private Animation m_Animation;
        private MessageReceipt m_OnCast;
        private MessageReceipt m_OnPawnChanged;
        
        // ---| Dependencies --------------------------------------------------------------------------------------->|
        // ---| Properties ----------------------------------------------------------------------------------------->|

        private int Slot => m_OverrideSlot ? m_Slot : transform.GetSiblingIndex();
        
        // ---| Events --------------------------------------------------------------------------------------------->|
        // ※  Initialization Methods: -------------------------------------------------------------------------------|

        private void Start()
        {
            m_Animation = GetComponent<Animation>();
            m_ShortcutText.Text = m_Controller.GetInputProvider<IInputProviderAbility>().GetInputName(Slot);
            
            Refresh(m_Controller.GetPossessedPawn());
        }

        private void OnEnable()
        {
            m_OnPawnChanged = m_Controller.OnPawnChanged(Refresh);
        }

        private void OnDisable()
        {
            m_OnPawnChanged.Dispose();
            m_OnCast.Dispose();
        }

        // ※  Public Methods: ---------------------------------------------------------------------------------------|
        // ※  Virtual Methods: --------------------------------------------------------------------------------------|

        public void OnPointerEnter(PointerEventData eventData)
        {
            m_Highlight.SetActive(true);
        }

        public void OnPointerExit(PointerEventData eventData)
        {
            m_Highlight.SetActive(false);
        }
        
        public void OnPointerClick(PointerEventData eventData) => Perform();

        // ※  Private Methods: --------------------------------------------------------------------------------------|

        private void Refresh(Pawn pawn)
        {
            if (pawn == null) return;

            var caster = pawn.Get<Caster>();
            if (caster == null) return;

            m_OnCast.Dispose();
            m_OnCast = caster.OnCast(Animate);
            
            var ability = caster.GetSlottedAbility(Slot);
            if (ability == null) return;
            
            m_Icon.sprite = ability.Icon;
        }

        private void Animate(Ability ability)
        {
            var caster = m_Controller.GetPossessedPawn()?.Get<Caster>();
            if(ability == caster?.GetSlottedAbility(Slot)) m_Animation.Play();
        }
        
        private void Perform()
        {
            var caster = m_Controller.GetPossessedPawn()?.Get<Caster>();
            if (caster == null || !caster.CanCast(Slot)) return;
            
            m_Animation.Play();
            caster.StartCast(Slot);
        }
        
        //============================================================================================================||   
    }
}