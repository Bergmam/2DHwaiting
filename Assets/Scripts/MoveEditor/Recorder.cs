using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// Class for recording several Frames and storing them in a Move in the MoveEditor.
/// </summary>
public class Recorder : MonoBehaviour 
{
	ProgressBarBehaviour progressBarBehaviour;
	private Move move; //The move being built by the recorder.
	private List<GameObject> endPoints;
	public bool reverseOnWayBack = true;
	private MovePlayer movePlayer;
	private bool doneRecording;

	private Frame initialPoseFrame;


	void Start()
	{
		doneRecording = false;
		endPoints = new List<GameObject> ();
		movePlayer = gameObject.GetComponent<MovePlayer> ();
        FindEndPoints ();
		initialPoseFrame = GetCurrentPoseFrame ();
    }
		
	public void SetMove(Move move)
	{
		this.move = move;
		//Adjust progress bar to fit the specified move.
		progressBarBehaviour = GameObject.Find ("ProgressBar").GetComponent<ProgressBarBehaviour> ();
		progressBarBehaviour.SetTotalNbrOfFrames (move.GetNumberOfFrames ());
	}

	/// <summary>
	/// Adds a frame object containng rotations of each limb to the move.
	/// </summary>
	public void RecordFrame ()
	{
		if (move == null)
		{
			return;
		}
		if (progressBarBehaviour.GetCurrentNbrOfFrames () < move.GetNumberOfFrames ()) 
		{
			Frame frame = GetCurrentPoseFrame ();
			progressBarBehaviour.IncrementNbrOfFrames ();
			UpdateFrameTwistLimits ();
			move.AddFrame (frame);
			if (progressBarBehaviour.GetCurrentNbrOfFrames () >= move.GetNumberOfFrames () / 2 && reverseOnWayBack) 
			{
				ReverseFrames ();
				FinishMove ();
			}
			else if (progressBarBehaviour.GetCurrentNbrOfFrames () >= move.GetNumberOfFrames () && !reverseOnWayBack)
			{
				FinishMove ();
			}
		}
	}

	/// <summary>
	/// Creates a frame object containng current rotations of each limb.
	/// </summary>
	/// <returns>The current pose frame.</returns>
	private Frame GetCurrentPoseFrame()
	{
		Frame frame = new Frame ();
		foreach (GameObject go in endPoints) 
		{
			//Make sure endPoit has a drag and drop script before checking its parent's roation.
			DragAndDrop dragAndDrop = go.GetComponent<DragAndDrop> ();
			if (dragAndDrop == null) 
			{
				continue;
			}
			//Add end point parent (limb) rotation and name to move.
			float rotation = go.transform.parent.localEulerAngles.z;
			string name = go.transform.parent.name;
			frame.AddBodyPartRotation (name, rotation);
		}
		return frame;
	}

	/// <summary>
	/// Updates character frame twist limits to match current pose.
	/// </summary>
	private void UpdateFrameTwistLimits()
	{
		foreach (GameObject go in endPoints) 
		{
			//Make sure endPoit has a drag and drop script.
			DragAndDrop dragAndDrop = go.GetComponent<DragAndDrop> ();
			if (dragAndDrop == null) 
			{
				continue;
			}
			dragAndDrop.UpdateFrameLimits (); //Update drag and drop rotation limit by frame.
		}
	}

	void Update()
	{
		if (Input.GetKeyDown ("space")) 
		{
			RecordFrame ();
		}
	}

	/// <summary>
	/// Adds the reverse of all existing frames to make the move end where it started.
	/// </summary>
	private void ReverseFrames()
	{
		Frame[] frames = move.GetFrames ();
		int halfNbrOfFrames = move.GetNumberOfFrames () / 2;
		for (int i = 0; i < halfNbrOfFrames; i++) 
		{
			int frameIndex = halfNbrOfFrames - 1 - i;
			Frame frame = frames [frameIndex];
			move.AddFrame (frame);
			progressBarBehaviour.IncrementNbrOfFrames ();
		}
	}

	/// <summary>
	/// Finds the end points of the all the children of the current gameobject.
	/// </summary>
	private void FindEndPoints()
	{
		Transform[] children = gameObject.GetComponentsInChildren<Transform> ();
		foreach (Transform child in children) 
		{
			GameObject go = child.gameObject;
			DragAndDrop dragAndDrop = go.GetComponent<DragAndDrop> ();
			if (dragAndDrop != null) 
			{
				endPoints.Add (go);
			}
		}
	}

	/// <summary>
	/// Tells the MovePlayer that we are done recording the move and that it should start playing the move
	/// </summary>
	private void FinishMove()
	{
		movePlayer.SetAutoLoopEnabled(true);
		if (move != null)
		{
			movePlayer.PlayMove (move);
			doneRecording = true;
		}
	}

	public bool IsDoneRecording(){
		return doneRecording;
	}

	/// <summary>
	/// Reset the recorder and the progress bar.
	/// </summary>
	/// <param name="newMove">New move.</param>
	public void Reset(Move newMove)
	{
		if (newMove == null)
		{
			return;
		}
        this.move = newMove;
		movePlayer.SetAutoLoopEnabled (false);
		Destroy(progressBarBehaviour);
		progressBarBehaviour.SetTotalNbrOfFrames (move.GetNumberOfFrames ());
		progressBarBehaviour.SetCurrentNbrOfFrames (0);
		movePlayer.FrameToCharacter (initialPoseFrame); //Reset character pose
		UpdateFrameTwistLimits();
	}
}
