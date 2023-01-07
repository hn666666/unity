using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UserGUI : MonoBehaviour
{
    // Start is called before the first frame update
    GameObject PlayerTank = default;
    public ScoreRecorder score_recorder;
    private GUIStyle labelStyle;

    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void OnGUI() {
	labelStyle = new GUIStyle("label");
	labelStyle.alignment = TextAnchor.MiddleCenter;
	labelStyle.fontSize = Screen.height/15;
	GUI.color = Color.black;
	if(score_recorder.score>=100 && PlayerTank) {
		Debug.Log("YOU WIN!");
		GUI.Label(new Rect(Screen.width/2 - Screen.width/8,Screen.height/2 - Screen.height/8,Screen.width/4,Screen.height/4), "YOU WIN!",labelStyle);
	}
	else if(!PlayerTank) {
		Debug.Log("Game Over!");
		GUI.Label(new Rect(Screen.width/2 - Screen.width/8,Screen.height/2 - Screen.height/8,Screen.width/4,Screen.height/4), "Game Over!",labelStyle);
	}
}
}
