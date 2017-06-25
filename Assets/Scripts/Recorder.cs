using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Recorder : MonoBehaviour {

	private Move move; //The move being built by the recorder.
	public GameObject[] endPoints; //All end points (greens) of the recorded character. These are added manually in the unity scene editor.
                                   //TODO: Look through all childs of transform to find end points on start up to avoid manual work in editor?
    public bool isRecording = false;

	void Start(){
		move = new Move ();
	}

	//Create a frame object containing rotations of each limb and add it to the move.
	public void RecordFrame ()
	{
        // If we are recording a move, set isRecording variable
        if (move.GetFrames().Length == 0)
        {
            isRecording = true;
            print("isrecording = true");
        }

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
			frame.addBodyPartRoation (name, rotation);
			dragAndDrop.UpdateFrameLimits (); //Update drag and drop rotation limit by frame.
		}
		move.AddFrame (frame);
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
					print (i + ". frame.getRotation (\"Upper Right Arm\") = " + frame.getRotation ("Upper Right Arm"));
					i++;
				}
			}
		}
	}
}
