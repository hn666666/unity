using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tank_Attack : MonoBehaviour
{
    public GameObject shellPrefab;//子弹预制体
    public KeyCode fireKey = KeyCode.Space;//发射子弹键盘按键
    public float shellSpeed = 10;//子弹发射速度
    public AudioClip shotAudio;
    private Transform firePoint;
    // Start is called before the first frame update
    void Start()
    {
        firePoint = transform.Find("FirePoint");//找到发射点
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyDown(fireKey)){//空格键按下后
            AudioSource.PlayClipAtPoint(shotAudio, transform.position);
            GameObject go = GameObject.Instantiate(shellPrefab, firePoint.position, firePoint.rotation);//在发射点位置实例化子弹
            go.GetComponent<Rigidbody>().velocity = go.transform.forward*shellSpeed;
            go.name = "Player";
        }
    }
}
