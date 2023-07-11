using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ThreshSkill2Script : MonoBehaviour
{
    public GameObject hitbox;
    public DamageModel damageModel;
    // Start is called before the first frame update
    void Start()
    {
        hitbox.GetComponent<AttackInfoScript>().damageModel = damageModel;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void CreateHitbox()
    {
        hitbox.SetActive(true);
    }
}
