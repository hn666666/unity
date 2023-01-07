using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FirstController : MonoBehaviour
{
    public bool state=true;
    public ScoreRecorder scoreRecorder;

    public GameObject player;

    public Vector3 getPlayerPosition()
    {
        return player.transform.localPosition;
    }

    public bool getState()
    {
        return player;
    }
    // Start is called before the first frame update
    void Start()
    {
        
    }



    // Update is called once per frame
    void Update()
    {
        
    }
}
