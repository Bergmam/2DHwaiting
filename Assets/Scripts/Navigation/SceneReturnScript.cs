using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SceneReturnScript : MonoBehaviour {

	void Update () {
		//Press Escape to go back
		if (Input.GetKeyDown (KeyCode.Escape)) {
			SceneHandler.GoBack ();
		}
	}
}
