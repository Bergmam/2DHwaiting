using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class ProgressBarBehaviour : MonoBehaviour {

	private int barHeigh = 20;
	private int barWidth = 400;
	private int totalNumberOfFrames = 10;
	private int currentNumberOfFrames = 0;

	public void SetCurrentNumberOfFrames(int nbr){
		currentNumberOfFrames = nbr;
	}

	public void SetTotalNumberOfFrames(int nbr){
		currentNumberOfFrames = nbr;
	}

	void OnGUI(){
		float positionX = Screen.width / 2 - barWidth / 2;
		float positionY = 0.8f * Screen.height - barHeigh / 2;
		float percentageOfFrames = (float)currentNumberOfFrames / (float)totalNumberOfFrames;

		GUI.BeginGroup (new Rect (positionX, positionY, barWidth, barHeigh));
			GUIStyle filledStyle = OneColorStyle (Color.green);
			GUIStyle backgroundStyle = OneColorStyle (Color.grey);
			GUI.Box (new Rect (0, 0, barWidth, barHeigh), "");

			GUI.BeginGroup (new Rect (3f, 3f, barWidth - 6f, barHeigh - 6f));
				GUI.Box (new Rect (0,0, barWidth * percentageOfFrames - 6f, barHeigh - 6f), "", filledStyle);
			GUI.EndGroup ();

		GUI.EndGroup ();
	}

	private GUIStyle OneColorStyle(Color color){
		GUIStyle style = new GUIStyle ();
		Texture2D fillTexture = new Texture2D (1, 1);
		fillTexture.SetPixel (0, 0, color);
		fillTexture.Apply();
		style.normal.background = fillTexture;
		return style;
	}
}
