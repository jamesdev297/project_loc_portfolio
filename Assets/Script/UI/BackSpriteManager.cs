using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BackSpriteManager : MonoBehaviour
{
    
    public GameObject leftTop;
    public GameObject backSprite;

    private float backSpriteGap = 340.0f;
    public Color myColor;
    // Start is called before the first frame update
    void Start()
    {
        for (int y = 1; y < Screen.height / (backSpriteGap * 0.2f); y++)
        {
            float xOffset = 0.0f;
            if (y % 2 == 0)
            {
                xOffset = -backSpriteGap*0.5f;
            }
            for (int x = 0; x < (Screen.width * 1.6f) / (backSpriteGap* 0.3f); x++)
            {
                GameObject o = Instantiate(backSprite, leftTop.transform, false);
                o.GetComponent<SpriteRenderer>().color = myColor;
                o.transform.localPosition = new Vector3(x * backSpriteGap + xOffset, 50.0f -y * backSpriteGap*0.6f, 0.0f);
                o.transform.localScale = new Vector3(40.0f, 40.0f, 40.0f);
            }
        }
        
        InvokeRepeating("SpawnBackSprite", 0.0f, 1.0f);
    }

    void SpawnBackSprite()
    {
        for (int x = 0; x < (Screen.width * 1.6f) / (backSpriteGap* 0.3f); x++)
        {
            GameObject o = Instantiate(backSprite, leftTop.transform, false);
            o.GetComponent<SpriteRenderer>().color = myColor;
            o.transform.localPosition = new Vector3(x * backSpriteGap + backSpriteGap*0.5f, 50.0f, 0.0f);
            o.transform.localScale = new Vector3(40.0f, 40.0f, 40.0f);
        }
    }
    // Update is called once per frame
    void Update()
    {
        
    }
}
