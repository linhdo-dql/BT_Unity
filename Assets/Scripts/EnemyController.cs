using System;
using System.Collections;
using BehaviorDesigner.Runtime;
using TMPro;
using UnityEngine;
public class EnemyController : MonoBehaviour
{
    private BehaviorTree tree;
    public string id;
    public GameObject enemyInfo;
    public bool isAttack1CooldownDone;
    public bool isAttack2CooldownDone;
    public bool isAttack3CooldownDone;
    public bool isAttack4CooldownDone;
    public bool isAttack5CooldownDone;
    public float minRangeAttack = 0;
    public float maxHP;
    [Range(0.0f, 100f)]
    public float currentHP;
    public GameObject atk01;
    public GameObject atk02;
    public GameObject atk03;
    public GameObject atk04;
    public GameObject atk05;
    public TextMeshPro stateText;
    public TextMeshPro actionText;
    public TextMeshPro attackText;
    public TextMeshPro atk01Text;
    public TextMeshPro atk02Text;
    public TextMeshPro atk03Text;
    public TextMeshPro atk04Text;
    public TextMeshPro atk05Text;
    private bool isReseted;

    private void Awake()
    {
        tree = GetComponent<BehaviorTree>();
    }

    // Start is called before the first frame update
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
           // StartCoroutine(ResetBT(0));
        }
    }
    private void OnTriggerStay2D(Collider2D collision)
    {
        var isLeft = transform.position.x < MainCharacterController.instance.transform.position.x;
        transform.localScale = new Vector3(isLeft ? -1 : 1, 1, 1);
        SaveEnemyInfo();
    }
    private IEnumerator ResetBT(float s)
    {
        yield return new WaitForSeconds(s);
        tree.enabled = false;
        tree.enabled = true;
    }

    public void SaveEnemyInfo()
    {
        enemyInfo.transform.localScale = transform.localScale;
    }
    private void Update()
    {
        //var distance = Mathf.Abs(transform.position.x - MainCharacterController.instance.transform.position.x);
        //if (distance > minRangeAttack)
        //{
        //    StartCoroutine(ResetBT(1));
        //}
    }

    public void SetStateText(string v)
    {
        stateText.text = "Node: "+v;
    }

    public void SetActionText(string v)
    {
        actionText.text = "<b>Action:</b> <size=0.85><i>"+v;
    }

    public void SetAttackText(float range, float cooldownTime)
    {
        attackText.text = "Range:" + range +","+"cooldown:"+ cooldownTime;
    }

    internal void Attack(int value)
    {
        atk01.SetActive(false);
        if(atk02!=null)
        {
            atk02.SetActive(false);
        }
        if(atk03!=null)
        {
            atk03.SetActive(false);
        }
        if(atk04!=null)
        {
            atk04.SetActive(false);
        }
        if(atk05!=null)
        {
            atk05.SetActive(false);
        }
        switch(value)
        {
            case 1: atk01.SetActive(true);
                    SetStateText("Attack1");
                    SetActionText(isAttack1CooldownDone ? "Attack1 Attacking..." : "Attack1 Cooldowning...=> Idle");
                    atk01Text.text = "Range: 0.3 \nIsCooldown =" + isAttack1CooldownDone;
                    break;
            case 2: atk02.SetActive(true);
                    SetStateText("Attack2");
                    SetActionText(isAttack2CooldownDone ? "Attack2 Attacking": "Attack2 Cooldowning... => Idle");
                    atk02Text.text = "Range: 0.75 \nIsCooldown =" + isAttack2CooldownDone;
                    break;
            case 3:
                    atk03.SetActive(true);
                    SetStateText("Attack3");
                    SetActionText(isAttack3CooldownDone ? "Attack3 Attacking..." : "Attack3 Cooldowning...=> Idle");
                    atk03Text.text = "Range: 1 \nIsCooldown =" + isAttack3CooldownDone;
                    break;
            case 4:
                    atk04.SetActive(true);
                    SetStateText("Attack4");
                    SetActionText(isAttack4CooldownDone ? "Attack4 Attacking" : "Attack4 Cooldowning... => Idle");
                    atk04Text.text = "Range: 1.25 \nIsCooldown =" + isAttack4CooldownDone;
                    break;
            case 5:
                    atk05.SetActive(true);
                    SetStateText("Attack5");
                    SetActionText(isAttack5CooldownDone ? "Attack5 Attacking..." : "Attack5 Cooldowning...=> Idle");
                    atk05Text.text = "Range: 1.5 \nIsCooldown =" + isAttack5CooldownDone;
                    break;
        }
    }

    public float CurrentPersentHP()
    {
        return (currentHP / maxHP) * 100;
    }
}
