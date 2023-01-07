using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tank_Movement : MonoBehaviour
{
    public float speed = 5;//坦克移动速度
    public float angularSpeed = 10;//坦克旋转速度
    public AudioClip idleAudio;//静止声音
    public AudioClip drivingAudio;//发动声音
    private new AudioSource audio;//声音组件
    private new Rigidbody rigidbody;
    // Start is called before the first frame update
    void Start()
    {
        rigidbody = this.GetComponent<Rigidbody>();//获得刚体组件
        audio = this.GetComponent<AudioSource>();//获得声音组件
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        float v = Input.GetAxis("Vertical");//对应键盘上下箭头，按下触发
        rigidbody.velocity = transform.forward*v*speed;//利用刚体加速度

        float h = Input.GetAxis("Horizontal");//对应键盘左右箭头，按下触发
        rigidbody.angularVelocity = transform.up*h*angularSpeed;

        if(Mathf.Abs(h) > 0.1||Mathf.Abs(v)>0.1){
            audio.clip = drivingAudio;
            if(audio.isPlaying==false)
                audio.Play();
        }
        else{
            audio.clip = idleAudio;
            if(audio.isPlaying==false)
                audio.Play();
        }
    }
}
