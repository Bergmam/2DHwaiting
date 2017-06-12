using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
﻿using System.IO;
using System;

public class SceneHandler{

	private static Stack sceneStack;
	private static AssetBundle myLoadedAssetBundle;
	private static string[] scenePaths;
	private static ArrayList scenePathsList;

	public static void Init(){
		scenePathsList = new ArrayList ();
		sceneStack = new Stack ();

		//String path of folder containing all scenes
		string sceneDirectoryPath = "Assets" + Path.DirectorySeparatorChar + "Scenes";
		scenePaths = Directory.GetFiles (sceneDirectoryPath);

		//Add all scene names to list scenePathsList
		for (int i = 0; i < scenePaths.Length; i++) {
			string path = scenePaths [i];
            Debug.Log(path);
			path = path.Split (Path.DirectorySeparatorChar) [2].ToString (); //Remove begining of path
			path = path.Split ('.') [0].ToString (); //Remove file ending
			if (!scenePathsList.Contains (path)) {
				scenePathsList.Add (path);
			}
		}
	}

	//Open scene if it exists and add the current scene to stack
	public static void SwitchScene(string sceneName){
		if (scenePathsList.Contains (sceneName)) {
			sceneStack.Push (SceneManager.GetActiveScene().name);
			SceneManager.LoadScene (sceneName);
		}
	}

	//If stack is non empty, the top scene is loaded
	public static void GoBack(){
		if (sceneStack.Count > 0) {
			string previousSceneName = (string)sceneStack.Pop ();
			if (scenePathsList.Contains (previousSceneName)) {
				SceneManager.LoadScene (previousSceneName);
			}
		}
	}
}
