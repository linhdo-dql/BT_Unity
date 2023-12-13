using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System;
using UnityEngine.Events;

public class SkillButton : MonoBehaviour
{
	[SerializeField] private Image m_skillIcon;
	[SerializeField] private Image m_skillCooldownOverlay;
	[SerializeField] private Text m_amountTxt;
	[SerializeField] private Text m_coolDownTxt;
	[SerializeField] private Button m_btnComp;

	private Utils.SkillType m_skillType;
	private SkillController m_skillController;
	private int m_currentAmount;


	#region EVENTS
	private void RegisterEvents()
	{
		if (m_skillController == null) return;
		m_skillController.OnCoolDown.AddListener(UpdateCooldown);
		m_skillController.OnSkillUpdate.AddListener(UpdateTimeTrigger);
		m_skillController.OnCooldownStop.AddListener(UpdateUI);
	}

	private void UnregisterEvents()
	{
        if (m_skillController == null) return;
        m_skillController.OnCoolDown.RemoveListener(UpdateCooldown);
        m_skillController.OnSkillUpdate.RemoveListener(UpdateTimeTrigger);
        m_skillController.OnCooldownStop.RemoveListener(UpdateUI);
    }

	#endregion

	public void Initialize(Utils.SkillType type)
	{
		m_skillType = type;
		m_skillController = SkillManager.Ins.GetSkillController(type);
        UpdateUI();

		if(m_btnComp != null)
		{
			m_btnComp.onClick.RemoveAllListeners();
			m_btnComp.onClick.AddListener(TriggerSkill);
		}
		RegisterEvents();
	}

    private void TriggerSkill()
    {
		if (m_skillController == null) return;
		m_skillController.Trigger();
    }

    private void UpdateUI()
    {
		if (m_skillController == null) return;
		if(m_skillIcon)
		{
			m_skillIcon.sprite = m_skillController.skillStat.skillIcon;
		}

		UpdateAmountText();
		UpdateCooldown();
		UpdateTimeTrigger();

		bool canAtiveMe = m_currentAmount > 0 || !m_skillController.IsCooldowning;
		gameObject.SetActive(canAtiveMe);
    }

    private void UpdateTimeTrigger()
    {
		if (m_skillController == null) return;
    }

    private void UpdateCooldown()
    {
        if (m_coolDownTxt)
        {
            m_coolDownTxt.text =m_skillController.CooldownTime.ToString("f1");
			float cooldownProgress = m_skillController.coolDownProgress;
			if(m_skillCooldownOverlay)
			{
				m_skillCooldownOverlay.fillAmount = cooldownProgress;
				m_skillCooldownOverlay.gameObject.SetActive(m_skillController.IsCooldowning);
			}
        }
    }

    private void UpdateAmountText()
    {
		m_currentAmount = SkillManager.Ins.GetSkillAmount(m_skillType);
		if(m_amountTxt)
		{
			m_amountTxt.text = $"x{m_currentAmount}";
		}
    }

    private void OnDestroy()
    {
		UnregisterEvents();
    }
}