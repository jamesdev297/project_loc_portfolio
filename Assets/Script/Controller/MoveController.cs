using System;
using System.Collections;
using UnityEngine;
using Photon.Pun;
using Script;

public class MoveController : MonoBehaviourPunCallbacks
{
    public float speed = 1.0f;
    public GameObject footRing;
    public GameObject footRingPrefab;
    public Transform childTransform;
    protected Vector3 childLocalScale;
    protected Transform footCollider;
    public GameObject footColliderPrefab;

    private IEnumerator knockBackIEnumertaor;
    
    protected float lastY;
    public bool isLookTarget = false;

    public void KnockBack(bool isAlive, DamageModel damageModel, Action then)
    {
        if(isAlive)
            lookTarget(damageModel);
        if (knockBackIEnumertaor != null)
            StopCoroutine(knockBackIEnumertaor);

        knockBackIEnumertaor = StartKnockBack(isAlive, damageModel, then);
        StartCoroutine(knockBackIEnumertaor);
    }
    
    void lookTarget(DamageModel damageModel)
    {
        if (damageModel.owner.transform.position.x < transform.position.x)
        {
            childTransform.localScale = new Vector3(childLocalScale.x * -1, childLocalScale.y, childLocalScale.z);
        }
        else
        {
            childTransform.localScale = childLocalScale;
        }
    }
    
    public void lookTarget(Vector3 vector3)
    {
        if (vector3.x < transform.position.x)
        {
            childTransform.localScale = new Vector3(childLocalScale.x * -1, childLocalScale.y, childLocalScale.z);
        }
        else
        {
            childTransform.localScale = childLocalScale;
        }
    }

    IEnumerator StartKnockBack(bool isAlive, DamageModel damageModel, Action then)
    {
        PlayerManagerMulti.Instance.hitSound.Play();
        ChampionBehavior championBehavior = GetComponentInChildren<ChampionBehavior>();
        
        championBehavior.spriteRenderer.material.shader = championBehavior.whiteShader;
        yield return new WaitForSeconds(0.07f);
        championBehavior.spriteRenderer.material.shader = championBehavior.defaultShader;

        if(!isAlive)
            yield break;
        
        
        float knockBackTime = damageModel.knuckBackDelayTime;
        Debug.Log("start getdamged" + damageModel.knuckBackDelayTime);
        
        while (knockBackTime > 0.0f)
        {
            Vector3 dir;
            if (damageModel.owner.transform.position.x < transform.position.x)
            {
                dir = Vector3.right;
            }
            else
            {
                dir = Vector3.left;
            }
            transform.Translate(dir * Time.deltaTime);
            knockBackTime -= 1.0f;
        }

        then();
    }
        
}