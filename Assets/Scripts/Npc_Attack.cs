using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Npc_Attack : MonoBehaviour
{
    public GameObject shellPrefab;//子弹预制体
    public float shellSpeed = 10;//子弹发射速度
    private Transform firePoint;
    public Vector3 target;
    public AudioClip shotAudio;
    public FirstController sceneController;
    // Start is called before the first frame update
    void Start()
    {
        firePoint = transform.Find("FirePoint");//找到发射点
        sceneController = this.GetComponent<FirstController>();
        StartCoroutine(shoot());
    }

    // Update is called once per frame
    void Update()
    {
        if(sceneController.player){
            target = sceneController.getPlayerPosition();
        }
    }

    IEnumerator shoot(){ //协程实现npc坦克射击
        while(sceneController.player){
            if(Vector3.Distance(transform.position, target) < 10) {
            Debug.Log("fire");
            yield return new WaitForSeconds(1);
            GameObject go = GameObject.Instantiate(shellPrefab, firePoint.position, firePoint.rotation);//在发射点位置实例化子弹
            go.GetComponent<Rigidbody>().velocity = go.transform.forward*shellSpeed;
            AudioSource.PlayClipAtPoint(shotAudio, transform.position);
            go.name = "NPC";
            yield return 0;
            // yield return null;
            // yield return new WaitForSeconds(1f);//1秒后继续执行for循环
            }
            yield return 0;
        }
    }
}
