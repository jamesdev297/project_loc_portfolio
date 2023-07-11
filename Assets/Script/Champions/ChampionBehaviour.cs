
using System;
using System.Collections;
using Photon.Pun;
using Script;
using UnityEngine;
using UnityEngine.UI;

public class ChampionBehavior : MonoBehaviourPun
{
    public bool invincibility = false;
    public Color defaultColor = Color.white;
    public Shader whiteShader;
    public Shader defaultShader;
    public SpriteRenderer spriteRenderer;
    public bool isMoveEnable = true;
    public int normalAttackCnt = 0;
    public float skill1CoolTime = 0.0f;
    public float skill2CoolTime = 0.0f;
    public float skill3CoolTime = 0.0f;
    public float normalAttackCoolTime = 0.0f;
    public ChampionStatusController championStatusController;
    public bool isJump;
    public bool isRolling;
    public bool isMine;
    public bool isDie = false;
    public bool isStun = false;
    

    public void MoveEnable()
    {
        if (!GameManager.instance.isBot && !photonView.IsMine)
            return;
        if (championStatusController != null)
        {
            if(!championStatusController.stunned)
                isMoveEnable = true;
        }
    }
    
    public void MoveDisable()
    {
        if (!GameManager.instance.isBot && !photonView.IsMine)
            return;
        isMoveEnable = false;
    }

    public virtual void RunTrigger()
    {
        
    }

    public virtual void IdleTrigger()
    {
        
    }
    
    public virtual void Attack1()
    {
        
    }
    public virtual void Attack1Play()
    {
        
    }

    public virtual void Stun()
    {
        if (isMine)
        {
            isMoveEnable = false;
            isStun = true;
        }
        
        if (GameManager.instance.isBot)
            championStatusController.animator.Play("idle");
        else
            championStatusController.photonView.RPC("PlayAnim", RpcTarget.All, "idle");
    }
    
    IEnumerator SkillCoolTimeEnd(Image image)
    {
        image.fillAmount = 1.0f;
        image.color = defaultColor;
        yield return new WaitForSeconds(0.2f);
        image.color = championStatusController.defaultSkillCoolTimeColor;
        image.fillAmount = 0.0f;
        yield return null;
    }

    private void SetCoolTime(float currentCoolTime, float maxCoolTime, Image coolTimeImage)
    {
        coolTimeImage.fillAmount = (currentCoolTime / maxCoolTime);
        if (currentCoolTime <= 0.0f)
        {
            StartCoroutine(SkillCoolTimeEnd(coolTimeImage));
        }
    }
    
    public void coolTimeUpdate()
    {
        if (skill1CoolTime > 0.0f)
        {
            skill1CoolTime -= Time.deltaTime;
            if(isMine)
                SetCoolTime(skill1CoolTime, championStatusController.championModel.skills[0].cooldown, championStatusController.skill1CoolTimeImage);
        }
        if (skill2CoolTime > 0.0f)
        {
            skill2CoolTime -= Time.deltaTime;
            if(isMine)
                SetCoolTime(skill2CoolTime, championStatusController.championModel.skills[1].cooldown, championStatusController.skill2CoolTimeImage);
        }
        if (normalAttackCoolTime > 0.0f)
        {
            normalAttackCoolTime -= Time.deltaTime;
        }
        if (skill3CoolTime > 0.0f)
        {
            skill3CoolTime -= Time.deltaTime;
            if(isMine)
                SetCoolTime(skill3CoolTime, championStatusController.championModel.skills[2].cooldown, championStatusController.skill3CoolTimeImage);
        }
    }

    public void Die()
    {
        Debug.Log("MATT DIE()");
        isDie = true;
        if (GameManager.instance.isBot)
        {
            PlayerManagerMulti.Instance.RPCOnePlayerDied();
        }
        else
        {
            PlayerManagerMulti.Instance.photonView.RPC("RPCOnePlayerDied", RpcTarget.All);
        }
        StartCoroutine(JumpEnumerator(1.0f, 5.5f, true));
    }
    
    public IEnumerator JumpEnumerator(float time, float offset, bool lieBack)
    {
        Debug.Log("MATT JumpEnumerator()");

        PhotonTransformViewClassic photonTransform = GetComponentInParent<PhotonTransformViewClassic>();
        if(photonTransform != null)
            photonTransform.enabled = false;
        
        isJump = true;
        Vector3 startingPos = transform.parent.position;
        Vector3 finalPos = transform.parent.position;
        if (transform.localScale.x > 0.0f)
            finalPos = new Vector3(finalPos.x - offset, finalPos.y, finalPos.z);
        else
            finalPos = new Vector3(finalPos.x + offset, finalPos.y, finalPos.z);
        
        if(lieBack)
            transform.parent.rotation = Quaternion.Euler(new Vector3(0.0f, 0.0f, 90.0f * (transform.localScale.x > 0 ? 1 : -1)));

        float elapsedTime = 0;
        
        while (elapsedTime < time)
        {
            float _progress = elapsedTime / time;

            float parabola = 1.0f - 4.0f * (_progress - 0.5f) * (_progress - 0.5f);
            Vector3 pos = Vector3.Lerp(startingPos, finalPos, _progress);
            pos.y += parabola * 3.0f;
            
            transform.parent.position = pos;
            elapsedTime += Time.deltaTime;
            yield return null;
        }
        isJump = false;

        if (!lieBack && (isMine || GameManager.instance.isBot))
        {
            championStatusController.stunned = false;
            isMoveEnable = true;
        }
        
        if(photonTransform != null)
            photonTransform.enabled = true;

        yield return null;
    }

    public virtual void Skill1()
    {
        
    }
    
    public virtual void Skill1On()
    {
        
    }
    
    public virtual void Skill1Off()
    {
        
    }
    
    public virtual void Skill2()
    {
        
    }
    
    public virtual void Skill3()
    {
        
    }

    public virtual void GetDamaged()
    {
        
    }

    public void delayedSound(float delayedTime, AudioSource audioSource)
    {
        StartCoroutine(delayedSoundEnumerator(delayedTime, audioSource));
    }
    IEnumerator delayedSoundEnumerator(float delayedTime, AudioSource audioSource)
    {
        yield return new WaitForSeconds(delayedTime);
        audioSource.Play();
    }
    

}