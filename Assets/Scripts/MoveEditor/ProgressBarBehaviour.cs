using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

/// <summary>
/// Class for handling the behaviour of the progress bar in the MoveEditor.
/// </summary>
public class ProgressBarBehaviour : MonoBehaviour 
{
	private float totalNrOfFrames = 0;
	private float currentNrOfFrames = 0;
	private RectTransform fill;
	private Vector2 twoByTwo;

	void Start()
	{
		this.fill = transform.FindChild ("ProgressBarFill").gameObject.GetComponent<RectTransform>();
		this.twoByTwo = new Vector2 (2, 2);
	}

	public void SetTotalNbrOfFrames(int nbr)
	{
		totalNrOfFrames = (float) nbr;
	}

	public void SetCurrentNbrOfFrames(int nbr)
	{
		currentNrOfFrames = (float) nbr;
		UpdateFill ();
	}

	public void IncrementNbrOfFrames ()
	{
		if (currentNrOfFrames < totalNrOfFrames) 
		{
			currentNrOfFrames++;
		}
		UpdateFill ();
	}

	public void DecrementNbrOfFrames ()
	{
		if (currentNrOfFrames > 0) 
		{
			currentNrOfFrames--;
		}
	}

	public int GetCurrentNbrOfFrames()
	{
		return (int) Math.Round(currentNrOfFrames);
	}

	/// <summary>
	/// Updates the fill to show the number of recorded frames.
	/// </summary>
	private void UpdateFill()
	{
		//Bounds = bot = 0, top = 1, left = 0, right = percentage of total frames recorded
		fill.anchorMin = Vector2.zero;
		fill.anchorMax = new Vector2 (currentNrOfFrames / totalNrOfFrames, 1);
		//Create a border of unit length 2 around fill
		fill.offsetMax = twoByTwo;
		fill.offsetMin = twoByTwo;
		//Make sure fill is placed in middle of anchor
		fill.anchoredPosition = Vector2.zero;
	}
}
