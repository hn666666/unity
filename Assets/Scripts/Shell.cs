using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shell : MonoBehaviour
{
    public GameObject shellExplosionPrefab;
    public AudioClip shellExplosionAudio;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void OnTriggerEnter( Collider collider){//子弹碰撞触发器
        AudioSource.PlayClipAtPoint(shellExplosionAudio, transform.position);
        GameObject.Instantiate(shellExplosionPrefab, transform.position, transform.rotation);//实例化爆炸效果
        GameObject.Destroy(this.gameObject);//删除子弹
        if(collider.tag == "Player"){//NPC发射子弹碰撞到玩家时
            collider.SendMessage("TankDamage");//给坦克发送消息 调用Tank里面函数
        }
        else if(this.name=="Player" && collider.tag=="Enemy"){//玩家发射子弹碰撞到NPC
            collider.SendMessage("TankDamage");
        }
    }
}
