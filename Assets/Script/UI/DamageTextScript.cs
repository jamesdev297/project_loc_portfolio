using System.Collections;
using UnityEngine;

public class DamageTextScript : MonoBehaviour
{
    private TextMesh _text;
    private float fadeOutTime = 0.4f;
    // Start is called before the first frame update
    void Start()
    {
        _text = GetComponent<TextMesh>();
        MeshRenderer meshRenderer = GetComponent<MeshRenderer>();
        meshRenderer.sortingOrder = 3;
        Destroy(gameObject, 2.0f);
        StartCoroutine(FadeOut());
    }

    // Update is called once per frame
    void Update()
    {
        transform.Translate(Vector3.up * Time.deltaTime * 1.7f);
    }
    
    IEnumerator FadeOut()
    {
        yield return new WaitForSeconds(0.25f);
        float elapsedTime = 0.0f;
        Color c = _text.color;
        while (elapsedTime < fadeOutTime)
        {
            yield return null;
            elapsedTime += Time.deltaTime;
            c.a = 1.0f - ChampionStatusController.EaseInQuart(0.0f, 1.0f, elapsedTime/fadeOutTime);
            _text.color = c;
        }
    }
}
