using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class NameValidator : MonoBehaviour
{
	private GameObject alreadyUsedText;
	private InputField inputField;
	private string name;
	private bool validName;

	void Start ()
	{
		alreadyUsedText = transform.FindChild ("AlreadyUsedText").gameObject;
		alreadyUsedText.SetActive (false);
		inputField = transform.FindChild ("NameInputField").gameObject.GetComponent<InputField> ();
	}

	public void ValidateName()
	{
		name = inputField.text;
		validName = !AvailableMoves.ContainsName (name);
		validName &= !name.Equals (string.Empty);
		alreadyUsedText.SetActive (AvailableMoves.ContainsName (name));
	}

	public bool IsNameValid()
	{
		return validName;
	}

	public string GetName()
	{
		return this.name;
	}
}
