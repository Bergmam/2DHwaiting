using System.Collections;
using System.Collections.Generic;
using UnityEngine;
﻿using System.IO;
using UnityEngine.UI;

/// <summary>
/// Class for managing update of GUI when input settings changes.
/// This class creates buttons for each available move, the buttons trigger a text telling the user to press a key and the text tells this class to update GUI.
/// </summary>
public class InputGuiManager : MonoBehaviour {
	private GameObject[] player1InputButtons;
	private GameObject[] player2InputButtons;

	void Start () {
		List<string> characterOneButtons = InputSettings.GetCharacterButtons (1);
		Color32 color1 = StaticCharacterHolder.character1.GetColor ();
		List<string> characterTwoButtons = InputSettings.GetCharacterButtons (2);
		Color32 color2 = StaticCharacterHolder.character2.GetColor ();

		int maxNrOfMoves = InputSettings.MaxNrOfMoves ();
		player1InputButtons = new GameObject[maxNrOfMoves];
		player2InputButtons = new GameObject[maxNrOfMoves];
		for (int i = 0; i < maxNrOfMoves; i++) {
			Move move = new Move ();
			move.SetName ("Move " + (i + 1));
			string previewPath = "Prefabs" + Path.DirectorySeparatorChar + "MoveInputPanel";
			GameObject previewPanelObject = (GameObject)Resources.Load (previewPath);
			GameObject previewPanel = Instantiate (previewPanelObject, previewPanelObject.transform.position, previewPanelObject.transform.rotation, transform);

			previewPanel.transform.Find ("NameText").GetComponent<Text> ().text = "Move " + (i + 1);
			InitiateInputButton (previewPanel, 1, i, characterOneButtons [i]);
			InitiateInputButton (previewPanel, 2, i, characterTwoButtons [i]);
		}

		//Make sure text is able to notify this class when the user presses a key/button.
		GameObject enterButtonText = GameObject.Find ("EnterButtonText");
		enterButtonText.GetComponent<EnterButtonScript> ().SetInputGuiManager (this);
		enterButtonText.SetActive (false); //Hide text.
		UpdateGUI ();
	}

	private void InitiateInputButton(GameObject previewPanel, int characterNumber, int index, string button){
		Transform inputButtonTransform = previewPanel.transform.Find ("Player" + characterNumber + "Button");

		inputButtonTransform.GetComponent<InputButtonBehaviour> ().SetCharacterNumber (characterNumber);
		inputButtonTransform.GetComponent<InputButtonBehaviour> ().SetIndex (index);
		inputButtonTransform.GetComponent<InputButtonBehaviour> ().SetEnterButtonText (GameObject.Find ("EnterButtonText")); //Make sure buttons are able to show/hide text.

		if (characterNumber == 1) {
			player1InputButtons [index] = inputButtonTransform.gameObject;
		} else {
			player2InputButtons [index] = inputButtonTransform.gameObject;
		}
	}

	/// <summary>
	/// Update GUI to match input settings.
	/// </summary>
	public void UpdateGUI(){
		UpdateCharacterButtons (player1InputButtons, 1);
		UpdateCharacterButtons (player2InputButtons, 2);
	}

	/// <summary>
	/// Update the gui of the columns of buttons belonging to one character.
	/// </summary>
	/// <param name="playerInputButtons">Player input buttons.</param>
	/// <param name="characterNumber">Character number.</param>
	private void UpdateCharacterButtons(GameObject[] playerInputButtons, int characterNumber)
	{
		for (int i = 0; i < playerInputButtons.Length; i++) {
			GameObject inputButton = playerInputButtons [i];
			string button = InputSettings.GetCharacterButton (characterNumber, i);
			Transform buttonTextTransform = inputButton.transform.Find ("Text");
			buttonTextTransform.gameObject.GetComponent<Text> ().text = button;
		}
	}

}
