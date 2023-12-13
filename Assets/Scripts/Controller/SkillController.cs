using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using static Utils;

public class SkillController : MonoBehaviour
{

    public SkillType type;
    public SkillSO skillStat;
    protected bool m_isTriggered;
    private bool m_isCooldowning;
    protected float m_triggerTime;
    protected float m_cooldownTime;

    public UnityEvent OnTriggerEnter;
    public UnityEvent OnSkillUpdate;
    public UnityEvent OnCoolDown;
    public UnityEvent OnStop;
    public UnityEvent<SkillType, int> OnStopWithType;
    public UnityEvent OnCooldownStop;

    public float coolDownProgress
    {
        get => m_cooldownTime / skillStat.cooldownTime;
    }

    public float triggerProgress
    {
        get => m_triggerTime / skillStat.timeTrigger;
    }

    public bool IsTriggered { get => m_isTriggered; }
    public bool IsCooldowning { get => m_isCooldowning; }
    public float CooldownTime { get => m_cooldownTime; }

    public virtual void LoadStat()
    {
        if (skillStat == null) return;
        m_cooldownTime = skillStat.cooldownTime;
        m_triggerTime = skillStat.timeTrigger;
    }

    public void Trigger()
    {
        if (m_isTriggered || m_isCooldowning) return;
        m_isTriggered = true;
        m_isCooldowning = true;

        OnTriggerEnter?.Invoke();
    }

    private void Update()
    {
        CoreHandle();
    }

    private void CoreHandle()
    {
        ReduceTriggerTime();
        ReduceCooldownTime();
    }

    private void ReduceTriggerTime()
    {
        if (!m_isTriggered) return;
        m_triggerTime -= Time.deltaTime;
        if(m_triggerTime <= 0)
        {
            Stop();
        }
        OnSkillUpdate?.Invoke();
    }

    public void Stop()
    {
        m_triggerTime = skillStat.timeTrigger;
        m_isTriggered = false;

        OnStopWithType?.Invoke(type, 1);
        OnStop?.Invoke();
    }

    public void ForceStop()
    {
        m_isTriggered = true;
        m_isCooldowning = false;
        LoadStat();
    }

    private void ReduceCooldownTime()
    {
        if (!m_isCooldowning) return;
        m_cooldownTime -= Time.deltaTime;
        OnCoolDown?.Invoke();
        if (m_cooldownTime > 0) return;
        m_isCooldowning = false;
        OnCooldownStop?.Invoke();
        m_cooldownTime = skillStat.cooldownTime;
    }
}
