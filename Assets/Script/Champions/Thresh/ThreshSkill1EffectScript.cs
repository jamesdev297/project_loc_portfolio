using System;
using System.Collections;
using System.Collections.Generic;
using Photon.Pun;
using Script.Champions.Thresh;
using UnityEngine;

public class ThreshSkill1EffectScript : MonoBehaviourPun
{
    private float maxSkillTime = 1.2f;
    private float skillTime = 0f;
    private float speed = 17.0f;

    public ThreshBehaviour threshBehaviour;
    public GameObject ownerChmapionBehavior;

    private Vector3 direction;
    private Vector3 reverseDirection;

    private void Start()
    {
        if (transform.localScale.x > 0f)
        {
            direction = Vector3.left;
            reverseDirection = Vector3.right;
        }
        else
        {
            direction = Vector3.right;
            reverseDirection = Vector3.left;
        }
        Destroy(gameObject, 4.0f);
    }
    
    // Start is called before the first frame update
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (!photonView.IsMine)
            return;

        if (threshBehaviour == null)
            return;
        
        if (!threshBehaviour.skill1Catched)
        {
            if (other.tag == "ChampionBody")
            {
                if (other.transform.parent.gameObject != ownerChmapionBehavior)
                {
                    threshBehaviour.Skill1Catch(other.transform.parent.parent.gameObject);
                }
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (!photonView.IsMine)
            return;
        
        if (!threshBehaviour.skill1Catched)
        {
            skillTime += Time.deltaTime;
            if (skillTime < maxSkillTime/2-0.1f)
            {
                transform.Translate(direction * Time.deltaTime * speed);
            }else if (skillTime < maxSkillTime/2)
            {
                transform.Translate(direction * Time.deltaTime * speed/2);

            }else if (skillTime < maxSkillTime/2+0.1f)
            {
                transform.Translate(reverseDirection * Time.deltaTime * speed/2);

            }else if(skillTime < maxSkillTime)
            {
                transform.Translate(reverseDirection* Time.deltaTime * speed);
            }
            else
            {
                PhotonNetwork.Destroy(gameObject);
            }
        }
    }
}
