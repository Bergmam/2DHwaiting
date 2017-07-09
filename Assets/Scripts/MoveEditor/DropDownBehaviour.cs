using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DropDownBehaviour : MonoBehaviour {

	private Dropdown dropdown;

	void Start () {
		dropdown = gameObject.GetComponent<Dropdown> ();
		dropdown.ClearOptions ();
		GameObject character = GameObject.Find ("Character");
		DragAndDrop[] dragAndDropInstances = character.GetComponentsInChildren<DragAndDrop> ();
		List<string> bodyPartNames = new List<string> ();
		foreach (DragAndDrop dragAndDropInstance in dragAndDropInstances)
		{
			GameObject dragPoint = dragAndDropInstance.gameObject;
			Transform bodyPartTransform = dragPoint.transform.parent;
			bodyPartNames.Add (bodyPartTransform.name);
		}
		dropdown.AddOptions (bodyPartNames);
	}
}
