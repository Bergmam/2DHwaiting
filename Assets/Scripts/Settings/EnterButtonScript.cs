using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// Script attached to the text telling the user to press a button.
/// </summary>
public class EnterButtonScript : MonoBehaviour {

	private int currentIndex;
	private int currentCharacterNumber;
	private string availableKeys = "qertyuiopfghjklzxcvbnm,.-1234567890"; //All keyboard keys that are not used for movement.
	private InputGuiManager inputGuiManager;

	void Update () {
		for (int i = 0; i < availableKeys.Length; i++) {
			if (Input.GetKeyDown ("" + availableKeys [i])) {
				string newButton = "" + availableKeys [i];
				InputSettings.RemoveButton (newButton); //Remove button where it is currently used so that it is only used once.
				print("currentCharacterNumber = " + currentCharacterNumber + ", " + " currentIndex = " + currentIndex);
				InputSettings.AddButton (newButton, StaticCharacterHolder.characters [currentCharacterNumber - 1], currentIndex); //Add button in its new place.
				this.inputGuiManager.UpdateGUI (); //Update gui to match current settings.
				gameObject.SetActive (false); //Hide text telling user to press a key.
			}
		}
	}

	public void SetCurrentIndex(int currentIndex){
		this.currentIndex = currentIndex;
		UpdateText ();
	}

	public void SetCurrentCharacter(int currentCharacter){
		this.currentCharacterNumber = currentCharacter;
		UpdateText ();
	}

	public void SetInputGuiManager(InputGuiManager inputGuiManager){
		this.inputGuiManager = inputGuiManager;
	}

	private  void UpdateText()
	{
		this.GetComponent<Text> ().text = "Enter move " + currentIndex + " button\nfor player " + currentCharacterNumber;
	}
}
