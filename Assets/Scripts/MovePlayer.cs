using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovePlayer : MonoBehaviour {

	private bool isPlaying;
	Move moveToPlay;

	// Use this for initialization
	void Start() {

	}

	// Update is called once per frame
	void Update() {
		
	}

	public void PlayMove(Move move)
	{
		moveToPlay = move;
		isPlaying = true;
	}

}