using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ListKeyboardManager : MonoBehaviour {

	void Start()
	{
		Activate ();
	}

	public void Activate(){
		for (int j = 0; j < transform.childCount; j++)
		{
			// Find the right component and select it for keyboard navigation.
			Transform child = transform.GetChild (j);
			InputField inputField = child.GetComponentInChildren<InputField> ();
			if (inputField != null)
			{
				inputField.Select ();
				inputField.ActivateInputField ();
				return;
			}
			Slider slider = child.GetComponentInChildren<Slider> ();
			if (slider != null)
			{
				slider.Select ();
				return;
			}
			Button button = child.GetComponentInChildren<Button> ();
			if (button != null)
			{
				button.Select ();
				return;
			}
			Toggle toggleButton = child.GetComponentInChildren<Toggle> ();
			if (toggleButton != null && !child.name.Equals ("HeadToggleButton")) // TEMPORARY FIX
			{
				toggleButton.Select ();
				return;
			}
		}
	}


	void Update(){
		// If enter i pressed, find next or save button and select it.
		// This makes pressing enter again go to next phase.
		if (Input.GetKeyDown (KeyCode.Return) || Input.GetKeyDown (KeyCode.KeypadEnter))
		{
			Transform nextButtonTransform = transform.Find ("NextButton");
			if (nextButtonTransform == null)
			{
				nextButtonTransform = transform.Find ("SaveButton");
			}
			if (nextButtonTransform != null)
			{
				Button button = nextButtonTransform.GetComponent<Button> ();
				if (button != null)
				{
					button.Select ();
					return;
				}
			}
		}
	}
}
