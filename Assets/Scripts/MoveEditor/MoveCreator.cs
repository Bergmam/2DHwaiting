using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MoveCreator : MonoBehaviour
{
	private Move move;
	private SliderScript sliders;
	private Recorder recorder;
	private InputField nameInputField;
	private NameValidator nameValidator;
	private DropdownBehaviour dropDown;
	private Button saveButton;
	private MovePlayer movePlayer;

	void Start ()
	{
		GameObject character = GameObject.Find ("Character");
		recorder = character.GetComponent<Recorder> ();
		movePlayer = character.GetComponent<MovePlayer> ();
		sliders = GameObject.Find ("SlidersPanel").GetComponent<SliderScript> ();
		nameValidator = GameObject.Find ("NamePanel").GetComponent<NameValidator> ();
		dropDown = GameObject.Find ("BodypartDropdown").GetComponent<DropdownBehaviour> ();
		saveButton = GameObject.Find ("SaveButton").GetComponent<Button> ();
		nameInputField = GameObject.Find ("NameInputField").GetComponent<InputField> ();
		PlaceCharacter (0.5f, 0.5f);
		move = new Move ();
		recorder.SetMove (move);
	}

	void Update ()
	{
		//Update strength and speed values if they have changed. Don't need to update twice since the sliders update each other.
		if (sliders.GetSpeed () != move.GetSpeed () || sliders.GetStrength () != move.GetStrength ()) 
		{
			updateStrengthAndSpeed (sliders.GetSpeed (), sliders.GetStrength ());
		}
		//Update damage dealing bodypart.
		if (!dropDown.GetDamageDealer ().Equals (move.GetDamageDealer ()))
		{
			move.SetDamageDealer (dropDown.GetDamageDealer ());
		}
		//All frames recorded and a new, non-empty, move name has been entered.
		if (recorder.IsDoneRecording () && nameValidator.IsNameValid ()) {
			move.SetName (nameValidator.GetName ());
			saveButton.interactable = true;
		} else {
			saveButton.interactable = false; //hide button again if name is no longer valid.
		}
	}

	/// <summary>
	/// Updates the strength and speed values to the values of the sliders.
	/// </summary>
	private void updateStrengthAndSpeed(int speed, int strength)
	{
		move.SetSpeed(speed);
		move.SetStrength(strength);
		//If animation is already playing, replay it as speed changes.
		if (movePlayer != null && movePlayer.CheckIsPlaying ())
		{
			movePlayer.PlayMove (this.move);
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
	public void ResetMoveEditor()
	{
		nameInputField.text = string.Empty;
		sliders.ResetSliders(); //Reset sliders to 50/50
		move = new Move ();
		recorder.Reset (move);
        saveButton.interactable = false;
    }

	public void SaveMove()
	{
		if (!AvailableMoves.ContainsName (move.GetName ()) && !move.GetName().Equals(string.Empty))
		{
			AvailableMoves.AddMove ((Move)move.Clone ());
			ResetMoveEditor ();
		}
		else
		{
            saveButton.interactable = false;
        }
	}

	public void SetBlockMove(bool blockMove)
	{
		if (blockMove) {
			SetBlockMoveTexts ();
		} else {
			SetAttackMoveTexts ();
		}
		this.move.SetBlockMove (blockMove);
	}

	private void SetAttackMoveTexts()
	{
		sliders.SetSliderStrings ("Strength", "Speed");
		dropDown.SetLabelText ("Damage Dealer");
	}

	private void SetBlockMoveTexts()
	{
		sliders.SetSliderStrings ("Coverage", "Speed & Block");
		dropDown.SetLabelText ("Shield");
	}
}
