using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BackSpriteScript : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        transform.Translate(-2*Time.deltaTime, -2*Time.deltaTime, 0.0f);
    }
}
