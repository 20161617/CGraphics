using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Anims
{
    public AnimationClip idle;
    public AnimationClip run;
    // public AnimationClip runBackward;
    //public AnimationClip runRight;
    //public AnimationClip runLeft;
}

public class testScript : MonoBehaviour
{
    private float h = 0.0f;
    private float v = 0.0f;
    public float moveSpeed = 10.0f;
    public float rotSpeed = 100.0f;
    public Anims anim;
    private Animation _animation;
    private Transform tr;

    // Start is called before the first frame update
    void Start()
    {
        _animation = GetComponent<Animation>();
        _animation.clip = anim.idle;
        _animation.Play();
    }

    // Update is called once per frame
    void Update()
    {
        _animation.CrossFade(anim.idle.name, 0.3f);
    }
}
