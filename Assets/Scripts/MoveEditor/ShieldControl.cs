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
		transform.localScale = new Vector3 (previousScale.x, 0.5f + 0.015f * move.GetStrength (), previousScale.z);
		Vector3 previousPosition = transform.localPosition;
		transform.localPosition = new Vector3 (previousPosition.x, initialPosY + 0.015f * move.GetStrength (), previousPosition.z);
	}
}
