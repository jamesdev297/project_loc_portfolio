using System;
using System.Collections;
using System.Collections.Generic;
using Script;
using UnityEngine;
using UnityEngine.Serialization;
using Photon.Pun;


public enum ChampionState {
    NONE,
    IDLE,
    RUNNING,
    DAMAGED,
    ATTACK,  // include skill
    ROLLING,
    STUNNED,
}



public class PlayerStatusController : ChampionStatusController, IPunObservable
{
    public Vector2 speedMagnitude = new Vector2(4.0f, 2.0f);

    private bool isJumping = false;

    public float damage = 3;


    public void attack()
    {
        Debug.Log("attack()");
        AddEvent(new NormalAttackEvent());
    }

    public void skill1On()
    {
        Debug.Log("skill1On()");
        AddEvent(new Skill1OnEvent());
    }
    
    public void skill1Off()
    {
        Debug.Log("skill1On()");
        AddEvent(new Skill1OffEvent());
    }
    
    public void skill1()
    {
        Debug.Log("skill1()");
        AddEvent(new Skill1Event());
    }

    public void skill2()
    {
        Debug.Log("skill2()");
        AddEvent(new Skill2Event());
    }

    public void skill3()
    {
        Debug.Log("skill3()");
        AddEvent(new Skill3Event());
    }
    
    void Start()
    {
        Init();
        
        animator = transform.GetChild(0).GetComponent<Animator>();
        
        if (PhotonNetwork.IsConnected && photonView == null || (photonView != null && (photonView.IsMine ||
                    GameManager.instance.isBot)))
        {
            GameObject.Find("Attack1Button").GetComponent<Attack1Button>().playerStatusController = this;
            GameObject.Find("Skill1Button").GetComponent<Skill1ButtonScriopt>().playerStatusController = this;
            GameObject.Find("Skill2Button").GetComponent<Skill2ButtonScript>().playerStatusController = this;
            GameObject.Find("Skill3Button").GetComponent<Skill3ButtonScript>().playerStatusController = this;
        }
        

        AddEvent(new IdleEvent());
    }

    public override void AddEvent(ChampionStatusEvent championStatusEvent)
    {
        base.AddEvent(championStatusEvent);
        Debug.Log("Player AddEvent! " + championStatusEvent);
    }
    
    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
    }

}
