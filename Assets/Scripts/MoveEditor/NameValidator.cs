using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class NameValidator : MonoBehaviour {

	private GameObject alreadyUsedText;
	private InputField inputField;
	private string newName;
	private bool nameExists;

	// Use this for initialization
	void Start () {
		alreadyUsedText = transform.FindChild ("AlreadyUsedText").gameObject;
		alreadyUsedText.SetActive (false);
		inputField = transform.FindChild ("NameInputField").gameObject.GetComponent<InputField> ();
	}

	public void ValidateName()
	{
		name = inputField.text;
		nameExists = AvailableMoves.ContainsName (name);
		alreadyUsedText.SetActive (nameExists);
	}

	public bool IsNameValid()
	{
		return !nameExists;
	}
}
