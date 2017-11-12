using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EnterButtonScript : MonoBehaviour {

	private int currentIndex;
	private int currentCharacterNumber;
	private string availableKeys = "qertyuiopfghjklzxcvbnm,.-1234567890";
	private InputGuiManager inputGuiManager;

	// Use this for initialization
	void Start () {

	}

	// Update is called once per frame
	void Update () {
		for (int i = 0; i < availableKeys.Length; i++) {
			if (Input.GetKeyDown ("" + availableKeys [i])) {
				string newButton = "" + availableKeys [i];
				InputSettings.RemoveButton (newButton);
				InputSettings.AddButton (newButton, StaticCharacterHolder.characters [currentCharacterNumber - 1], currentIndex);
				this.inputGuiManager.UpdateButtons ();
				gameObject.SetActive (false);
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
