using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Class with method to call the SceneHandler and load the previous scene.
/// </summary>
public class SceneReturnScript : MonoBehaviour 
{
	void Update () 
	{
		//Press Escape to go back
		if (Input.GetKeyDown (KeyCode.Escape)) 
		{
            float fadeTime = GameObject.Find("Canvas").GetComponent<SceneFade>().BeginFade(1);
            Wait(fadeTime);
			SceneHandler.GoBack ();
		}
	}

    IEnumerator Wait(float time)
    {
        yield return new WaitForSeconds(time);
    }
}
