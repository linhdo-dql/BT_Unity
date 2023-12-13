using System.Collections;
using System.Collections.Generic;
using SRN;
using UnityEngine;

public class SceneController : Singleton<SceneController>
{
    [SerializeField] private SkillSystemDrawer skillSystemDrawer;
    // Start is called before the first frame update
    void Start()
    {
        SkillManager.Ins?.AddSkill(Utils.SkillType.Stun, 3);
        SkillManager.Ins?.AddSkill(Utils.SkillType.Haste, 2);
        SkillManager.Ins?.AddSkill(Utils.SkillType.Immune, 2);
        skillSystemDrawer.DrawSkillButtons();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
