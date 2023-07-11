using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RangeCircleScript : MonoBehaviour
{
    public Transform target;
    
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (transform == null || target == null)
        {
            Destroy(gameObject);
            return;
        }
        transform.position = target.position;
    }
}
