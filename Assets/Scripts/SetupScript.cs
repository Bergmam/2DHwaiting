using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SetupScript : MonoBehaviour {

	void Start () {
		SceneHandler.Init (); //Instantiate SceneHandler with list of existing scenes and return stack
	}
}
