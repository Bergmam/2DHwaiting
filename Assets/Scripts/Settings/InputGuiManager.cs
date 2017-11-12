using System.Collections;
using System.Collections.Generic;
using UnityEngine;
﻿using System.IO;
using UnityEngine.UI;

public class InputGuiManager : MonoBehaviour {

	private List<InputPanelTextBehaviour> inputTextBehaviours;
	private GameObject[] player1InputButtons;
	private GameObject[] player2InputButtons;

	// Use this for initialization
	void Start () {
		inputTextBehaviours = new List<InputPanelTextBehaviour> ();


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

		GameObject enterButtonText = GameObject.Find ("EnterButtonText");
		enterButtonText.GetComponent<EnterButtonScript> ().SetInputGuiManager (this);
		enterButtonText.SetActive (false);
		UpdateButtons ();
	}

	private void InitiateInputButton(GameObject previewPanel, int characterNumber, int index, string button){
		Transform inputButtonTransform = previewPanel.transform.Find ("Player" + characterNumber + "Button");

		inputButtonTransform.GetComponent<InputButtonBehaviour> ().SetCharacterNumber (characterNumber);
		inputButtonTransform.GetComponent<InputButtonBehaviour> ().SetIndex (index);
		inputButtonTransform.GetComponent<InputButtonBehaviour> ().SetEnterButtonText (GameObject.Find ("EnterButtonText"));

		if (characterNumber == 1) {
			player1InputButtons [index] = inputButtonTransform.gameObject;
		} else {
			player2InputButtons [index] = inputButtonTransform.gameObject;
		}
	}

	public void UpdateButtons(){
		//Make sure input fields display the actual registered moves after one field has been updated.
		// Necessary because some fields may be cleared after the same button is registered again on a different move.
		UpdateCharacterFields (player1InputButtons, 1);
		UpdateCharacterFields (player2InputButtons, 2);
	}

	private void UpdateCharacterFields(GameObject[] playerInputButtons, int characterNumber)
	{
		for (int i = 0; i < playerInputButtons.Length; i++) {
			GameObject inputButton = playerInputButtons [i];
			string button = InputSettings.GetCharacterButton (characterNumber, i);
			Transform buttonTextTransform = inputButton.transform.Find ("Text");
			buttonTextTransform.gameObject.GetComponent<Text> ().text = button;
		}
	}

}
