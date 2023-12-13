using System.Collections;
using System.Collections.Generic;
using SRN.SkillSystem;
using UnityEngine;
using static Utils;

public class SkillSystemDrawer : MonoBehaviour
{
    [SerializeField] private Transform m_gridRoot;
    [SerializeField] private SkillButton m_skillButtonPrefab;
    private Dictionary<SkillType, int> m_skillCollecteds;

    public void DrawSkillButtons()
    {
        Helper.ClearChilds(m_gridRoot);
        m_skillCollecteds = SkillManager.Ins.SkillCollecteds;
        if (m_skillCollecteds == null || m_skillCollecteds.Count <= 0) return;
        foreach(var skillCollected in m_skillCollecteds)
        {
            var skillButtonClone = Instantiate(m_skillButtonPrefab);
            Helper.AssignToRoot(m_gridRoot, skillButtonClone.transform, Vector3.zero, Vector3.one);
            skillButtonClone.Initialize(skillCollected.Key);
        }
    }
}
