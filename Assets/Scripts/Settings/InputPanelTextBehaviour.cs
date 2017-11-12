using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InputPanelTextBehaviour : MonoBehaviour {

	public static readonly string NO_BUTTON = "NO BUTTON"; // Used to make a difference between an uninstantiated button and a button that has been reset.
	private string currentButton;
	private int characterNumber;
	private int index;
	private InputListBehaviour parentListBehaviour;

	void Start(){
		this.parentListBehaviour = GameObject.Find ("MovePanelContainer").GetComponent<InputListBehaviour> ();
	}

	public void ButtonChanged(string newButton)
	{
		print ("currentButton = " + currentButton + ", newButton = " + newButton);
		if (newButton == null || newButton.Equals ("")) {
			print ("2");
			ResetButton ();
			return;
		}
		newButton = newButton.ToLower (); // Make no difference of capital or lower case letter.
		bool hadButtonPreviously = currentButton != null && !currentButton.Equals ("") && !currentButton.Equals(NO_BUTTON);
		if (hadButtonPreviously) {
			if (newButton.Equals (currentButton)) { // Same button registered, do nothing.
				print ("3");
				return;
			}
			if (newButton.Length >= 2) { // Reset inputfield to contain same button again if user types more than one character.
				gameObject.GetComponent<InputField> ().text = currentButton;
				return;
			} else if (newButton.Length == 1) {
				SwitchButton (newButton);		
			}
		} else if (currentButton != null && currentButton.Equals (NO_BUTTON)) {
			print ("1");
			SwitchButton (newButton);
		}

		this.currentButton = newButton;
	}

	private void SwitchButton(string newButton){
		InputSettings.RemoveButton (newButton);
		InputSettings.AddButton (newButton, StaticCharacterHolder.characters [characterNumber - 1], index);
		this.parentListBehaviour.ButtonUpdated ();
	}

	public void SetCharacterNumber(int characterNumber)
	{
		this.characterNumber = characterNumber;
	}

	public int GetCharacterNumber()
	{
		return this.characterNumber;
	}

	public void SetIndex(int index){
		this.index = index;
	}

	public void ResetButton(){
		this.currentButton = NO_BUTTON;
	}

	public string GetButton()
	{
		return this.currentButton;
	}
}
