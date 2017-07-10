using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class NameValidator : MonoBehaviour
{
	private GameObject alreadyUsedText;
	private InputField inputField;
	private string name;
	private bool nameExists;

	void Start ()
	{
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

	public string GetName()
	{
		return this.name;
	}
}
