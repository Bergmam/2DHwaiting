using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShieldControl : MonoBehaviour {

	private float initialPosY;

	void Start()
	{
		this.initialPosY = transform.localPosition.y;
	}

	/// <summary>
	/// Updates the scale and position of a shield to match a move.
	/// </summary>
	/// <param name="move">Move.</param>
	public void UpdateScale(Move move)
	{
		//TODO: Remove magic numbers
		Vector3 previousScale = transform.localScale;
		float newScale = 0.25f + (1f - 0.25f) * move.GetStrength () / 100f; // min + (max-min) * percentage. Makes scale go [0.25 - 1.0].
		transform.localScale = new Vector3 (previousScale.x, newScale, previousScale.z);
	}
}
