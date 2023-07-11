using System.Collections;
using Photon.Pun;
using RSG;
using UnityEngine;

namespace Script.Champions.Lucian
{
    public class LucianBehavior : ChampionBehavior
    {
        public Transform attack1Offset;
        private LucianModel _lucianModel;
        public GameObject attack1Range;
        public GameObject attack1Effect;
        public GameObject distance1Range;
        public GameObject skill2Effect;
        public GameObject skill3LegOffset;
        public GameObject skill3LegEffect;

        public bool isAttack = false;
        private bool isReadySkill1 = false;
        private bool isLeftDirectionForSkill1 = false;
        private IEnumerator skill1ForwardEnumerator;
        private IEnumerator skill2ForwardEnumerator;
        private IEnumerator skill3ForwardEnumerator;
        public bool proceedingSkill3 = false;

        public AudioSource bulletSound;
        public AudioSource laserSound;

        void Start()
        {
            _lucianModel = GameInfoManager.Instance.ChampMap[Constants.LucianId] as LucianModel;
            spriteRenderer = GetComponent<SpriteRenderer>();
            defaultShader = spriteRenderer.material.shader;
        }

        void Update()
        {
            
            if (isMine || GameManager.instance.isBot)
                coolTimeUpdate();
            
            if (transform.localScale.x < 0.0f)
            {
                isAttack = CanSeePlayer(-10f);
                
            }
            else
            {
                isAttack = CanSeePlayer(10f);
            }

            if (isReadySkill1 == true)
            {
                if (isLeftDirectionForSkill1)
                {
                    transform.parent.position += Vector3.left * 0.15f * Time.deltaTime * 90f;
                }
                else
                {
                    transform.parent.position += Vector3.right * 0.15f * Time.deltaTime * 90f;
                }
            }
        }

        public override void Attack1()
        {
            if (proceedingSkill3 == true || isReadySkill1 == true)
                return;
            
            isMoveEnable = false;
            championStatusController.championState = ChampionState.ATTACK;
            if (GameManager.instance.isBot)
                championStatusController.animator.Play("attack1");
            else
                championStatusController.photonView.RPC("PlayAnim", RpcTarget.All, "attack1");
        }

        public override void Skill1()
        {
            if (proceedingSkill3 == true)
                return;
            
            Skill1Start();
        }

        public void Skill1Start()
        {
            isMoveEnable = false;
            championStatusController.championState = ChampionState.RUNNING;
            isReadySkill1 = true;
            isLeftDirectionForSkill1 = transform.localScale.x < 0.0f;
            
            if (skill1ForwardEnumerator == null)
            {
                skill1ForwardEnumerator = SkillGoForward(1.0f);
            }
            
            StopCoroutine(skill1ForwardEnumerator);
            
            // isMoveEnable = true;
            if (GameManager.instance.isBot)
            {
                championStatusController.animator.Play("skill1");
            }
            else
            {
                championStatusController.photonView.RPC("PlayAnim", RpcTarget.All, "skill1");
            }
            
            // if (transform.localScale.x < 0.0f)
            // {
            //     transform.parent.position += new Vector3(-5.0f, 0, 0);
            // }
            // else
            // {
            //     transform.parent.position += new Vector3(5.0f, 0, 0);
            // }
            //StartCoroutine(MoveSkill(isLeftDirection));
        }

        public void Skill1End()
        {
            isReadySkill1 = false;
            isMoveEnable = true;
        }

        // private IEnumerator MoveSkill(bool isLeftDirection)
        // {
        //     float time = 2f;
        //     while (time > 0)
        //     {
        //         if (isLeftDirection)
        //         {
        //             transform.parent.position += Vector3.left * 1f;
        //         }
        //         else
        //         {
        //             transform.parent.position += Vector3.right * 1f;
        //         }
        //
        //         time -= 0.4f;
        //         yield return new WaitForSeconds(0.2f);
        //     }
        // }
        
        
        public bool CanSeePlayer(float distance)
        {
            bool val;

            Vector2 endPos = attack1Range.gameObject.transform.position + Vector3.right * distance;
            
            RaycastHit2D hit = Physics2D.Linecast(attack1Range.gameObject.transform.position, endPos,
                1 << LayerMask.NameToLayer("Default"));
            
            
            if (hit.collider != null)
            {
                if (hit.collider.gameObject.CompareTag("ChampionBody"))
                {
                    val = true;
                }
                else
                {
                    val = false;
                }
                
                Debug.DrawLine(attack1Range.gameObject.transform.position, hit.point, Color.blue);
            }
            else
            {
                val = false;
                Debug.DrawLine(attack1Range.gameObject.transform.position, endPos, Color.blue);
            }
            
            return val;
        } 
        
        public override void Skill2()
        {
            
            
            
            if (proceedingSkill3 == true || isReadySkill1 == true)
                return;
            
            isMoveEnable = false;
            championStatusController.championState = ChampionState.ATTACK;
            
            if (skill2ForwardEnumerator != null)
            {
                StopCoroutine(skill2ForwardEnumerator);
                skill2ForwardEnumerator = null;
            }

            skill2ForwardEnumerator = LucianQ();
            StartCoroutine(skill2ForwardEnumerator);
            
            if (GameManager.instance.isBot)
            {
                championStatusController.animator.Play("skill2");
            }
            else
            {
                championStatusController.photonView.RPC("PlayAnim", RpcTarget.All, "skill2");
            }
            
        }

        [PunRPC]
        public void RPCLucianQ()
        {
            Invoke("InstantiateBeem", 0.3f);
        }

        [PunRPC]
        public void RPCLucianE()
        {
            InstantiateLeg();
        }

        IEnumerator SkillGoForward(float delayTime)
        {
            yield return new WaitForSeconds(delayTime);
        }

        IEnumerator LucianR()
        {
            championStatusController.moveController.isLookTarget = true;
            if (GameManager.instance.isBot)
            {
                InstantiateLeg();
            }
            else
                photonView.RPC("RPCLucianE", RpcTarget.All);

            yield return new WaitForSeconds(1.7f);
            championStatusController.moveController.isLookTarget = false;
            proceedingSkill3 = false;
        }
        
        IEnumerator LucianQ()
        {
            if (GameManager.instance.isBot)
            {
                Invoke("InstantiateBeem", 0.3f);
            }
            else
                photonView.RPC("RPCLucianQ", RpcTarget.All);

            yield return new WaitForSeconds(0.2f);
        }

        void InstantiateBeem()
        {
            laserSound.Play();
            Vector3 position = attack1Offset.position;
            
            if (transform.localScale.x > 0.0f)
            {
                position.x += 2.5f;
                position.y -= .2f;
            }
            else
            {
                position.x -= 2.5f;
                position.y -= .2f;
            }
            DamageModel attack1Damage = GetComponentInParent<ChampionStatusController>().newDamage(_lucianModel.skill2DamageModel, gameObject);
            GameObject effect = Instantiate(skill2Effect, position, Quaternion.identity);
            effect.GetComponent<AttackInfoScript>().damageModel = attack1Damage;
            
            float scaleValue = 5.8f;
            
            if (transform.localScale.x > 0.0f)
            {
                effect.transform.localScale = new Vector3(scaleValue, scaleValue, scaleValue);
            }
            else
            {
                effect.transform.localScale = new Vector3(-1*scaleValue, scaleValue, scaleValue);
            }
            
            Destroy(effect, 0.2f);
        }

        void InstantiateLeg()
        {
            GameObject effect = Instantiate(skill3LegEffect, skill3LegOffset.transform.position, Quaternion.identity);
            float scaleValue = 1f;
            effect.transform.SetParent(transform);
            SpriteRenderer spriteRenderer = effect.GetComponent<SpriteRenderer>();
            if (transform.localScale.x > 0.0f)
            {
                spriteRenderer.flipX = true;
                effect.transform.localScale = new Vector3(scaleValue, scaleValue, scaleValue);
            }
            else
            {
                spriteRenderer.flipX = false;
                effect.transform.localScale = new Vector3(-1*scaleValue, scaleValue, scaleValue);
            }
            Destroy(effect, 1.7f);
        }
        
        public override void Skill3()
        {

            if (isReadySkill1 == true)
                return;
            
            proceedingSkill3 = true;
            isMoveEnable = true;
            championStatusController.championState = ChampionState.RUNNING;
            
            if (skill3ForwardEnumerator != null)
            {
                StopCoroutine(skill3ForwardEnumerator);
                skill3ForwardEnumerator = null;
            }

            skill3ForwardEnumerator = LucianR();
            StartCoroutine(skill3ForwardEnumerator);
            
            if (GameManager.instance.isBot)
            {
                championStatusController.animator.Play("skill3");
            }
            else
            {
                championStatusController.photonView.RPC("PlayAnim", RpcTarget.All, "skill3");
            }
        }
        public void Attack3()
        {

            if (isReadySkill1 == true)
                return;
            
            bulletSound.Play();
            DamageModel attack1Damage = GetComponentInParent<ChampionStatusController>().newDamage(_lucianModel.normalAttackDamageModel, gameObject);
            GameObject effect = Instantiate(attack1Effect, attack1Offset.position, Quaternion.identity);
            Bullet bullet = effect.GetComponent<Bullet>();
            bullet.damageModel = attack1Damage;
            SpriteRenderer spriteRenderer = effect.GetComponent<SpriteRenderer>();
            if (transform.localScale.x < 0.0f)
            {
                spriteRenderer.flipX = false;
                bullet.MoveTo(new Vector3(-1, 0, 0));
            }
            else
            {
                spriteRenderer.flipX = true;
                bullet.MoveTo(new Vector3(1, 0, 0));
            }
        }
        
    }
    
}