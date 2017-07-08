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
	//public GameObject[] endPoints; //All end points (greens) of the recorded character. These are added manually in the unity scene editor.
                                   //TODO: Look through all childs of transform to find end points on start up to avoid manual work in editor?
	public bool reverseOnWayBack = true;
	private MovePlayer movePlayer;

    public GameObject confirmPrompt;
    public GameObject confirmText;
    public GameObject errorText;

	public GameObject slidersObject;
    public SliderScript sliders;
	private Frame initialPoseFrame;


	void Start()
	{
		move = new Move ();
		endPoints = new List<GameObject> ();
		progressBarBehaviour = gameObject.AddComponent<ProgressBarBehaviour> ();
		progressBarBehaviour.SetTotalNbrOfFrames (move.GetNumberOfFrames ());
		movePlayer = gameObject.AddComponent<MovePlayer> ();
        FindEndPoints ();
		initialPoseFrame = GetCurrentPoseFrame ();

		DamagaDealerChangedNotifier damageDealerNotifier = new DamagaDealerChangedNotifier (move);
		// Set the move in all selectors so that they can set the damage dealing body part and listen to changes.
		foreach (DamageDealerSelector selector in gameObject.GetComponentsInChildren<DamageDealerSelector>())
		{
			if (selector != null)
			{
				selector.SetDamageDealerNotifier (damageDealerNotifier);
			}
		}
    }

	/// <summary>
	/// Adds a frame object containng rotations of each limb to the move.
	/// </summary>
	public void RecordFrame ()
	{
		if (progressBarBehaviour.GetCurrentNbrOfFrames () < move.GetNumberOfFrames ()) 
		{
			Frame frame = GetCurrentPoseFrame ();
			progressBarBehaviour.IncrementNbrOfFrames ();
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
			dragAndDrop.UpdateFrameLimits (); //Update drag and drop rotation limit by frame.
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
		//###### TEST ###### TODO: Remove this
		else if (Input.GetKeyDown ("p")) 
		{
			int i = 0;
			foreach (System.Object o in move.GetFrames()) 
			{
				if (o is Frame) 
				{
					Frame frame = (Frame)o;
					print (i + ". frame.getRotation (\"Lower Right Arm\") = " + frame.getRotation ("Lower Right Arm"));
					i++;
				}
			}
			movePlayer.PlayMove (move);
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
        move.SetSpeed(sliders.GetSpeed());
        slidersObject.SetActive(false);
        movePlayer.SetAutoLoopEnabled(true);
        movePlayer.PlayMove(move);
        confirmPrompt.SetActive(true);

    }
    
    /// <summary>
    /// Method run when a player wants to save a finished move
    /// </summary>
    public void ConfirmMove()
    {
        confirmPrompt.SetActive(false);
        confirmText.SetActive(true);

    }

    /// <summary>
    /// Method run when a player wants to cancel a finished move
    /// </summary>
    public void CancelMove()
        // TODO: Reset the character to the original position
    {
        confirmPrompt.SetActive(false);
        ResetMoveEditor();

    }

    /// <summary>
    /// Method to call when the input field of the move has been filled.
    /// Updates the name of the move, updates AvailableMoves with the move, and resets the MoveEditor.
    /// </summary>
    /// <param name="name"></param>
    public void ConfirmText(InputField input)
    {
		move.SetStrength(sliders.GetStrength());
        move.SetName(input.text);

        if (AvailableMoves.ContainsName(move.GetName()))
        {
            // TODO: Some kind of pop-up when the name already exists, then player can enter a new name.
            print("The name of the move already exists in AvailableMoves ");
            errorText.SetActive(true);
        }
        else
        {
            errorText.SetActive(false); // Might already be false if we never entered a duplicate name.
            confirmText.SetActive(false);
            input.text = "Name...";
            AvailableMoves.AddMove(move);
            print("Move with name " + input.text + " and values strength: " + move.GetStrength().ToString() + ", speed: " + move.GetSpeed() + " saved in AvailableMoves");
        }
        print("AvailableMoves now contains: " + AvailableMoves.PrintMoveNames());
        ResetMoveEditor();
    }

    /// <summary>
    /// Resets the MoveEditor by restarting the progressBar and the movePlayer.
    /// </summary>
    private void ResetMoveEditor()
	{
		movePlayer.SetAutoLoopEnabled (false);
		move = new Move ();
		Destroy(progressBarBehaviour);
		progressBarBehaviour = gameObject.AddComponent<ProgressBarBehaviour> ();
		progressBarBehaviour.SetTotalNbrOfFrames (move.GetNumberOfFrames ());
		movePlayer.FrameToCharacter (initialPoseFrame); //Reset character pose
		UpdateFrameTwistLimits();
		sliders.ResetSliders(); //Reset sliders to 50/50
		confirmText.GetComponentInChildren<InputField> ().text = ""; //Reset move name
		slidersObject.SetActive(true);
    }
}
