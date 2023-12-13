using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class TestBehaviorTreeEnemy : MonoBehaviour
{
    public Slider hpSlider;
    public EnemyController enemyController;
    public Toggle atk1;
    public Toggle atk2;
    public Toggle enable;

    // Start is called before the first frame update
    void Start()
    {
        Initialize();
        hpSlider.onValueChanged.AddListener(ChangeHp);
        atk1.onValueChanged.AddListener(Attack1);
        atk2.onValueChanged.AddListener(Attack2);
        enable.onValueChanged.AddListener(EnableGo);
    }

    private void EnableGo(bool IsEnable)
    {
        enemyController.gameObject.SetActive(IsEnable);
    }

    private void Attack1(bool attack1IsCooldown)
    {
        enemyController.isAttack5CooldownDone = attack1IsCooldown;
    }

    private void Attack2(bool attack2IsCooldown)
    {
        enemyController.isAttack2CooldownDone = attack2IsCooldown;
    }

    private void ChangeHp(float hp)
    {
        enemyController.currentHP = hp;
    }

    private void Initialize()
    {
        hpSlider.value = enemyController.currentHP;
        hpSlider.maxValue = enemyController.maxHP;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
