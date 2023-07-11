using System;
using Script;
using UnityEngine;
using Photon.Pun;

public class PlayerMoveController : MoveController, IPunObservable
{
    private GameObject player;
    private Vector2 moveControllerVector;
    private MyFloatingJoystick floatingJoystick;
    private Vector2 joystickThreashold;
    public Vector2 speedMagnitude;
    public Transform groundLT;
    public Transform groundRB;
    private Animator animator;
    private GameObject playerManager;
    private PlayerStatusController _playerStatusController;
    

    private void Start()
    {
        if (PhotonNetwork.IsConnected && photonView == null || (photonView != null && (photonView.IsMine ||
                                                                GameManager.instance.isBot)))
        {
            CameraFollow cameraFollow = Camera.main.GetComponent<CameraFollow>();
            cameraFollow.target = transform;
        }
        
        footCollider = Instantiate(footColliderPrefab).transform;
        footRing = Instantiate(footRingPrefab);
        footRing.GetComponent<RangeCircleScript>().target = footCollider;
        
        childLocalScale = transform.GetChild(0).localScale;
        childTransform = transform.GetChild(0);
        
        groundLT = GameObject.Find("GroundLT").transform;
        groundRB = GameObject.Find("GroundRB").transform;
        moveControllerVector = Vector2.zero;
        playerManager = GameObject.Find("PlayerManager");
        if (playerManager.GetComponent<CampaignPlayManager>() != null)
        {
            floatingJoystick = CampaignPlayManager.Instance.floatingJoystick;
            joystickThreashold = CampaignPlayManager.Instance.joystickThreshold;
        }else if (playerManager.GetComponent<PlayerManagerMulti>() != null)
        {
            floatingJoystick = PlayerManagerMulti.Instance.floatingJoystick;
            joystickThreashold = PlayerManagerMulti.Instance.joystickThreshold;
        }
        
        animator = GetComponentInChildren<Animator>();
        speedMagnitude = GetComponent<PlayerStatusController>().speedMagnitude;
        _playerStatusController = GetComponent<PlayerStatusController>();
        
        Debug.Log("TEST" + childLocalScale);
    }

    void moveControl()
    {
        float joystickHorizontal = floatingJoystick.Horizontal;
        float joystickVertical = floatingJoystick.Vertical;

        if (joystickHorizontal > joystickThreashold.x)
        {
            moveControllerVector.x = Time.deltaTime * (speedMagnitude.x - speedMagnitude.x * Mathf.Abs(joystickVertical) * 0.5f);
        }else if(joystickHorizontal < -joystickThreashold.x)
        {
            moveControllerVector.x = -1 * Time.deltaTime * (speedMagnitude.x - speedMagnitude.x * Mathf.Abs(joystickVertical) * 0.5f);
        }
        else
        {
            moveControllerVector.x = 0.0f;
        }
        
        if (joystickVertical > joystickThreashold.y)
        {
            moveControllerVector.y = Time.deltaTime * speedMagnitude.y;
        }else if(joystickVertical < -joystickThreashold.y)
        {
            moveControllerVector.y = -1 * Time.deltaTime * speedMagnitude.y;
        }
        else
        {
            moveControllerVector.y = 0.0f;
        }
    }
    
    private void Update()
    {
        if (!GameManager.instance.isBot && PhotonNetwork.IsConnected && photonView != null && !photonView.IsMine)
            return;

        if (_playerStatusController.championBehavior != null)
        {
            if (_playerStatusController.championBehavior.isJump || _playerStatusController.championBehavior.isRolling)
            {
                footCollider.transform.position = new Vector3(transform.position.x, lastY, 0.0f);
            }
            else
            {
                footCollider.transform.position = transform.position;
                lastY = transform.position.y;
            }
        }
        

        // Debug.Log($"MATT {PhotonNetwork.IsConnected} {photonView} {photonView.IsMine}");
        moveControl();

        if (_playerStatusController.championBehavior != null)
        {
            if (_playerStatusController.championBehavior.isMoveEnable && !_playerStatusController.championBehavior.isDie)
            {
                if (isLookTarget && _playerStatusController.target)
                {
                    if (transform.position.x < _playerStatusController.target.position.x)
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
                    if (moveControllerVector.x > 0.0f)
                    {
                        childTransform.localScale = childLocalScale;  
                    }else if (moveControllerVector.x < 0.0f)
                    {
                        childTransform.localScale = new Vector3(childLocalScale.x * -1, childLocalScale.y, childLocalScale.z);  
                    }
                }
           

                Vector3 dir = moveControllerVector.normalized * speed * Time.deltaTime;
                dir.y = dir.y * 0.7f;
            
                transform.Translate(dir);
            }
        }
    }

    private void LateUpdate()
    {
        if (!GameManager.instance.isBot && PhotonNetwork.IsConnected && photonView != null && !photonView.IsMine)
            return;
        
        if (Mathf.Abs(moveControllerVector.magnitude)*10.0f > 0.0f && _playerStatusController.championBehavior.isMoveEnable && !_playerStatusController.championBehavior.isDie)
        {
            animator.SetFloat("moveMagnitude", 1.0f);
        }
        else
        {
            animator.SetFloat("moveMagnitude", 0.0f);
        }
        
        Vector3 oldPos = childTransform.position;
        Vector3 newPos = new Vector3(oldPos.x, oldPos.y, oldPos.z);
        if (transform.position.x < groundLT.position.x)
        {
            newPos.x = groundLT.position.x + 0.1f;
        }
        if (footCollider.position.y > groundLT.position.y)
        {
            newPos.y = groundLT.position.y;
        }
        if (transform.position.x > groundRB.position.x)
        {
            newPos.x = groundRB.position.x - 0.1f;
        }
        if (footCollider.position.y < groundRB.position.y)
        {
            newPos.y = groundRB.position.y;
        }
        transform.position = newPos;
    }
    
    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {

    }
}