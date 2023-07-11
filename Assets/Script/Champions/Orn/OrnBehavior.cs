using System.Collections;
using UnityEngine;

namespace Script.Champions.Orn
{
    public class OrnBehavior : ChampionBehavior
    {
        public GameObject attack1Collider;
        public Transform attack1Offset;
        public Transform skill1Offset;
        public GameObject attack1Range;
        public GameObject skill2Range;
        private OrnModel _ornModel;
        public GameObject distance1Range;
        public GameObject skill1Effect;
        public GameObject skill2Effect;
        private bool isReadySkill2 = false;
        private bool isLeftDirectionForSkill2 = false;



        void Start()
        {
            _ornModel = ChampionFactory.Create(ChampionFactory.ChampionType.Orn) as OrnModel;
            spriteRenderer = GetComponent<SpriteRenderer>();
            defaultShader = spriteRenderer.material.shader;
        }

        void Update()
        {
            
            coolTimeUpdate();
            
            if (isReadySkill2 == true)
            {
                if (isLeftDirectionForSkill2)
                {
                    transform.parent.Translate(Vector3.left * Time.deltaTime * 10f);
                }
                else
                {
                    transform.parent.Translate(Vector3.right * Time.deltaTime * 10f);
                }
            }
        }

        public void Attack1()
        {

            DamageModel attack1Damage = GetComponentInParent<ChampionStatusController>().newDamage(_ornModel.normalAttackDamageModel, gameObject);
            GameObject collider = Instantiate(attack1Collider, attack1Offset.position, Quaternion.identity);
            collider.GetComponent<AttackInfoScript>().damageModel = attack1Damage;
            Destroy(collider, 0.2f);
            
        }
        
        public void Skill1()
        {
            
            DamageModel skill11Damage = GetComponentInParent<ChampionStatusController>().newDamage(_ornModel.skill1DamageModel, gameObject);
            GameObject effect = Instantiate(skill1Effect, skill1Offset.position, Quaternion.identity);
            effect.GetComponent<AttackInfoScript>().damageModel = skill11Damage;
            Destroy(effect, 1.5f);
        }

        public void Skill2Start()
        {
            isReadySkill2 = true;
            isLeftDirectionForSkill2 = transform.localScale.x < 0.0f;
            
            DamageModel skill2Damage = GetComponentInParent<ChampionStatusController>().newDamage(_ornModel.skill2DamageModel, gameObject);

            GameObject effect = Instantiate(skill2Effect, transform.position, Quaternion.identity);
            //effect.transform.SetParent(transform);
            
            float scaleValue = 0.8f;
            
            if (transform.localScale.x > 0.0f)
            {
                effect.transform.localScale = new Vector3(scaleValue, scaleValue, scaleValue);
            }
            else
            {
                effect.transform.localScale = new Vector3(-1*scaleValue, scaleValue, scaleValue);
            }

            effect.GetComponentInChildren<AttackInfoScript>().damageModel = skill2Damage;
            
            
            StartCoroutine(MoveSkill(isLeftDirectionForSkill2, effect));
            
        }
        
        private IEnumerator MoveSkill(bool isLeftDirection, GameObject effect)
        {
            float time = 2f;
            while (time > 0)
            {
                if (isLeftDirection)
                {
                    effect.transform.position += Vector3.left * 1f;
                }
                else
                {
                    effect.transform.position += Vector3.right * 1f;
                }
        
                time -= 0.4f;
                yield return new WaitForSeconds(0.2f);
            }
            
            Destroy(effect);
        }


        public void Skill2End()
        {
            isReadySkill2 = false;
        }
        
        
        
        // public bool CanSeePlayer(float distance)
        // {
        //     bool val;
        //
        //     Vector2 endPos = attack1Range.gameObject.transform.position + Vector3.right * distance;
        //     
        //     RaycastHit2D hit = Physics2D.Linecast(attack1Range.gameObject.transform.position, endPos,
        //         1 << LayerMask.NameToLayer("Default"));
        //     
        //     
        //     if (hit.collider != null)
        //     {
        //         if (hit.collider.gameObject.CompareTag("ChampionBody"))
        //         {
        //             val = true;
        //         }
        //         else
        //         {
        //             val = false;
        //         }
        //         
        //         Debug.DrawLine(attack1Range.gameObject.transform.position, hit.point, Color.blue);
        //     }
        //     else
        //     {
        //         val = false;
        //         Debug.DrawLine(attack1Range.gameObject.transform.position, endPos, Color.blue);
        //     }
        //     
        //     return val;
        // } 
        
        public void Skill3()
        {
        }
    }
    
}