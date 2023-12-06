using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu (menuName = "Abilities/SwordBladeAbility")]
public class SwordBladeAbility : Ability
{
    public int swordDamage;
    public float swordRange;
    public float hitForce;
    public Color swordHighlightColor;

    private SworBladeTriggerable rcSwordBlade;

    public override void Activate(GameObject obj)
    {
        obj.GetComponent<MainCharacterController>().SwordBlade();
        CameraController.instance.Shake();
    }

    public override void TriggerAbility()
    {
        rcSwordBlade.Blade();
    }
}
