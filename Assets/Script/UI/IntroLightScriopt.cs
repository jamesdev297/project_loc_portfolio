using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IntroLightScriopt : MonoBehaviour
{
    private float sizeFactor = 0.97f;

    private Vector3 backVector;

    public SpriteRenderer lightSprite;
    public SpriteRenderer titleSprite;

    public Sprite lightOnSprite;
    public Sprite lightOffSprite;

    public Sprite titleNormal;
    public Sprite titleDark;
    
    // Start is called before the first frame update
    void Start()
    {
        backVector = transform.localScale;
        
        Invoke("SwitchOff", 0.1f);
    }

    // Update is called once per frame
    void Update()
    {
       
    }

    private void SwitchOn()
    {
        lightSprite.sprite = lightOnSprite;
        titleSprite.sprite = titleNormal;
        transform.localScale = backVector;
        Invoke("SwitchOff", Random.Range(0.03f,0.2f));
    }
    
    private void SwitchOff()
    {
        lightSprite.sprite = lightOffSprite;
        titleSprite.sprite = titleDark;
        transform.localScale = new Vector3(backVector.x * sizeFactor, backVector.y * sizeFactor, backVector.z * sizeFactor);
        Invoke("SwitchOn", Random.Range(0.03f,0.2f));
    }
}
