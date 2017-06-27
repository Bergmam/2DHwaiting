using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovePlayer : MonoBehaviour {

	private bool isPlaying;
	private Move moveToPlay;
	private List<Frame> frames;
	int frameIndex = 0;
	private Dictionary<string,Transform> nameBodyPartDict = new Dictionary<string, Transform> ();

	// Use this for initialization
	void Start() {
		frames = new List<Frame> ();
		FindRotatables ();
	}

	// Update is called once per frame
	void Update() {
		if (isPlaying) {
			if (frameIndex < frames.Count) {
				Frame frame = frames [frameIndex];
				FrameToCharacter (frame);
				frameIndex++;
			} else {
				//Clean up and reset for new move
				frameIndex = 0;
				isPlaying = false;
				frames.Clear ();
			}
		}
	}

	public void PlayMove(Move move)
	{
		moveToPlay = move;
		int speed = move.GetSpeed ();
		Frame[] moveFrames = move.GetFrames ();
		for (int i = 0; i < (moveFrames.Length - 1); i++) {
			Frame firstFrame = moveFrames [i];
			Frame secondFrame = moveFrames [i + 1];
			for (int p = 0; p <= 100; p += speed) {
				Frame newFrame = BlendFrames (firstFrame, secondFrame, p);
				frames.Add (newFrame);
			}
		}
		frameIndex = 0;
		isPlaying = true;
	}

	public Frame BlendFrames(Frame fromFrame, Frame toFrame, int percentage)
	{
		List<string> bodyPartNames = fromFrame.getBodyPartNames ();
		Frame newFrame = (Frame) fromFrame.Clone ();

		foreach (string name in bodyPartNames) 
		{
			float newRotation;
			float fromRot = fromFrame.getRotation (name);
			float toRot = toFrame.getRotation (name);
			if (toRot < fromRot && RotationUtils.InLimits (0, toRot, fromRot))
			{
				toRot = toRot + 360;
			} 
			if (fromRot < toRot && RotationUtils.InLimits (0, fromRot, toRot))
			{
				fromRot = fromRot + 360;
			}
			newRotation = fromRot + (float)percentage/(float)100 * (toRot- fromRot);
			newFrame.AddBodyPartRotation (name, newRotation);
		}
		return newFrame;
	}
		
	//Map a frame to the body parts of a character
	public void FrameToCharacter(Frame frame)
	{
		List<string> bodyPartNames = frame.getBodyPartNames ();
		foreach (string bodyPartName in bodyPartNames) {
			Transform bodyPart = nameBodyPartDict [bodyPartName];
			float rotation = frame.getRotation (bodyPartName);
			float currentXRot = bodyPart.localEulerAngles.x;
			float currentYRot = bodyPart.localEulerAngles.y;
			bodyPart.localEulerAngles = new Vector3 (currentXRot, currentYRot, rotation);
		}
	}

	private void FindRotatables()
	{
		Transform[] children = gameObject.GetComponentsInChildren<Transform> ();
		foreach (Transform child in children) {
			GameObject go = child.gameObject;
			string tag = go.tag;
			if (tag.Equals ("Rotatable")) {
				nameBodyPartDict.Add (go.name, child);
			}
		}
	}
}