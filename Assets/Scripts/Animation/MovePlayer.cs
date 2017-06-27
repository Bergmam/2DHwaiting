using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Add to top empty object of a character in Unity Hierarchy. Makes it possible for that character to play the animation of a <see cref="Move"/>.
/// </summary>
public class MovePlayer : MonoBehaviour
{
	private bool isPlaying;
	private Move moveToPlay;
	private List<Frame> frames;
	int frameIndex = 0;
	private Dictionary<string,Transform> nameBodyPartDict = new Dictionary<string, Transform> ();
	private bool autoLoop;

	void Start()
	{
		autoLoop = false;
		frames = new List<Frame> ();
		FindRotatables ();
	}

	void Update()
	{
		//While isPlaying is true, a new frame is displayed each update until animation is done playing.
		if (isPlaying)
		{
			if (frameIndex < frames.Count)
			{
				Frame frame = frames [frameIndex];
				FrameToCharacter (frame);
				frameIndex++;
			} else if(autoLoop){
				frameIndex = 0;
			} else
			{
				//Clean up and reset for new move
				frameIndex = 0;
				isPlaying = false;
				frames.Clear ();
			}
		}
	}

	/// <summary>
	/// Calculates all frames between the recorded key frames to create a complete animation.
	/// </summary>
	/// <param name="move">The move to be displayed.</param>
	public void PlayMove(Move move)
	{
		moveToPlay = move;
		int speed = move.GetSpeed ();
		Frame[] moveFrames = move.GetFrames ();
		for (int i = 0; i < (moveFrames.Length - 1); i++)
		{
			Frame firstFrame = moveFrames [i];
			Frame secondFrame = moveFrames [i + 1];
			for (int p = 0; p <= 100; p += speed)
			{
				Frame newFrame = BlendFrames (firstFrame, secondFrame, p);
				frames.Add (newFrame);
			}
		}
		frameIndex = 0; //Make sure the animation starts from the first frame.
		isPlaying = true;
	}

	/// <summary>
	/// Blends two frames to create a new frame.
	/// </summary>
	/// <returns>The new blended frame.</returns>
	/// <param name="fromFrame">The first frame.</param>
	/// <param name="toFrame">The second frame.</param>
	/// <param name="percentage">The percentage to add from the second frame to the first frame.
	/// At 0 percent, the blended frame is identical to the first, and 100 it is identical to the secont</param>
	public Frame BlendFrames(Frame fromFrame, Frame toFrame, int percentage)
	{
		List<string> bodyPartNames = fromFrame.getBodyPartNames ();
		Frame newFrame = (Frame) fromFrame.Clone ();

		foreach (string name in bodyPartNames) 
		{
			float newRotation;
			float fromRot = fromFrame.getRotation (name);
			float toRot = toFrame.getRotation (name);
			//Make sure rotation around zero does not result in a loop in the wrong direction.
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

	/// <summary>
	/// Map a frame to the body parts of a character
	/// </summary>
	/// <param name="frame">The frame to map.</param>
	public void FrameToCharacter(Frame frame)
	{
		List<string> bodyPartNames = frame.getBodyPartNames ();
		foreach (string bodyPartName in bodyPartNames)
		{
			Transform bodyPart = nameBodyPartDict [bodyPartName];
			float rotation = frame.getRotation (bodyPartName);
			float currentXRot = bodyPart.localEulerAngles.x;
			float currentYRot = bodyPart.localEulerAngles.y;
			bodyPart.localEulerAngles = new Vector3 (currentXRot, currentYRot, rotation);
		}
	}

	/// <summary>
	/// Finds all rotatable body parts of the character to which the MovePlayer is attached.
	/// Necessary in order to rotate the correct body parts when a move animation is played.
	/// </summary>
	private void FindRotatables()
	{
		Transform[] children = gameObject.GetComponentsInChildren<Transform> ();
		foreach (Transform child in children)
		{
			GameObject go = child.gameObject;
			string tag = go.tag;
			if (tag.Equals ("Rotatable"))
			{
				nameBodyPartDict.Add (go.name, child);
			}
		}
	}

	public void SetAutoLoopEnabled(bool enabled)
	{
		autoLoop = enabled;
	}
}