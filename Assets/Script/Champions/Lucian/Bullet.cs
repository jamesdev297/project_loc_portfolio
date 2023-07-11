using System;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    [SerializeField] private float moveSpeed = 0.0f;
    [SerializeField]
    private Vector3 moveDirection = Vector3.right;

    public DamageModel damageModel;

    void Start()
    {
        // Destroy(gameObject, 4.0f);    
    }

    void Update()
    {
        transform.position += moveDirection * moveSpeed * Time.deltaTime;
    }

    public void MoveTo(Vector3 direction)
    {
        moveDirection = direction;
    }
    
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "BorderBullet")
        {
            Destroy(gameObject);
        } else if (collision.gameObject.tag == "ChampionBody")
        {
            if (damageModel != null)
            {
                transform.GetComponentInChildren<AttackInfoScript>().damageModel = damageModel;
                Destroy(gameObject);
            }
        }
    }
}
