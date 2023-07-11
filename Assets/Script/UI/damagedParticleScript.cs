using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class damagedParticleScript : MonoBehaviour
{
    private SpriteRenderer _spriteRenderer;
    private float fadeOutTime = 0.65f;
    // Start is called before the first frame update
    void Start()
    {
        _spriteRenderer = GetComponent<SpriteRenderer>();
        Destroy(gameObject, 1.0f);
        StartCoroutine(FadeOut());
    }

    // Update is called once per frame
    void Update()
    {
    }

    IEnumerator FadeOut()
    {
        float elapsedTime = 0.0f;
        Color c = _spriteRenderer.color;
        while (elapsedTime < fadeOutTime)
        {
            yield return null;
            elapsedTime += Time.deltaTime;
            c.a = 1.0f - ChampionStatusController.EaseInQuart(0.0f, 1.0f, elapsedTime/fadeOutTime);
            _spriteRenderer.color = c;
        }
    }
}
