using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Ability : ScriptableObject
{
    public new string name;
    public Sprite sprite;
    public AudioClip audioClip;
    public float coolDownTime;
    public float activeTime;
    public float animatorClip;

    public abstract void Activate(GameObject obj);
    public abstract void TriggerAbility();
}
