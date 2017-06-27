using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Class with method to change the scene if the specified scene exists
/// </summary>
public class SceneButtonBehaviour : MonoBehaviour {
	public void SwitchScene(string sceneName){
		SceneHandler.SwitchScene (sceneName);
	}
}
