using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShadowScript : MonoBehaviour {

	private float height;

	void Update ()
	{
		if (Mathf.Abs (transform.position.y - height) > 0.01f) //Corect shadow height if it is too far from ground.
		{
			float x = transform.position.x;
			float z = transform.position.z;
			Vector3 newPosition = new Vector3 (x, height, z);
			transform.position = newPosition;
		}
	}

	public void SetHeight(float height){
		this.height = height;
	}
}
