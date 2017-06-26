using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovePlayer : MonoBehaviour {

	private bool isPlaying;
	Move moveToPlay;

	// Use this for initialization
	void Start() {

	}

	// Update is called once per frame
	void Update() {
		
	}

	public void PlayMove(Move move)
	{
		moveToPlay = move;
		isPlaying = true;
	}

	public Frame BlendFrames(Frame fromFrame, Frame toFrame, int percentage)
	{
		List<string> fromFrameNames = fromFrame.getBodyPartNames ();
		List<string> toFrameNames = toFrame.getBodyPartNames ();
		Frame newFrame = fromFrame;

		foreach (string name in fromFrameNames) 
		{
			if (toFrameNames.Contains (name)) 
			{
				float newRotation = fromFrame.getRotation(name) + (float)percentage/(float)100 * (toFrame.getRotation(name) - fromFrame.getRotation(name));
				newFrame.AddBodyPartRotation (name, newRotation);
			}
		}
		return newFrame;
	}
}