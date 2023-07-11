
using System;
using System.Collections;
using Photon.Pun;
using Script;
using Script.Champions;
using UnityEngine;



public class  JaxBehaviour: ChampionBehavior
{
    public GameObject attack1Collider;
    public Transform attack1Offset;
    private JaxModel _jaxModel;
    public GameObject attack1Range;
    public GameObject rangeCirclePrefab;
    public GameObject rangeCircle;
    
    private bool qRangeOn = false;
    public Vector3 qTargetVec = Vector3.zero;
    private IEnumerator jumpEnumerator;

    private IEnumerator jaxEEnumerator;
    private IEnumerator attack1ForwardEnumerator;
    private IEnumerator jaxRollEnumerator;
    public GameObject jaxEWeapon;


    // Start is called before the first frame update
    void Start()
    {
        _jaxModel = GameInfoManager.Instance.ChampMap[Constants.JaxId] as JaxModel;
        spriteRenderer = GetComponent<SpriteRenderer>();
        defaultShader = spriteRenderer.material.shader;

        if (GameObject.Find("GroundLayer"))
        {
            rangeCircle = Instantiate(rangeCirclePrefab, GameObject.Find("GroundLayer").transform);
            rangeCircle.GetComponent<SpriteMask>().enabled = false;
            rangeCircle.GetComponent<RangeCircleScript>().target = transform.parent;
        }
    }

    public bool isInQRange(Vector3 targetPos)
    {
        return transform.parent.position.x + 9.0f > targetPos.x
               && transform.parent.position.x - 9.0f < targetPos.x
               && transform.parent.position.y + 4.0f > targetPos.y
               && transform.parent.position.y - 4.0f < targetPos.y;
    }
    
    // Update is called once per frame
    void Update()
    {
        if (isMine || GameManager.instance.isBot)
            coolTimeUpdate();
        
        if (qRangeOn)
        {
            Vector3 targetPos = PlayerManagerMulti.Instance.enemy.transform.position;

            if (isInQRange(targetPos))
            {
                PlayerManagerMulti.Instance.enemy.GetComponentInChildren<SpriteRenderer>().color = Color.blue;
                qTargetVec = new Vector3(targetPos.x, targetPos.y, targetPos.z);
            }
            else
            {
                PlayerManagerMulti.Instance.enemy.GetComponentInChildren<SpriteRenderer>().color = PlayerManagerMulti.enemySkinColor;
                qTargetVec = Vector3.zero;
            }
        }   
    }

    private IEnumerator JumpToEnemy(float time, Transform target, Vector3 finalPos)
    {
        if (target.position.x < finalPos.x)
            finalPos = new Vector3(finalPos.x - 2.0f, finalPos.y - 0.6f, finalPos.z);
        else
            finalPos = new Vector3(finalPos.x + 2.0f, finalPos.y - 0.6f, finalPos.z);
        
        PhotonTransformViewClassic photonTransform = GetComponentInParent<PhotonTransformViewClassic>();
        if(photonTransform != null)
            photonTransform.enabled = false;
        
        isJump = true;
        Vector3 startingPos = target.position;
        
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
        
        if(photonTransform != null)
            photonTransform.enabled = true;

        yield return null;
    }
    
    public override void Attack1Play()
    {
        normalAttackCnt++;
    }
    
    public void Attack1AttackBox()
    {
        DamageModel attack1Damage = GetComponentInParent<ChampionStatusController>().newDamage(_jaxModel.normalAttackDamageModel, gameObject);
        GameObject collider = Instantiate(attack1Collider, attack1Offset.position, Quaternion.identity);
        float localScaleX = collider.transform.localScale.x;
        collider.transform.localScale = new Vector3(localScaleX*1.5f, 1.0f, 1.0f);
        collider.GetComponent<AttackInfoScript>().damageModel = attack1Damage;
        Destroy(collider, 0.2f);
    }
    
    public void Attack3AttackBox()
    {
        DamageModel attack1Damage = GetComponentInParent<ChampionStatusController>().newDamage(_jaxModel.attack3DamageModel, gameObject);
        GameObject collider = Instantiate(attack1Collider, attack1Offset.position, Quaternion.identity);
        float localScaleX = collider.transform.localScale.x;
        collider.transform.localScale = new Vector3(localScaleX*2.0f, 1.0f, 1.0f);
        collider.GetComponent<AttackInfoScript>().damageModel = attack1Damage;
        Destroy(collider, 0.2f);
    }
    
    public void QAttackBox()
    {
        DamageModel attack1Damage = GetComponentInParent<ChampionStatusController>().newDamage(_jaxModel.attack3DamageModel, gameObject);
        GameObject collider = Instantiate(attack1Collider, attack1Offset.position, Quaternion.identity);
        float localScaleX = collider.transform.localScale.x;
        float localScaleY = collider.transform.localScale.y;

        collider.transform.localScale = new Vector3(localScaleX*2.0f, localScaleY*1.5f, 1.0f);
        collider.GetComponent<AttackInfoScript>().damageModel = attack1Damage;
        Destroy(collider, 0.3f);
    }
    
    public void EAttackBox()
    {
        DamageModel attack1Damage = GetComponentInParent<ChampionStatusController>().newDamage(_jaxModel.skill2DamageModel, gameObject);
        GameObject collider = Instantiate(attack1Collider, attack1Offset.position, Quaternion.identity);
        float localScaleX = collider.transform.localScale.x;
        float localScaleY = collider.transform.localScale.y;
        collider.transform.localScale = new Vector3(localScaleX*3.0f, localScaleY*2.0f, 1.0f);
        collider.GetComponent<AttackInfoScript>().damageModel = attack1Damage;
        Destroy(collider, 0.2f);
    }

    IEnumerator Attack1GoForward(float delayTime, float forwardTime)
    {
        yield return new WaitForSeconds(delayTime);

        float dir = transform.localScale.x;

        float speed = 400.0f;
        float elapsedTime = 0;
        
        while (elapsedTime < forwardTime)
        {
            Vector3 pos = transform.parent.position;
            transform.parent.position = new Vector3(pos.x + Mathf.Clamp01(forwardTime - elapsedTime) * dir * Time.deltaTime * speed,  pos.y, pos.z);

            elapsedTime += Time.deltaTime;
            yield return null;
        }
        
        yield return null;
    }
    
    public override void Attack1()
    {
        isMoveEnable = false;
        championStatusController.championState = ChampionState.ATTACK;
        if(attack1ForwardEnumerator != null)
            StopCoroutine(attack1ForwardEnumerator);
        
        if (normalAttackCnt >= 2)
        {
            attack1ForwardEnumerator = Attack1GoForward(0.15f, 0.18f);
            StartCoroutine(attack1ForwardEnumerator);

            if (GameManager.instance.isBot)
                championStatusController.animator.Play("attack3");
            else
                championStatusController.photonView.RPC("PlayAnim", RpcTarget.All, "attack3");
            normalAttackCnt = 0;
        }
        else
        {
            attack1ForwardEnumerator = Attack1GoForward(0.1f, 0.15f);
            StartCoroutine(attack1ForwardEnumerator);

            if (GameManager.instance.isBot)
                championStatusController.animator.Play("attack1");
            else
                championStatusController.photonView.RPC("PlayAnim", RpcTarget.All, "attack1");
        }
    }
    
    
    public override void Skill1On()
    {
        rangeCircle.GetComponent<SpriteMask>().enabled = true;
        qRangeOn = true;
        
    }
    
    public override void Skill1Off()
    {
        rangeCircle.GetComponent<SpriteMask>().enabled = false;
        qRangeOn = false;
        PlayerManagerMulti.Instance.enemy.GetComponentInChildren<SpriteRenderer>().color = PlayerManagerMulti.enemySkinColor;
    }

    [PunRPC]
    public void RPCJumpToEnemy(Vector3 targetVector)
    {
        StartCoroutine(JumpToEnemy(0.3f, transform.parent, targetVector));
    }
    
    
    IEnumerator Rolling(float dir, float time)
    {
        PhotonTransformViewClassic photonTransform = GetComponentInParent<PhotonTransformViewClassic>();
        if(photonTransform != null)
            photonTransform.enabled = false;

        isRolling = true;
        float elapsedTime = 0;
        float rollingSpd = 13.0f;
        float defaultY = transform.parent.position.y;

        while (elapsedTime < time)
        {
            Vector3 pos = transform.parent.position;
            if (dir > 0.0f)
            {
                transform.parent.position = new Vector3(pos.x + rollingSpd * Time.deltaTime,  defaultY + 0.9f*Mathf.Sin((elapsedTime* Mathf.PI) / time), pos.z);
                transform.localRotation = Quaternion.Euler(new Vector3(0.0f, 0.0f, -360.0f * (elapsedTime/time)));
            }
            else
            {
                transform.parent.position = new Vector3(pos.x - rollingSpd * Time.deltaTime, defaultY + 0.9f*Mathf.Sin((elapsedTime* Mathf.PI) / time), pos.z);
                transform.localRotation = Quaternion.Euler(new Vector3(0.0f, 0.0f, 360.0f * (elapsedTime/time)));
            }

            elapsedTime += Time.deltaTime;
            yield return null;
        }
        
        isMoveEnable = true;
        // notIdleAnim = false;
        transform.localPosition = Vector3.zero;
        transform.localRotation = Quaternion.identity;
        isRolling = false;

        if(photonTransform != null)
            photonTransform.enabled = true;

        yield return null;
    }
    
    [PunRPC]
    public void RPCRolling(float dir, float time)
    {
        if (jaxRollEnumerator != null)
        {
            StopCoroutine(jaxRollEnumerator);
        }
        jaxRollEnumerator = Rolling(dir, time);
        StartCoroutine(jaxRollEnumerator);
    }

    [PunRPC]
    public void RPCJaxE(String code)
    {
        if (code == "start")
        {
            jaxEWeapon.SetActive(true);
        }else if (code == "final")
        {
            jaxEWeapon.SetActive(false);
        }
    }
    
    IEnumerator JaxE()
    {
        if (GameManager.instance.isBot)
        {
            championStatusController.animator.Play("skill2");
            jaxEWeapon.SetActive(true);
        }
        else
        {
            championStatusController.photonView.RPC("PlayAnim", RpcTarget.All, "skill2");
            photonView.RPC("RPCJaxE", RpcTarget.All, "start");
        }

        yield return new WaitForSeconds(1.0f);
        jaxEWeapon.SetActive(false);

        // isMoveEnable = false;
        if (GameManager.instance.isBot)
        {
            championStatusController.animator.Play("skill2final");
        }
        else
        {
            championStatusController.photonView.RPC("PlayAnim", RpcTarget.All, "skill2final");
            photonView.RPC("RPCJaxE", RpcTarget.All, "final");
        }
    }
    
    public override void Skill1()
    {
        Debug.Log($"MATT JAX Skill1! {championStatusController.championState} {isMoveEnable}");

        if (!isMine)
        {
            championStatusController.championState = ChampionState.ATTACK;
            isMoveEnable = false;
            championStatusController.moveController.lookTarget(qTargetVec);
            StartCoroutine(JumpToEnemy(0.3f, transform.parent, qTargetVec));
            championStatusController.animator.Play("skill1");
        }
        
        if (qTargetVec != Vector3.zero)
        {
            if(qRangeOn)
            {
                championStatusController.championState = ChampionState.ATTACK;
                isMoveEnable = false;
                championStatusController.moveController.lookTarget(qTargetVec);
                if (GameManager.instance.isBot)
                {
                    StartCoroutine(JumpToEnemy(0.3f, transform.parent, qTargetVec));
                    championStatusController.animator.Play("skill1");
                }
                else
                {
                    photonView.RPC("RPCJumpToEnemy", RpcTarget.All, qTargetVec);
                    championStatusController.photonView.RPC("PlayAnim", RpcTarget.All, "skill1");
                }
            }
        }
        else
        {
            championStatusController.championState = ChampionState.IDLE;
        }
        
        rangeCircle.GetComponent<SpriteMask>().enabled = false;
        PlayerManagerMulti.Instance.enemy.GetComponentInChildren<SpriteRenderer>().color = PlayerManagerMulti.enemySkinColor;
        qRangeOn = false;
    }
    
    public override void Skill2()
    {
        // championStatusController.championState = ChampionState.ATTACK;
        if (jaxEEnumerator != null)
        {
            StopCoroutine(jaxEEnumerator);
            jaxEEnumerator = null;
        }
        jaxEEnumerator = JaxE();
        StartCoroutine(jaxEEnumerator);
    }
    
    public override void Skill3()
    {
        championStatusController.championState = ChampionState.ROLLING;
        float currentDir = transform.localScale.x;
        float rollingTime = 0.4f;
        if (GameManager.instance.isBot)
        {
            if (jaxRollEnumerator != null)
            {
                StopCoroutine(jaxRollEnumerator);
            }
            jaxRollEnumerator = Rolling(currentDir, rollingTime);
            StartCoroutine(jaxRollEnumerator);
            championStatusController.animator.Play("skill3");
        }
        else
        {
            photonView.RPC("RPCRolling", RpcTarget.All, currentDir, rollingTime);
            championStatusController.photonView.RPC("PlayAnim", RpcTarget.All, "skill3");
        }
    }

    public override void GetDamaged()
    {
        // jaxEWeapon.SetActive(false);
    }
}
