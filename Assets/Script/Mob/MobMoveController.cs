
using System;
using System.Collections;
using Pathfinding;
using Script;
using UnityEngine;
using UnityEngine.UI;
using Random = UnityEngine.Random;

public class MobMoveController : MoveController
{
    public MobStatusController statusController;
    [SerializeField] private float nextWayPointDistance = 0.01f;

    public int currentWaypoint = 0;
    public bool reachedEndOfPath = false;
    public Path path;
    public Transform groundLT;
    public Transform groundRB;
  
    private void Start()
    {
        childLocalScale = transform.GetChild(0).localScale;
        childTransform = transform.GetChild(0);
        
        groundLT = GameObject.Find("GroundLT").transform;
        groundRB = GameObject.Find("GroundRB").transform;
        
        footCollider = Instantiate(footColliderPrefab).transform;

        statusController = GetComponent<MobStatusController>();
    }

    public void OnPathComplete(Path p)
    {
        if (!p.error)
        {
            path = p;
            currentWaypoint = 0;
        }
    }

    
    public void GoToWaypoint()
    {
        if (!statusController.championBehavior.isMoveEnable)
        {
            return;
        }
        if (path == null)
        {
            return;
        }

        if (currentWaypoint >= path.vectorPath.Count)
        {
            Debug.Log("REACHED!");
            reachedEndOfPath = true;
            if (Random.Range(0f, 1.0f) > 0.4)
            {
                statusController.updateNewState(statusController.mobAINoneState);
            }
            else
            {
                statusController.updateNewState(statusController.mobAIDistance);
            }
            return;
        }
        else
        {
            reachedEndOfPath = false;
        }

        Vector2 direction = (path.vectorPath[currentWaypoint] - transform.position).normalized;
        Vector2 force = direction * speed * 1.0f * Time.deltaTime;
        force.y = force.y * 0.7f;
       

        statusController.animator.SetFloat(Constants.moveMagnitude, 1.0f);
        transform.Translate(force);

        if (statusController.championBehavior.isJump)
        {
            footCollider.transform.position = new Vector3(transform.position.x, lastY, 0.0f);
        }
        else
        {
            footCollider.transform.position = transform.position;
            lastY = transform.position.y;
        }
        
        // Debug.Log(direction.x);

        if (isLookTarget && statusController.target != null)
        {
            if (transform.position.x < statusController.target.position.x)
            {
                transform.GetChild(0).localScale = childLocalScale;
            }
            else
            {
                transform.GetChild(0).localScale = new Vector3(childLocalScale.x * -1, childLocalScale.y, childLocalScale.z);
            }
        }
        else
        {
            if (direction.x > 0.2f)
            {
                transform.GetChild(0).localScale = childLocalScale;
            }else if (direction.x < -0.2f)
            {
                transform.GetChild(0).localScale = new Vector3(childLocalScale.x * -1, childLocalScale.y, childLocalScale.z);
            }
        }
        
        float distance = Vector2.Distance(transform.position, path.vectorPath[currentWaypoint]);
        if (distance < nextWayPointDistance)
        {
            currentWaypoint++;
        }
    }
    
    public void LateUpdate()
    {
        
        Vector3 oldPos = childTransform.position;
        Vector3 newPos = new Vector3(oldPos.x, oldPos.y, oldPos.z);
        if (transform.position.x < groundLT.position.x)
        {
            newPos.x = groundLT.position.x;
        }
        if (footCollider.position.y > groundLT.position.y)
        {
            newPos.y = groundLT.position.y;
        }
        if (transform.position.x > groundRB.position.x)
        {
            newPos.x = groundRB.position.x;
        }
        if (footCollider.position.y < groundRB.position.y)
        {
            newPos.y = groundRB.position.y;
        }
        transform.position = newPos;
    }
}
