using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Recorder : MonoBehaviour {

	ProgressBarBehaviour progressBarBehaviour;
	private Move move; //The move being built by the recorder.
	private List<GameObject> endPoints;
	//public GameObject[] endPoints; //All end points (greens) of the recorded character. These are added manually in the unity scene editor.
                                   //TODO: Look through all childs of transform to find end points on start up to avoid manual work in editor?
	public bool reverseOnWayBack = true;
	private MovePlayer movePlayer;

	void Start(){
		move = new Move ();
		endPoints = new List<GameObject> ();
		progressBarBehaviour = gameObject.AddComponent<ProgressBarBehaviour> ();
		progressBarBehaviour.SetTotalNbrOfFrames (move.GetNumberOfFrames ());
		movePlayer = gameObject.AddComponent<MovePlayer> ();
		FindEndPoints ();
	}

	//Create a frame object containing rotations of each limb and add it to the move.
	public void RecordFrame ()
	{
		if (progressBarBehaviour.GetCurrentNbrOfFrames () < move.GetNumberOfFrames ()) {
			Frame frame = new Frame ();
			foreach (GameObject go in endPoints) {
				//Make sure endPoit has a drag and drop script before checking its parent's roation.
				DragAndDrop dragAndDrop = go.GetComponent<DragAndDrop> ();
				if (dragAndDrop == null) {
					continue;
				}
				//Add end point parent (limb) rotation and name to move.
				float rotation = go.transform.parent.localEulerAngles.z;
				string name = go.transform.parent.name;
				frame.AddBodyPartRotation (name, rotation);
				dragAndDrop.UpdateFrameLimits (); //Update drag and drop rotation limit by frame.
			}
			progressBarBehaviour.IncrementNbrOfFrames ();
			move.AddFrame (frame);
			if (progressBarBehaviour.GetCurrentNbrOfFrames () >= move.GetNumberOfFrames () / 2 && reverseOnWayBack) {
				ReverseFrames ();
			}
		}
	}

	void Update(){
		if (Input.GetKeyDown ("space")) {
			RecordFrame ();
		}
		//###### TEST ###### TODO: Remove this
		else if (Input.GetKeyDown ("p")) {
			int i = 0;
			foreach (System.Object o in move.GetFrames()) {
				if (o is Frame) {
					Frame frame = (Frame)o;
					print (i + ". frame.getRotation (\"Lower Right Arm\") = " + frame.getRotation ("Lower Right Arm"));
					i++;
				}
			}
			movePlayer.PlayMove (move);
		}
	}

	private void ReverseFrames(){
		Frame[] frames = move.GetFrames ();
		int halfNbrOfFrames = move.GetNumberOfFrames () / 2;
		for (int i = 0; i < halfNbrOfFrames; i++) {
			int frameIndex = halfNbrOfFrames - 1 - i;
			Frame frame = frames [frameIndex];
			move.AddFrame (frame);
			progressBarBehaviour.IncrementNbrOfFrames ();
		}
	}

	private void FindEndPoints()
	{
		Transform[] children = gameObject.GetComponentsInChildren<Transform> ();
		foreach (Transform child in children) {
			GameObject go = child.gameObject;
			DragAndDrop dragAndDrop = go.GetComponent<DragAndDrop> ();
			if (dragAndDrop != null) {
				endPoints.Add (go);
			}
		}
	}
}
