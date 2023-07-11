using System;
using System.Collections;
using Photon.Pun;
using UnityEngine;

namespace Script.Champions.Thresh
{
    public class  ThreshBehaviour: ChampionBehavior
    {
        public GameObject attack1Collider;
        public Transform attack1Offset;
        private ThreshModel threshModel;
        public GameObject attack1Range;
        public GameObject attack1Effect;
        public GameObject skill1Effect;
        public bool skill1Catched = false;
        private Animator _animator;
        private GameObject lastSkill1Object;
        public GameObject skill2Effect;
        public GameObject skill1Range;
        public GameObject skill3Effect;
        

        // Start is called before the first frame update
        void Start()
        {
            threshModel = GameInfoManager.Instance.ChampMap[Constants.ThreshId] as ThreshModel;
            spriteRenderer = GetComponentInChildren<SpriteRenderer>();
            defaultShader = spriteRenderer.material.shader;
            _animator = GetComponentInChildren<Animator>();
        }

        // Update is called once per frame
        void Update()
        {
            if (isMine || GameManager.instance.isBot)
                coolTimeUpdate();
            
            if ((!GameManager.instance.isBot && isMine) || GameManager.instance.isBot)
            {
                if (skill1Catched)
                {
                    if (lastSkill1Object != null)
                    {
                        if (lastSkill1Object.transform.position.x > transform.parent.position.x)
                        {
                            transform.parent.Translate(Vector3.right * Time.deltaTime * 10.0f);
                        }
                        else
                        {
                            transform.parent.Translate(Vector3.left * Time.deltaTime * 10.0f);
                        }
                
                        if (Vector2.Distance(transform.parent.position, lastSkill1Object.transform.position) < 1.0f)
                        {
                            PhotonNetwork.Destroy(lastSkill1Object);
                            
                            championStatusController.target.gameObject.GetComponent<ChampionStatusController>().AddEvent(new IdleEvent());
                            if (GameManager.instance.isBot)
                                championStatusController.animator.Play("skill1_2");
                            else
                                championStatusController.photonView.RPC("PlayAnim", RpcTarget.All, "skill1_2");
                        }
                    }
                }
            }

        }

        public void Skill1CatchedFalse()
        {
            skill1Catched = false;
        }

        public override void Attack1()
        {
            championStatusController.championState = ChampionState.ATTACK;
            isMoveEnable = false;
            normalAttackCnt++;
            if (GameManager.instance.isBot)
                championStatusController.animator.Play("attack1");
            else
                championStatusController.photonView.RPC("PlayAnim", RpcTarget.All, "attack1");
        }
        public void Attack1Action()
        {
            DamageModel attack1Damage = GetComponentInParent<ChampionStatusController>().newDamage(threshModel.normalAttackDamageModel, gameObject);
            GameObject effect = Instantiate(attack1Effect, transform.position, Quaternion.identity);
            
            float scaleValue = 0.8f;
            if (transform.localScale.x > 0.0f)
            {
                effect.transform.localScale = new Vector3(-1*scaleValue, scaleValue, scaleValue);
            }
            else
            {
                effect.transform.localScale = new Vector3(scaleValue, scaleValue, scaleValue);
            }

            effect.GetComponentInChildren<AttackInfoScript>().damageModel = attack1Damage;
            Destroy(effect, 0.2f);
        }

        public override void Skill1()
        {
            Debug.Log("THRESH Skill1()");
            championStatusController.championState = ChampionState.ATTACK;
            isMoveEnable = false;
                
            if (GameManager.instance.isBot)
                championStatusController.animator.Play("skill1");
            else
                championStatusController.photonView.RPC("PlayAnim", RpcTarget.All, "skill1");
        }

        public void Skill1EffectOn()
        {
            if (!GameManager.instance.isBot && !isMine)
                return;
            
            GameObject effect = PhotonNetwork.Instantiate("ThreshSkill1", transform.position, Quaternion.identity);
            effect.GetComponent<ThreshSkill1EffectScript>().threshBehaviour = this;
            effect.GetComponent<ThreshSkill1EffectScript>().ownerChmapionBehavior = gameObject;
            lastSkill1Object = effect;
            //effect.transform.SetParent(transform);
            float scaleValue = 2f;
            if (transform.localScale.x > 0.0f)
            {
                effect.transform.localScale = new Vector3(-1*scaleValue, scaleValue, scaleValue);
            }
            else
            {
                effect.transform.localScale = new Vector3(scaleValue, scaleValue, scaleValue);
            }
        }

        public void Skill1Catch(GameObject playerOffset)
        {
            Debug.Log($"OnTriggerEnter2D THRESH Skill1Catch {playerOffset}");

            skill1Catched = true;
            if (!isMine && !GameManager.instance.isBot)
                return;
            
            playerOffset.GetComponent<ChampionStatusController>().AddEvent(new StunEvent());
            if (GameManager.instance.isBot)
                championStatusController.animator.Play("skill1_1");
            else
                championStatusController.photonView.RPC("PlayAnim", RpcTarget.All, "skill1_1");
        }

        public override void Skill2()
        {
            Debug.Log("THRESH Skill2()");
            championStatusController.championState = ChampionState.ATTACK;
            isMoveEnable = false;
                
            if (GameManager.instance.isBot)
                championStatusController.animator.Play("skill2");
            else
                championStatusController.photonView.RPC("PlayAnim", RpcTarget.All, "skill2");
        }

        public void Skill2EffectOn()
        {
            DamageModel attack1Damage = GetComponentInParent<ChampionStatusController>().newDamage(threshModel.attack3DamageModel, gameObject);

            GameObject effect = Instantiate(skill2Effect, transform.position, Quaternion.identity);
            float scaleValue = 5.8f;
            if (transform.localScale.x > 0.0f)
            {
                effect.transform.localScale = new Vector3(-1*scaleValue, scaleValue, scaleValue);
            }
            else
            {
                effect.transform.localScale = new Vector3(scaleValue, scaleValue, scaleValue);
            }
            effect.GetComponentInChildren<ThreshSkill2Script>().damageModel = attack1Damage;
            Destroy(effect, 0.27f);
        }
        
        public override void Skill3()
        {
            Debug.Log("THRESH Skill3()");
            championStatusController.championState = ChampionState.ATTACK;
            isMoveEnable = false;
            
            if (GameManager.instance.isBot)
                championStatusController.animator.Play("skill3");
            else
                championStatusController.photonView.RPC("PlayAnim", RpcTarget.All, "skill3");
        }

        public void Skill3Action()
        {
            Instantiate(skill3Effect, transform.parent.position, Quaternion.identity);
            StartCoroutine(Skill3Enumerator());
        }

        IEnumerator Skill3Enumerator()
        {
            yield return new WaitForSeconds(0.3f);
            if(isMine || GameManager.instance.isBot)
                championStatusController.moveController.speed *= 1.5f;
            GetComponent<SpriteRenderer>().color = new Color(0.0f, 1.0f, 0.969f);
            yield return new WaitForSeconds(3.0f);
            if (!isMine)
            {
                GetComponent<SpriteRenderer>().color = PlayerManagerMulti.enemySkinColor;
            }
            else
            {
                GetComponent<SpriteRenderer>().color = Color.white;
            }
            if(isMine || GameManager.instance.isBot)
                championStatusController.moveController.speed = 0.045f * championStatusController.championModel.movementSpeed;
        }
        
        public override void RunTrigger()
        {
            normalAttackCnt = 0;
        }
        
        public override void IdleTrigger()
        {
            normalAttackCnt = 0;
        }
    }
}