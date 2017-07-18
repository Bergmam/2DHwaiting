using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Class with method to call the SceneHandler and load the previous scene.
/// If a promt is to be displayed before going back, it can be set in the editor.
/// </summary>
public class SceneReturnScript : MonoBehaviour 
{
	public GameObject promptPanel; //Set in editor if exists for the current scene. Leave null otherwise.

	void Start()
	{
		if (promptPanel != null)
		{
			promptPanel.SetActive (false);
		}
	}

	void Update () 
	{
		//Press Escape to go back
		if (Input.GetKeyDown (KeyCode.Escape)) 
		{
			if (promptPanel == null) 
			{
				GoBack ();
			}
			else
			{
				promptPanel.SetActive (!promptPanel.activeSelf); //Hide panel again if pressing escape while it is active.
			}
		}
	}

	public void GoBack()
	{
		HidePrompt ();
		SceneHandler.GoBack ();
	}

	public void HidePrompt()
	{
		if (promptPanel != null)
		{
			promptPanel.SetActive (false);
		}
	}
}
