using System.Collections;
using System.Collections.Generic;
using UnityEngine;
﻿using System.IO;
using UnityEngine.UI;

public class InputListBehaviour : MonoBehaviour {

	private List<InputPanelTextBehaviour> inputTextBehaviours;
	private InputField[] player1InputFields;
	private InputField[] player2InputFields;

	// Use this for initialization
	void Start () {
		inputTextBehaviours = new List<InputPanelTextBehaviour> ();


		List<string> characterOneButtons = InputSettings.GetCharacterButtons (1);
		Color32 color1 = StaticCharacterHolder.character1.GetColor ();
		List<string> characterTwoButtons = InputSettings.GetCharacterButtons (2);
		Color32 color2 = StaticCharacterHolder.character2.GetColor ();

		int maxNrOfMoves = InputSettings.MaxNrOfMoves ();
		player1InputFields = new InputField[maxNrOfMoves];
		player2InputFields = new InputField[maxNrOfMoves];
		for (int i = 0; i < maxNrOfMoves; i++) {
			Move move = new Move ();
			move.SetName ("Move " + (i + 1));
			string previewPath = "Prefabs" + Path.DirectorySeparatorChar + "InputPanel";
			GameObject previewPanelObject = (GameObject)Resources.Load (previewPath);
			GameObject previewPanel = Instantiate (previewPanelObject, previewPanelObject.transform.position, previewPanelObject.transform.rotation, transform);
			MovePanelBehaviour panelBehaviour = previewPanel.GetComponent<MovePanelBehaviour> ();

			previewPanel.transform.Find ("NameText").GetComponent<Text> ().text = "Move " + (i + 1);
			InitiateInputField (previewPanel, 1, i, characterOneButtons [i]);
			InitiateInputField (previewPanel, 2, i, characterTwoButtons [i]);
		}
	}

	private void InitiateInputField(GameObject previewPanel, int characterNumber, int index, string button){
		Transform inputFieldTransform = previewPanel.transform.Find ("Player" + characterNumber + "InputField");
		inputFieldTransform.GetComponent<InputPanelTextBehaviour> ().SetCharacterNumber (characterNumber);
		inputFieldTransform.GetComponent<InputPanelTextBehaviour> ().SetIndex (index);
		InputField inputField = inputFieldTransform.GetComponent<InputField> ();
		inputField.text = button;
		inputField.placeholder.GetComponent<Text>().text = "button " + index;
		if (characterNumber == 1) {
			player1InputFields [index] = inputFieldTransform.GetComponent<InputField> ();
		} else {
			player2InputFields [index] = inputFieldTransform.GetComponent<InputField> ();
		}
	}

	public void ButtonUpdated(){
		//Make sure input fields display the actual registered moves after one field has been updated.
		// Necessary because some fields may be cleared after the same button is registered again on a different move.
		UpdateCharacterFields (player1InputFields, 1);
		UpdateCharacterFields (player2InputFields, 2);
	}

	private void UpdateCharacterFields(InputField[] playerInputFields, int characterNumber)
	{
		for (int i = 0; i < playerInputFields.Length; i++) {
			InputField inputField = playerInputFields [i];
			string button = InputSettings.GetCharacterButton (characterNumber, i);
			if (inputField.GetComponent<InputPanelTextBehaviour> ().GetButton ().Equals (InputPanelTextBehaviour.NO_BUTTON)) {
				//button = "";
			}
			inputField.text = button;
			/*if (button.Equals ("")) {
				//Make sure currentButton of all InputPanelTextBehaviours are updated to be able to register a new button.
				inputField.GetComponent<InputPanelTextBehaviour> ().ResetButton ();
			}*/
		}
	}
}
