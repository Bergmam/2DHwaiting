using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovePlayer : MonoBehaviour {

	private bool isPlaying;
	private Move moveToPlay;

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
		List<string> bodyPartNames = fromFrame.getBodyPartNames ();
		Frame newFrame = fromFrame;

		foreach (string name in bodyPartNames) 
		{
			float newRotation;
			float fromRot = fromFrame.getRotation (name);
			float toRot = toFrame.getRotation (name);
			if (toRot < fromRot && RotationUtils.InLimits (0, fromRot, toRot)) 
			{
				toRot = toRot + 360;
			} 
			if (fromRot < toRot && RotationUtils.InLimits (0, toRot, fromRot))
			{
				fromRot = fromRot + 360;
			}
			newRotation = fromRot + (float)percentage/(float)100 * (toRot- fromRot);
			newFrame.AddBodyPartRotation (name, newRotation);
		}
		return newFrame;
	}
}