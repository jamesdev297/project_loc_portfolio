using UnityEngine;
using Random = UnityEngine.Random;

public class CloudMoveScript : MonoBehaviour
{
    float movespeed = 0.005f;
    private int direction = 1;

    private void Start()
    {
        movespeed = Random.Range(0.005f, 0.01f);

        float random = Random.Range(-1f, 1f);

        if (random > 0)
        {
            direction = 1;
        }
        else
        {
            direction = -1;
        }
    }

    void Update()
    {
        transform.Translate(direction * movespeed * Time.deltaTime * 10f, 0, 0);

        if (transform.localPosition.x > 1000 || transform.localPosition.x < -1000)
        {
            direction = -1 * direction;
        }
    }
}
