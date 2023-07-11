using System;
using System.Collections;
using System.Linq;
using Pathfinding;
using Script;
using UnityEngine;
using UnityEngine.UI;
using Random = UnityEngine.Random;

public class MobStatusController : ChampionStatusController
{
    private Seeker seeker;
    private Vector2 targetPos;
    public MobMoveController mobMoveController;

    public MobAIDistanceState mobAIDistance;
    public MobAIChaseState mobAIChaseState;
    public MobAIRunAwayState mobAIRunAwayState;
    public MobAINoneState mobAINoneState;
    public IEnumerator delayedAndActionEnumerator;
    public IEnumerator repeatUpdatePathEnumerator;

    private MobAIState currentState;
    public Text mobStatusText;

    public Transform getTarget()
    {
        return target;
    }
    void Start()
    {
        Init();
        mobMoveController = GetComponent<MobMoveController>();
        animator = transform.GetChild(0).GetComponentInChildren<Animator>();
        championBehavior = GetComponentInChildren<ChampionBehavior>();
        seeker = GetComponent<Seeker>();
        
        // if(Debug.isDebugBuild)
            // mobStatusText = GameObject.Find("MobStatus").GetComponent<Text>();
        
        stateInit();
        Debug.Log("noneState : " + mobAINoneState);
        updateNewState(mobAINoneState);
    }

    protected virtual void stateInit()
    {
        mobAINoneState = new MobAINoneState(this);
        mobAIChaseState = new MobAIChaseState(this);
        mobAIRunAwayState = new MobAIRunAwayState(this);
        mobAIDistance = new MobAIDistanceState(this);
    }

    // Update is called once per frame
    void Update()
    {
        if (currentState != null)
        {
           // Debug.Log(currentState);
            currentState.UpdateState();
        }

        if (championBehavior.normalAttackCoolTime > 0.0f)
        {
            championBehavior.normalAttackCoolTime -= Time.deltaTime;
        }
        
        
        UpdateMobState();
    }

    public void StartDelayedAndAction(float delayedSec, Action action)
    {
        delayedAndActionEnumerator = DelayedAndAction(delayedSec, action);
        StartCoroutine(delayedAndActionEnumerator);
    }
    
    public void StopAllCoroutine()
    {
         if(delayedAndActionEnumerator != null)
             StopCoroutine(delayedAndActionEnumerator);
         if(repeatUpdatePathEnumerator != null)
             StopCoroutine(repeatUpdatePathEnumerator);
    }

    public Vector3 getDistancePoint()
    {
        float dis = Random.Range(3.0f, 6.0f);
        float rad = Random.Range(0, Mathf.PI*2);
        return target.position + new Vector3(dis * Mathf.Sin(rad), dis * Mathf.Cos(rad), 0);
    }
    
    public void UpdatePath(Vector2 targetPos)
    {
        // if(seeker.IsDone())
        seeker.StartPath(transform.position, targetPos, mobMoveController.OnPathComplete);
    }
    public IEnumerator RepeatUpdatePath(Vector3 target)
    {
        while (true)
        {
         //   Debug.Log("RepeadUpdatePath!");
            UpdatePath(target);
            yield return new WaitForSeconds(0.7f);
        }
    }
    public IEnumerator DelayedAndAction(float delayedSec, Action action)
    {
        yield return new WaitForSeconds(delayedSec);
        action();
    }
    public void StartRepeatUpdatePath(Vector3 target)
    {
        repeatUpdatePathEnumerator = RepeatUpdatePath(target);
        StartCoroutine(repeatUpdatePathEnumerator);
    }
    public void updateNewState(MobAIState newState)
    {
        if(currentState != null)
            currentState.ExitState();
        newState.EnterState();
        Debug.Log("NEWSTATE : " + newState);
        // if(Debug.isDebugBuild)
            // mobStatusText.text = newState.ToString().Split('.').Last() + "/" + championBehavior.isMoveEnable;
        currentState = newState;
    }

    public virtual void UpdateMobState()
    {
        switch (championState)
        {
            case ChampionState.IDLE:
                if (prevChampionState != ChampionState.IDLE)
                {
                    animator.SetFloat(Constants.moveMagnitude, 0.0f);
                }
                break;
            case ChampionState.ROLLING:
                if (prevChampionState != ChampionState.ROLLING)
                {
                    animator.SetFloat(Constants.moveMagnitude, 0.0f);
                }
                break;
            case ChampionState.ATTACK:
                {
                    animator.SetFloat(Constants.moveMagnitude, 0.0f);
                }
                break;
            case ChampionState.DAMAGED:
                animator.SetFloat(Constants.moveMagnitude, 0.0f);
                break;
            case ChampionState.RUNNING:
                if (prevChampionState != ChampionState.RUNNING)
                {
                    // animator.SetFloat(Constants.moveMagnitude, 1.0f);
                }
                break;
        }
        prevChampionState = championState;
    }
    
    public override void AddEvent(ChampionStatusEvent championStatusEvent)
    {
        if (championStatusEvent is DamagedEvent)
        {
            if (PlayerManagerMulti.Instance.gameSet)
                return;
            if (championBehavior.invincibility)
                return;
            
            DamageModel damageModel = (championStatusEvent as DamagedEvent).damageModel;
            championState = ChampionState.DAMAGED;
            updateNewState(mobAINoneState);
            bool isAlive = championModel.currentHealth > 0.0f;

            if (UpdateDamage(damageModel) <= 0.0f)
            {
                animator.Play("die");
                if (championBehavior.isMine || GameManager.instance.isBot)
                {
                    stunned = true;
                    championBehavior.isMoveEnable = false;
                }
                moveController.KnockBack(isAlive, damageModel, () =>
                {
                    if(isAlive)
                        championBehavior.Die();
                });
                
                
                return;
            };

            StartCoroutine(StartGetDamaged(isAlive, damageModel,() =>
            {
                if (Random.Range(0, 10) > 2)
                {
                    updateNewState(mobAIDistance);
                }
                else
                {
                    updateNewState(mobAINoneState);
                }
                if (damageModel is ThreshAttack3)
                {
                    StartCoroutine(championBehavior.JumpEnumerator(0.7f, 5.5f, false));
                }
            }));
        }else if (championStatusEvent is AttackEvent)
        {
            NormalAttackTrigger();
        }else if (championStatusEvent is Skill1Event)
        {
            Skill1Trigger();
        }else if (championStatusEvent is Skill2Event)
        {
            Skill2Trigger();
        }else if (championStatusEvent is Skill3Event)
        {
            Skill3Trigger();
        }else if (championStatusEvent is RunningEvent)
        {
            if(championState != ChampionState.ATTACK)
                OnRunState();
            RunTrigger();
        }else if (championStatusEvent is IdleEvent)
        {
            if(championState != ChampionState.ATTACK)
                OnIdleState();
            IdleTrigger();
        }else if (championStatusEvent is StunEvent)
        {
            championState = ChampionState.STUNNED;
            Stun();
        }
    }

    void RunTrigger()
    {
        championBehavior.RunTrigger();
    }
    
    void IdleTrigger()
    {
        championBehavior.IdleTrigger();
    }
    
    protected virtual void NormalAttackTrigger()
    {
        Attack1();
    } 
    
    protected virtual void Skill1Trigger()
    {
        Skill1();
    } 
    protected virtual void Skill2Trigger()
    {
        Skill2();
    } 
    protected virtual void Skill3Trigger()
    {
        Skill3();
    } 
   
}
