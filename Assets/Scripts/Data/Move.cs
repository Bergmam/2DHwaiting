using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Data class holding information of a Move. Consists of a collection of <see cref="Frame"/>'s and stats.
/// </summary>
[System.Serializable]
public class Move
{
	private int speed = 10;
	private float strength;
	private Frame[] frames;
	private int nextIndex; //At which index to place the next recorded frame. Adding or removing frames updates the index.
	private bool done;
	private int defaultNrOfFrames = 10; //C# requires default constructor. In default constructor, this number is used to create the frame array.
    private string name;

	/// <summary>
	/// Initializes a new instance of the <see cref="Move"/> class.
	/// </summary>
	public Move()
	{
		frames = new Frame[defaultNrOfFrames];
	}

	/// <summary>
	/// Initializes a new instance of the <see cref="Move"/> class.
	/// </summary>
	/// <param name="nrOfFrames">Number of frames total in the move.</param>
	public Move(int nrOfFrames)
	{
		frames = new Frame[nrOfFrames]; //TODO: Should the length of a move be adjustable? Maybe it is always the same?
	}

	/// <summary>
	/// Add a frame to the list. Increases nextIndex.
	/// </summary>
	/// <param name="frame">Frame.</param>
	public void AddFrame(Frame frame)
	{
		if (nextIndex < frames.Length)
		{
			frames [nextIndex] = frame;
			nextIndex++;
		}
	}

	/// <summary>
	/// Remove a frame from the list. Decreases nextIndex.
	/// </summary>
	public void RemoveFrame()
	{
		if (nextIndex > 0)
		{
			nextIndex--;
			frames [nextIndex] = null;
		}
	}

	public void SetSpeed(int newSpeed)
	{
		if (newSpeed > 0)
		{
			speed = newSpeed;
		}
	}

	public void SetStrength(int newStrength)
	{
		if (newStrength > 0)
		{
			strength = newStrength;
		}
	}

	public Frame[] GetFrames()
	{
		return frames;
	}

	/// <summary>
	/// Returns the total number of frames this move has when completed.
	/// </summary>
	/// <returns>The number of frames.</returns>
	public int GetNumberOfFrames()
	{
		return frames.Length;
	}

    /// <summary>
    /// Get the speed of this instance
    /// </summary>
    /// <returns>The Speed of this instance</returns>
	public int GetSpeed()
	{
		return speed;
	}

    public float GetStrength()
    {
        return strength;
    }

    public void SetName(string name)
    {
        this.name = name;
    }

    public string GetName()
    {
        return name;
    }
}
