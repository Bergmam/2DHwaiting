using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveCreator : MonoBehaviour {

	// Use this for initialization
	void Start () {
		PlaceCharacter (0.5f, 0.5f);
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	private void PlaceCharacter (float x, float y)
	{
		float screenHeight = Screen.height;
		float screenWidth = Screen.width;
		Vector3 characterStartPosition = new Vector3 (x * screenWidth, y * screenHeight, 0f);
		characterStartPosition = Camera.main.ScreenToWorldPoint (characterStartPosition);
		characterStartPosition = new Vector3 (characterStartPosition.x, characterStartPosition.y, 0f);
		transform.position = characterStartPosition;
	}
}
