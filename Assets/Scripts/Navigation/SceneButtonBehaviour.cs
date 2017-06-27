using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Class to put on buttons that will change the scene. 
/// The method in the class calls the SceneHandler to switch the scene.
/// </summary>
public class SceneButtonBehaviour : MonoBehaviour 
{
	public void SwitchScene(string sceneName)
	{
		SceneHandler.SwitchScene (sceneName);
	}
}
