using System;
using UnityEngine;
 
public class CameraFollow : MonoBehaviour
{
    public float dampTime = 0.15f;
    private Vector3 velocity = Vector3.zero;
    public Transform target;

    [SerializeField] public Vector2 verticalBound = new Vector2(0.0f, 0.0f);
    [SerializeField] public Vector2 horizontalBound = new Vector2(-5.0f, 5.0f);
    

    // Update is called once per frame
    void Update () 
    {
        if (target)
        {
            Vector3 point = Camera.main.WorldToViewportPoint(target.position);
            Vector3 delta = target.position - Camera.main.ViewportToWorldPoint(new Vector3(0.5f, 0.5f, point.z)); //(new Vector3(0.5, 0.5, point.z));
            Vector3 destination = transform.position + delta;
            destination.x = Mathf.Max(horizontalBound.x, Mathf.Min(horizontalBound.y, destination.x));
            destination.y = Mathf.Max(verticalBound.x, Mathf.Min(verticalBound.y, destination.y));
            transform.position = Vector3.SmoothDamp(transform.position, destination, ref velocity, dampTime);
        }
     
    }
}