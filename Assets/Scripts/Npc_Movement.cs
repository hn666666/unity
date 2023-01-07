using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Npc_Movement : MonoBehaviour
{
    // Start is called before the first frame update
    public FirstController sceneController;

    public Vector3 target;
    void Start()
    {
        sceneController = this.GetComponent<FirstController>();//获得
    }

    // Update is called once per frame
    void Update () {
        if(sceneController.getState()) {
            target = sceneController.getPlayerPosition();
            // 追踪玩家
            UnityEngine.AI.NavMeshAgent agent = GetComponent<UnityEngine.AI.NavMeshAgent>();
            agent.destination = target;
        
        }
        else {
            // 游戏结束，停止寻路
            UnityEngine.AI.NavMeshAgent agent = GetComponent<UnityEngine.AI.NavMeshAgent> ();
            agent.velocity = Vector3.zero;
            agent.ResetPath();
        }
    }
}
