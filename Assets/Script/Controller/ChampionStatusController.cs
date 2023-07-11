using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;
using Script;
using Random = System.Random;

public class ChampionStatusController : MonoBehaviourPun
{
    public ChampionModel championModel;
   // private DeckModel currentDeck;
    public ChampionState championState;
    public ChampionState prevChampionState;
    public MoveController moveController;
    public Animator animator;
    public ChampionBehavior championBehavior;
    public Text healthText;
    public Image healthBar;
    public Image healthBarBack;
    private Vector3 healthBarInitVector3;

    public Image skill1CoolTimeImage;
    public Image skill2CoolTimeImage;
    public Image skill3CoolTimeImage;
    
    public Color defaultSkillCoolTimeColor = new Color(0.2f, 0.2f, 0.2f, 0.5f);

    private IEnumerator easeOutQuintEnumerator;
    private IEnumerator easeOutCubicEnumerator;

    private IEnumerator stunEnumerator;
    private Vector3 oldLocalScale;
    public Transform target;
    private GameObject damageParticlePrefab;
    private GameObject damageTextPrefab;
    
    public bool stunned;

    private float particleOffsetX = 80.0f;
    private float particleOffsetY = 350.0f;
    private Canvas _canvas;
    // Start is called before the first frame update
    void Start()
    {
        Init();
    }

    public void RegisterChampionModel()
    {
        if (photonView.IsMine)
        {
            championModel = GameManager.instance.playerModel.selectChampionModel;
        }
        else
        {
            championModel = GameManager.instance.EnemyChampionModel;
            healthBar.color = new Color(254, 0, 255);
        }
        initHealth();
    }

    public void initHealth()
    {
        moveController.speed = 0.045f * championModel.movementSpeed;
        InitHealth();
        UpdateHealthText(); 
    }
    
    protected void Init()
    {
        _canvas = GameObject.Find("Canvas").GetComponent<Canvas>();
        damageParticlePrefab = Resources.Load("DamagedParticle") as GameObject;
        damageTextPrefab = Resources.Load("DamageText") as GameObject;
        
        oldLocalScale = transform.localScale;
        championBehavior = GetComponentInChildren<ChampionBehavior>();
        skill1CoolTimeImage = GameObject.Find("Skill1CoolTime").GetComponent<Image>(); 
        skill2CoolTimeImage = GameObject.Find("Skill2CoolTime").GetComponent<Image>(); 
        skill3CoolTimeImage = GameObject.Find("Skill3CoolTime").GetComponent<Image>();

        skill1CoolTimeImage.color = defaultSkillCoolTimeColor;
        skill2CoolTimeImage.color = defaultSkillCoolTimeColor;
        skill3CoolTimeImage.color = defaultSkillCoolTimeColor;

        if (photonView == null || GameManager.instance.isBot)
            return;
        
        if (PhotonNetwork.IsConnected && photonView.IsMine)
        {
            Debug.Log("myController register");
            PlayerManagerMulti.Instance.me = gameObject;
        }
        else if(PhotonNetwork.IsConnected)
        {
            Debug.Log("enemyController register");
            PlayerManagerMulti.Instance.enemy = gameObject;
        }
    }

    private void InitHealth()
    {
        healthBarInitVector3 = healthBar.transform.localPosition;
        championModel.currentHealth = championModel.health;
    }
    

    public void UpdateHealthText()
    {
        if (healthBar == null || healthBarBack == null || healthText == null)
            return;
        
        //healthBar.transform.localScale = new Vector3(championModel.hp / (float)championModel.GETMaxHealth(), 1, 1);
        //healthBar.transform.localScale = new Vector3(championModel.health / championModel.health, 1, 1);

        if (easeOutQuintEnumerator != null)
        {
            StopCoroutine(easeOutQuintEnumerator);
        }
        easeOutQuintEnumerator = EaseOutQuintLerp(1.0f, healthBar.transform, 
            healthBarInitVector3.x - (1 - championModel.currentHealth / championModel.health) * 425.0f);
        StartCoroutine(easeOutQuintEnumerator);

        
        if (easeOutCubicEnumerator != null)
        {
            StopCoroutine(easeOutCubicEnumerator);
        }
        easeOutCubicEnumerator = EaseOutCubicLerp(2.7f, healthBarBack.transform, 
            healthBarInitVector3.x - (1 - championModel.currentHealth / championModel.health) * 425.0f);
        StartCoroutine(easeOutCubicEnumerator);
        
        healthText.text = championModel.currentHealth.ToString();
    }

    public DamageModel newDamage(DamageModel damageModel, GameObject owner)
    {
        damageModel.owner = owner;
        return damageModel;
    }

    protected IEnumerator StartGetDamaged(bool isAlive, DamageModel damageModel, Action then)
    {
        championBehavior.GetDamaged();
        moveController.KnockBack(isAlive, damageModel, then);

        GameObject damageText = Instantiate(damageTextPrefab, new Vector3(transform.position.x+ UnityEngine.Random.Range(-0.5f, 0.5f), transform.position.y + 1.5f + UnityEngine.Random.Range(-0.2f, 0.3f), 0.0f), Quaternion.identity);
        int damageNum = Mathf.RoundToInt(damageModel.powerWeight);
        TextMesh damageTextMesh = damageText.GetComponent<TextMesh>();
        damageTextMesh.text = damageNum.ToString();
        if (championBehavior.isMine)
            damageTextMesh.color = new Color(1.0f, 0.0f, 0.47f);
        float damageTextScale = 0.37f + Mathf.Min(Math.Max(0, (damageNum - 3))*0.3f, 0.2f);
        damageText.transform.localScale = new Vector3(damageTextScale, damageTextScale, damageTextScale);
        
        int particleLen = UnityEngine.Random.Range(2, 5);
        for (int i = 0; i < particleLen; i++)
        {
            GameObject damageParticle = Instantiate(damageParticlePrefab, new Vector3(transform.position.x, transform.position.y + 0.5f, 0.0f), Quaternion.identity);
            float damagedScale = UnityEngine.Random.Range(0.05f, 0.1f);
            damageParticle.GetComponent<Rigidbody2D>().AddForce(new Vector2(UnityEngine.Random.Range(-1.0f, 1.0f)*particleOffsetX, UnityEngine.Random.Range(0.5f, 1.0f)* particleOffsetY));
            damageParticle.transform.localScale = new Vector3(damagedScale, damagedScale, damagedScale);
        }
        
        
        if (damageModel.stunDelayTime > 0.0f)
        {
            stunned = true;
            championBehavior.isMoveEnable = false;

            if (stunEnumerator != null)
                StopCoroutine(stunEnumerator);
            stunEnumerator = Stunned(damageModel.stunDelayTime);
            StartCoroutine(stunEnumerator);
        }
        else
        {
            if(!stunned)
                championState = ChampionState.IDLE;
        }
        yield break;
    }

    IEnumerator Stunned(float stunDelayTime)
    {
        transform.localScale = new Vector3(oldLocalScale.x * 1.2f, oldLocalScale.y * 0.75f, transform.localScale.z);
        yield return new WaitForSeconds(stunDelayTime);
        transform.localScale = oldLocalScale;
        stunned = false;
        championBehavior.isMoveEnable = true;
        championState = ChampionState.IDLE;
    }


    public void OnIdleState()
    {
        championState = ChampionState.IDLE;
        if (championBehavior.isMine || GameManager.instance.isBot)
        {
            championBehavior.isStun = false;
            championBehavior.isMoveEnable = true;
        }
    }
    
    public void OnRunState()
    {
        championState = ChampionState.RUNNING;
        if (championBehavior.isMine || GameManager.instance.isBot)
        {
            championBehavior.isStun = false;
            championBehavior.isMoveEnable = true;
        }
    }
    
    public void Attack1()
    {
        if (championState == ChampionState.RUNNING || championState == ChampionState.IDLE)
        {
            championBehavior.Attack1();
        }
    }

    public void Stun()
    {
        championBehavior.Stun();
    }
    
    private void Skill1On()
    {
        if (championState == ChampionState.RUNNING || championState == ChampionState.IDLE)
        {
            if (championBehavior.skill1CoolTime <= 0.0f)
            {
                championBehavior.Skill1On();
            }
        }
    }
    
    private void Skill1Off()
    {
       championBehavior.Skill1Off();
    }
    
    public void Skill1()
    {
        if (championState == ChampionState.RUNNING || championState == ChampionState.IDLE)
        {
            if (championBehavior.skill1CoolTime <= 0.0f)
            {
                championBehavior.skill1CoolTime = championModel.skills[0].cooldown;
                championBehavior.Skill1();
            }
        }
    }
    
    public void Skill2()
    {
        if (championState == ChampionState.RUNNING || championState == ChampionState.IDLE)
        {
            if (championBehavior.skill2CoolTime <= 0.0f)
            {
                championBehavior.skill2CoolTime = championModel.skills[1].cooldown;
                championBehavior.Skill2();
            }
        }
    }
    
    public void Skill3()
    {
        if (championState == ChampionState.RUNNING || championState == ChampionState.IDLE)
        {
            if (championBehavior.skill3CoolTime <= 0.0f)
            {
                championBehavior.skill3CoolTime = championModel.skills[2].cooldown;                                                                                                                                                                                                                                                                                                                                        
                championBehavior.Skill3();
            }
        }
    }
    

    [PunRPC]
    public void PlayAnim(string state)
    {
        animator.Play(state);
    }
    
    [PunRPC]
    public void RPCUpdateDamage(float currentHealth)
    {
        championModel.currentHealth = currentHealth;
        UpdateHealthText();
    }
    
    protected float UpdateDamage(DamageModel damageModel)
    {
        if (GameManager.instance.isBot || (!GameManager.instance.isBot && championBehavior.isMine))
        {
            if (championModel == null)
                return championModel.currentHealth;
        
            championModel.currentHealth -= damageModel.powerWeight;
            if (championModel.currentHealth <= 0)
                championModel.currentHealth = 0;
            
            if(!GameManager.instance.isBot)
                photonView.RPC("RPCUpdateDamage", RpcTarget.Others, championModel.currentHealth);
                
            UpdateHealthText();
        }
        return championModel.currentHealth;
    }

    public virtual void AddEvent(ChampionStatusEvent championStatusEvent)
    {
        Debug.Log("base.AddEvent() " + "/" + championStatusEvent + "-- currentState() " + championState + " -- moveEnable :" + championBehavior.isMoveEnable);

        if (!GameManager.instance.isBot && PhotonNetwork.IsConnected && photonView != null && !photonView.IsMine)
        {
            if (!(championStatusEvent is DamagedEvent))
            {
                return;
            }
        }
        

        if (championStatusEvent is DamagedEvent)
        {
            if (PlayerManagerMulti.Instance.gameSet)
                return;
            if (championBehavior.invincibility)
                return;
            
            championState = ChampionState.DAMAGED;
            DamageModel damageModel = (championStatusEvent as DamagedEvent).damageModel;
            bool isAlive = championModel.currentHealth > 0.0f;
            if (UpdateDamage(damageModel) <= 0.0f)
            {
                animator.Play("die");
                stunned = true;
                championBehavior.isMoveEnable = false;
                
                moveController.KnockBack(isAlive, damageModel, () =>
                {
                    if(isAlive)
                        championBehavior.Die();
                });
                return;
            };
            
            StartCoroutine(StartGetDamaged(isAlive, damageModel, () =>
            {
                if (damageModel is ThreshAttack3)
                {
                    StartCoroutine(championBehavior.JumpEnumerator(0.7f, 5.5f, false));
                }
            }));
        }

        if (championBehavior.isDie)
            return;
        
        if (championStatusEvent is RunningEvent)
        {
            OnRunState();
        } else if (championStatusEvent is IdleEvent)
        {
            OnIdleState();
        } else if (championStatusEvent is NormalAttackEvent)
        {
            Attack1();
        } else if (championStatusEvent is Skill1Event)
        {
            Skill1();
        } else if (championStatusEvent is Skill1OnEvent)
        {
            Skill1On();
        } else if (championStatusEvent is Skill1OffEvent)
        {
            Skill1Off();
        }else if (championStatusEvent is Skill2Event)
        {
            Skill2();
        } else if (championStatusEvent is Skill3Event)
        {
            Skill3();
        } else if (championStatusEvent is StunEvent)
        {
            championState = ChampionState.STUNNED;
            Stun();
        }
        
    }
    
    public static float EaseOutQuint(float start, float end, float value)
    {
        value--;
        end -= start;
        return end * (value * value * value * value * value + 1) + start;
    }
    
    public static float EaseOutCubic(float start, float end, float value)
    {
        value--;
        end -= start;
        return end * (value * value * value + 1) + start;
    }
    
    public static float EaseInQuart(float start, float end, float value)
    {
        end -= start;
        return end * value * value * value * value + start;
    }

    
    
    private IEnumerator EaseOutQuintLerp (float time, Transform target, float finalPosX)
    {
        float startingPosX  = target.localPosition.x;
        float elapsedTime = 0;
         
        while (elapsedTime < time)
        {
            target.localPosition = new Vector3(Mathf.Lerp(startingPosX, finalPosX, EaseOutQuint(0.0f, 1.0f, elapsedTime/time)),
                target.localPosition.y, target.localPosition.z);
            elapsedTime += Time.deltaTime;
            yield return null;
        }
    }
    
    private IEnumerator EaseOutCubicLerp (float time, Transform target, float finalPosX)
    {
        float startingPosX  = target.localPosition.x;
        float elapsedTime = 0;
         
        while (elapsedTime < time)
        {
            target.localPosition = new Vector3(Mathf.Lerp(startingPosX, finalPosX, EaseOutQuint(0.0f, 1.0f, elapsedTime/time)),
                target.localPosition.y, target.localPosition.z);
            elapsedTime += Time.deltaTime;
            yield return null;
        }
    }

    
}
