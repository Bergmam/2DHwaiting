using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

/// <summary>
/// Class for handling the behaviour of the progress bar in the MoveEditor.
/// </summary>
public class ProgressBarBehaviour : MonoBehaviour 
{
	private RectTransform fill;
	private Vector2 twoByTwo;

	void Awake()
	{
		this.fill = transform.Find ("ProgressBarFill").gameObject.GetComponent<RectTransform>();
		this.twoByTwo = new Vector2 (2, 2);
	}

	/// <summary>
	/// Updates the fill to show the number of recorded frames.
	/// </summary>
	public void UpdateFill(float progress)
	{
		//Bounds = bot = 0, top = 1, left = 0, right = percentage of total frames recorded
		fill.anchorMin = Vector2.zero;
		fill.anchorMax = new Vector2 (progress, 1);
		//Create a border of unit length 2 around fill
		fill.offsetMax = twoByTwo;
		fill.offsetMin = twoByTwo;
		//Make sure fill is placed in middle of anchor
		fill.anchoredPosition = Vector2.zero;
	}
}
