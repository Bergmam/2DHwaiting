using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SceneButtonBehaviour : MonoBehaviour {

	//Open scene if it exists
	public void SwitchScene(string sceneName){
		SceneHandler.SwitchScene (sceneName);
	}
}
