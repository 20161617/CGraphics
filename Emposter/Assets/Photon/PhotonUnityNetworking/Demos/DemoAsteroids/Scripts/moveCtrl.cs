using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;



public class moveCtrl : MonoBehaviourPunCallbacks, IPunObservable
{
    public enum PlayerState { idle, run };
    public PlayerState playerState = PlayerState.idle;

    private float h = 0.0f;
    private float v = 0.0f;
    public float moveSpeed = 10.0f;
    public float rotSpeed = 100.0f;

    private Animator animator;
    private Transform tr;

    void Start()
    {
        tr = GetComponent<Transform>();
        animator = GetComponent<Animator>();

    }

    void AnimUpdate() //이동에 따른 플레이어 애니메이션 변경 
    {
        if (v >= 0.1f)
        {
            animator.SetBool("IsWalk", true);
        }
        else if (v <= -0.1f)
        {
            animator.SetBool("IsWalk", true);
        }
        else if (h >= 0.1f)
        {
            animator.SetBool("IsWalk", true);
        }
        else if (h <= -0.1f)
        {
            animator.SetBool("IsWalk", true);
        }
        else
        {
            Debug.Log("h :" + h + "  v :" + v);
            playerState = PlayerState.idle;
            animator.SetBool("IsWalk", false);
            // _animation.CrossFade(anim.idle.name, 0.3f);
        }
    }
    void MoveUpdate()
    {
         h = Input.GetAxis("Horizontal");
         v = Input.GetAxis("Vertical");

        //Vector3 moveDir = (Vector3.forward * v) + (Vector3.right * h);
        //tr.Translate(moveDir * moveSpeed * Time.deltaTime, Space.Self);
        //tr.Rotate(Vector3.up * Time.deltaTime * rotSpeed * Input.GetAxis("Mouse X"));

    }
    void Update()
    {
        //controlled locally일 경우 이동(자기 자신의 캐릭터일 때)
        if (photonView.IsMine)
        {
            MoveUpdate();
            AnimUpdate();
        }
        else
        {
            //끊어진 시간이 너무 길 경우(텔레포트)
            if ((tr.position - currPos).sqrMagnitude >= 10.0f * 10.0f)
            {
                tr.position = currPos;
                tr.rotation = currRot;
            }
            //끊어진 시간이 짧을 경우(자연스럽게 연결 - 데드레커닝)
            else
            {
                tr.position = Vector3.Lerp(tr.position, currPos, Time.deltaTime * 10.0f);
                tr.rotation = Quaternion.Slerp(tr.rotation, currRot, Time.deltaTime * 10.0f);
            }
        }

    }

    //클론이 통신을 받는 변수 설정
    private Vector3 currPos;
    private Quaternion currRot;

    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        //통신을 보내는 
        if (stream.IsWriting)
        {
            stream.SendNext(tr.position);
            stream.SendNext(tr.rotation);
        }

        //클론이 통신을 받는 
        else
        {
            currPos = (Vector3)stream.ReceiveNext();
            currRot = (Quaternion)stream.ReceiveNext();
        }
    }
}

