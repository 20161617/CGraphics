using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using UnityStandardAssets.Utility;

[System.Serializable]
public class Anim
{
    public AnimationClip idle;
    public AnimationClip run;
    // public AnimationClip runForward;
    //public AnimationClip runBackward;
    //public AnimationClip runRight;
    //public AnimationClip runLeft;
}
public class PlayerCtrl : MonoBehaviour
{
    private float h = 0.0f;
    private float v = 0.0f;
    private Transform tr;
    public float moveSpeed = 10.0f;
    public float rotSpeed = 100.0f;

    public Anim anim;
    private Animation _animation;


    // Start is called before the first frame update
    void Start()
    {

        _animation = GetComponentInChildren<Animation>();
        _animation.clip = anim.idle;
        _animation.Play();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
