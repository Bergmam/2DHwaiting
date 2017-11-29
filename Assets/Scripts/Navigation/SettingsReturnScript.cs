﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SettingsReturnScript : MonoBehaviour {

	private GameObject enterAllButtonsText;

	void Start()
	{
		this.enterAllButtonsText = GameObject.Find ("EnterAllButtonsText");
	}

	void Update ()
	{
		bool escapePressed = Input.GetKeyDown (KeyCode.Escape);
		bool allButtonsAdded = InputSettings.AllButtonsAdded ();

		if (allButtonsAdded)
		{
			enterAllButtonsText.SetActive (false);
		}

		if (escapePressed)
		{
			if (allButtonsAdded)
			{
				gameObject.GetComponent<SceneReturnScript> ().GoBack ();
			}
			else
			{
				enterAllButtonsText.SetActive (true);
			}
		}
	}
}