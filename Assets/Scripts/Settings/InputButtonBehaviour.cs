using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputButtonBehaviour : MonoBehaviour {

	private GameObject enterButtonText;
	private EnterButtonScript enterButtonScript;
	private int characterNumber;
	private int index;

	public void SetInputOfMove(){
		if (enterButtonText != null && enterButtonScript != null)
		{
			enterButtonScript.SetCurrentIndex (index);
			enterButtonScript.SetCurrentCharacter (characterNumber);
			enterButtonText.SetActive (true);
		}
	}

	public void SetEnterButtonText(GameObject enterButtonText){
		if (enterButtonText != null) {
			this.enterButtonText = enterButtonText;
			this.enterButtonScript = enterButtonText.GetComponent<EnterButtonScript> ();
		}
	}

	public void SetIndex(int index){
		this.index = index;
	}

	public void SetCharacterNumber(int characterNumber){
		this.characterNumber = characterNumber;
	}
}
