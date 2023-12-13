using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu(menuName = "Skill System")]
public class SkillSO : ScriptableObject
{
    public float timeTrigger;
    public float cooldownTime;
    public Sprite skillIcon;
    public AudioClip triggerSoundFx;
}
