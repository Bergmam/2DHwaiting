using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Move {

	private float speed;
	private float strength;
	private Frame[] frames;
	private int nextIndex;
	private bool done;
	private int defaultNbrOfFrames = 5; //C# requires default constructor. Here this number is used to create the frame array.

	public Move(){
		frames = new Frame[defaultNbrOfFrames];
	}

	public Move(int nbrOfFrames){
		frames = new Frame[nbrOfFrames]; //TODO: Should the length of a move be adjustable? Maybe it is always the same?
	}

	//Add a frame to the list. Increases nextIndex.
	public void AddFrame(Frame frame){
		if (nextIndex < frames.Length) {
			frames [nextIndex] = frame;
			nextIndex++;
		}
	}

	//Remove a frame from the list. Decreases nextIndex.
	public void RemoveFrame(){
		if (nextIndex > 0) {
			nextIndex--;
			frames [nextIndex] = null;
		}
	}

	public void SetSpeed(int newSpeed){
		if (newSpeed > 0) {
			speed = newSpeed;
		}
	}

	public void SetStrength(int newStrength){
		if (newStrength > 0) {
			strength = newStrength;
		}
	}

	public Frame[] GetFrames(){
		return frames;
	}
}
