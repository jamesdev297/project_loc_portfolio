using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackRangeScript : MonoBehaviour
{
    public GameObject target;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "ChampionBody")
        {
            Debug.Log($"Enter ChampionBody! other:{other.transform.parent.parent.gameObject.name}");
            target = other.transform.parent.parent.gameObject;
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.tag == "ChampionBody")
        {
            Debug.Log($"Exit ChampionBody! other:{other.transform.parent.parent.gameObject.name}");
            target = null;
        }
    }
}
