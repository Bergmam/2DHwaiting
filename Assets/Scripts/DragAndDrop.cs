using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DragAndDrop : MonoBehaviour {

	private bool mouseDown;

	void OnMouseDown() {
		mouseDown = true;
	}

	void OnMouseDrag() {
		if (mouseDown) {
			//TODO: Rotate limb
		}
	}

	void OnMouseUp() {
		mouseDown = false;
	}
}
