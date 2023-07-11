using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackInfoScript : MonoBehaviour
{
    public DamageModel damageModel;

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (damageModel != null)
        {
            if (other.transform.parent != null)
            {
                if (other.transform.parent.gameObject != damageModel.owner)
                {
                    if (other.tag == "ChampionBody")
                    {
                        other.GetComponentInParent<ChampionStatusController>().AddEvent(new DamagedEvent(damageModel));
                    }
                }
            }
        }
        
    }
}
