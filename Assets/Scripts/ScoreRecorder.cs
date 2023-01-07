using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScoreRecorder : MonoBehaviour
{
    public int score=0;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    public void Add(){
        score +=20;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
