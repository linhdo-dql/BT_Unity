using System;
using System.Collections;
using System.Collections.Generic;
using BehaviorDesigner.Runtime;
using UnityEngine;

public class MainCharacterController : MonoBehaviour
{
    public static MainCharacterController instance;
    public List<GameObject> enemies;
    public GameObject characterInfo;
    public bool isTheFirst;
    private Animator animator;
    private BehaviorTree bt;

    private void Awake()
    {
        instance = this;
        //InitSkill();
        bt = GetComponent<BehaviorDesigner.Runtime.BehaviorTree>();
        animator = GetComponent<Animator>();
        enemies = new List<GameObject>();
    }
    // Start is called before the first frame update
    void Start()
    {
        
    }

    List<string> abilities = new List<string>() { "Dash", "SwordBlade" };
    private float insightRange = 0.25f;
    internal bool isAbilityAction;

    // Start is called before the first frame update
    private void InitSkill()
    {
        foreach (var ability in abilities)
        {
            var ah = gameObject.AddComponent<AbilityHolder>();
            ah.ability = Resources.Load("Abilities/" + ability) as Ability;
            var skillBlock = GameObject.Find("SkillBlock");
            var skillButtonPrefab = Resources.Load("Prefabs/SkillButton");
            GameObject skillButton = Instantiate(skillButtonPrefab, skillBlock.transform) as GameObject;
            skillButton.GetComponent<SkillButtonController>().Populate(ah.ability);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Enemy"))
        {

            ResetBT();
            enemies.Add(collision.gameObject);
            
        }
    }

    private void Update()
    {
        characterInfo.transform.localScale = transform.localScale;
    }


    private void OnTriggerExit2D(Collider2D collision)
    {
        if(collision.CompareTag("Enemy"))
        {
            ResetBT();
            isTheFirst = true;

            if (enemies.Contains(collision.gameObject))
            {
                enemies.Remove(collision.gameObject);
            }
            animator.Play("p_Idle");
        }
    }

    private void OnTriggerStay(Collider other)
    {
       
    }
    internal void PushAll()
    {
        foreach(var e in enemies)
        {
            Push(e);
        }
    }

    private void Push(GameObject e)
    {
        isAbilityAction = true;
        StartCoroutine(FakeAddForceMotion(0.5f, e));
    }


    IEnumerator FakeAddForceMotion(float forceAmount, GameObject g)
    {
        yield return new WaitForSeconds(1f);
        var isFace = g.transform.position.x <= transform.position.x;
        var _rigidBody2D = g.GetComponent<Rigidbody2D>();
        float i = 0.01f;
        while (forceAmount > i)
        {
            _rigidBody2D.velocity = new Vector2(forceAmount / i * (isFace ? -1 : 1), _rigidBody2D.velocity.y); // !! For X axis positive force
            i = i + Time.deltaTime;
            yield return new WaitForEndOfFrame();
        }
        _rigidBody2D.velocity = Vector2.zero;
        isAbilityAction = false;
        ResetBT();
        yield return null;
    }

    
    private void ResetBT()
    {
        
      
    }

    internal void SwordBlade()
    {
        isAbilityAction = true;
        StartCoroutine(SwordBlades());
    }

    private IEnumerator SwordBlades()
    {
        foreach (var e in enemies)
        {
            Destroy(e);
        }
        yield return new WaitForSeconds(2f);
        bt.GetVariable("IsOverlapseEnemy").SetValue(false);
        isAbilityAction = false;
        ResetBT();
    }

    
}
