using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class bushScript : MonoBehaviour
{
    private Animator _animator;
    // Start is called before the first frame update
    void Start()
    {
        _animator = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "ChampionBody")
        {
            Debug.Log("Enter!" + other);
            _animator.SetTrigger("active");
        }
    }
}
