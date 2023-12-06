using System;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class SkillButtonController : MonoBehaviour
{
    public Image image;
    public Image mask;
    public Text skillText;
    private Ability _ability;
    public void SkillClicked()
    {
        var player = GameObject.FindGameObjectWithTag("Player");
        AbilityHolder abilityHolder = player.GetComponents<AbilityHolder>().First(a => a.ability.name == _ability.name);
        if (abilityHolder.abilityState != AbilityHolder.AbilityState.ready) return;
        abilityHolder.Action();
        mask.enabled = true;
        LeanTween.value(_ability.coolDownTime, 0, _ability.coolDownTime + _ability.activeTime).setOnUpdate((float f) =>
        {
            skillText.text = string.Format("{0:0.0}", f);
            mask.fillAmount = f / _ability.coolDownTime;
        }).setOnComplete(() =>
        {
            skillText.text = abilityHolder.ability.name;
            mask.enabled = false;
        });
    }
    public void Populate(Ability ability)
    {
        _ability = ability;
        image.sprite = ability.sprite;
        skillText.text = ability.name;
        mask.enabled = false;
    }
}