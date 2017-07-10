using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MoveCreator : MonoBehaviour
{
	private Move move;
	private SliderScript sliders;
	private Recorder recorder;
	private NameValidator nameValidator;
	private DropdownBehaviour dropDown;
	private Button saveButton;

	void Start ()
	{
		recorder = GameObject.Find ("Character").GetComponent<Recorder> ();
		sliders = GameObject.Find ("SlidersPanel").GetComponent<SliderScript> ();
		nameValidator = GameObject.Find ("NamePanel").GetComponent<NameValidator> ();
		dropDown = GameObject.Find ("BodypartDropdown").GetComponent<DropdownBehaviour> ();
		saveButton = GameObject.Find ("SaveButton").GetComponent<Button> ();
		PlaceCharacter (0.5f, 0.5f);
		move = new Move ();
		recorder.SetMove (move);
	}

	void Update ()
	{
		if (sliders.GetStrength () != move.GetStrength ())
		{
			move.SetStrength(sliders.GetStrength ());
		}
		if (sliders.GetSpeed () != move.GetSpeed ())
		{
			move.SetSpeed(sliders.GetSpeed ());
		}
		if (!dropDown.GetDamageDealer ().Equals (move.GetDamageDealer ()))
		{
			move.SetDamageDealer (dropDown.GetDamageDealer ());
		}
		if (recorder.IsDoneRecording () && nameValidator.IsNameValid ()) {
			move.SetName (nameValidator.GetName ());
			saveButton.interactable = true;
		} else {
			saveButton.interactable = false;
		}
	}

	private void PlaceCharacter (float x, float y)
	{
		float screenHeight = Screen.height;
		float screenWidth = Screen.width;
		Vector3 characterStartPosition = new Vector3 (x * screenWidth, y * screenHeight, 0f);
		characterStartPosition = Camera.main.ScreenToWorldPoint (characterStartPosition);
		characterStartPosition = new Vector3 (characterStartPosition.x, characterStartPosition.y, 0f);
		transform.position = characterStartPosition;
	}

	/// <summary>
	/// Resets the MoveEditor by restarting the progressBar and the movePlayer.
	/// </summary>
	private void ResetMoveEditor()
	{
		sliders.ResetSliders(); //Reset sliders to 50/50
		move = new Move ();
		recorder.Reset (move);
	}

	public void SaveMove()
	{
		if (!AvailableMoves.ContainsName (move.GetName ()))
		{
			AvailableMoves.AddMove (move);
			ResetMoveEditor ();
			saveButton.interactable = true;
		}
		else
		{
			saveButton.interactable = false;
		}
	}
}
