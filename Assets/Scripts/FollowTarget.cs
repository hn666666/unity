using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowTarget : MonoBehaviour {
	public GameObject player;
	private Vector3 offset;//it's a vector from player to camera
	//offset can be obtained by transform(camera) - player
	// Use this for initialization
	void Start () {
		offset = transform.position - player.transform.position;
	}
	
	// Update is called once per frame
	void LateUpdate () {
		if(player)
		transform.position = player.transform.position + offset;
	}
}