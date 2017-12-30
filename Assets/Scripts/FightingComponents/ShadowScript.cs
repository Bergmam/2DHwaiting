using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShadowScript : MonoBehaviour {

	private float height;
	private SpriteRenderer spriteRenderer;

	void Awake()
	{
		this.spriteRenderer = gameObject.GetComponent<SpriteRenderer> ();
	}

	void Update ()
	{
		if (Mathf.Abs (transform.position.y - height) > 0.01f) //Corect shadow height if it is too far from ground.
		{
			float x = transform.position.x;
			float z = transform.position.z;
			Vector3 newPosition = new Vector3 (x, height, z);
			transform.position = newPosition;
		}

		// Make sure shadow is not moved to front when character performs a move. Shadows should always be behind both characters.
		if (!spriteRenderer.sortingLayerName.Equals ("Character")) {
			spriteRenderer.sortingLayerName = "Character";
		}
	}

	public void SetHeight(float height){
		this.height = height;
	}
}
