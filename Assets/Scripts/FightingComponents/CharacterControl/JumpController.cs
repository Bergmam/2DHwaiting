using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Handles character jumping.
/// </summary>
public class JumpController : MonoBehaviour {

	private int jumpFrameDelay; // Used to make sure the character does not accidentally jump two frames in a row
	private Rigidbody2D thisBody;

	private float fallMultiplier = 4.0f;

	// Use this for initialization
	void Awake () {
		jumpFrameDelay = 0;
		thisBody = gameObject.GetComponent<Rigidbody2D> ();
	}
	
	// Update is called once per frame
	void Update () {
		if (thisBody.velocity.y < 0) {
			thisBody.velocity += Vector2.up * Physics2D.gravity.y * (fallMultiplier - 1) * Time.deltaTime;
		}

		if (jumpFrameDelay != 0)
		{
			jumpFrameDelay--;
		}
	}

	public void Jump()
	{
		if (Mathf.Abs (thisBody.velocity.y) <= 0.01 && jumpFrameDelay == 0)
		{
			thisBody.AddForce(Vector2.up * Parameters.jumpForce);
			jumpFrameDelay = 3;
		}
	}
}
