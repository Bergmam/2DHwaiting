using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Class handling the instantiation of the SceneHandler.
/// </summary>
public class SetupScript : MonoBehaviour 
{
	void Start () 
	{
		SceneHandler.Init (); //Instantiate SceneHandler with list of existing scenes and return stack
        SaveLoad.Load();
		StaticCharacterHolder.Init ();
		InputSettings.Init ();
	}
}
