using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

/// <summary>
/// Class for handling the behaviour of the progress bar in the MoveEditor.
/// </summary>
public class ProgressBarBehaviour : MonoBehaviour 
{

	// Size of the bar
	private int barHeigh = 20;
	private int barWidth = 400;
	private int totalNrOfFrames = 0;
	private int currentNrOfFrames = 0;

	public void SetTotalNbrOfFrames(int nbr)
	{
		totalNrOfFrames = nbr;
	}

	public void IncrementNbrOfFrames ()
	{
		if (currentNrOfFrames < totalNrOfFrames) 
		{
			currentNrOfFrames++;
		}
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
		return currentNrOfFrames;
	}
		
	/// <summary>
	/// Draws the progress bar.
	/// </summary>
	void OnGUI()
	{
		float positionX = Screen.width / 2 - barWidth / 2;
		float positionY = 0.8f * Screen.height - barHeigh / 2;
		float percentageOfFrames = (float)currentNrOfFrames / (float)totalNrOfFrames;

		// Draws a new rectangle 
		GUI.BeginGroup (new Rect (positionX, positionY, barWidth, barHeigh));
			GUIStyle filledStyle = OneColorStyle (Color.green);
			GUIStyle backgroundStyle = OneColorStyle (Color.grey);
			GUI.Box (new Rect (0, 0, barWidth, barHeigh), "");

			GUI.BeginGroup (new Rect (3f, 3f, barWidth - 6f, barHeigh - 6f));
				GUI.Box (new Rect (0,0, barWidth * percentageOfFrames - 6f, barHeigh - 6f), "", filledStyle);
			GUI.EndGroup ();

		GUI.EndGroup ();
	}
		
	/// <summary>
	/// Method to change the color of the progess bar when it is updated.
	/// </summary>
	/// <param name="color">The color of the bar to be drawn.</param>
	private GUIStyle OneColorStyle(Color color)
	{
		GUIStyle style = new GUIStyle ();
		Texture2D fillTexture = new Texture2D (1, 1);
		fillTexture.SetPixel (0, 0, color);
		fillTexture.Apply();
		style.normal.background = fillTexture;
		return style;
	}
}
