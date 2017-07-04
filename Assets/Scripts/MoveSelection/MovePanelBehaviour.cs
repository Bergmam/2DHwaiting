using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MovePanelBehaviour : MonoBehaviour {

	private Text speedText;
	private Text strengthText;
	private Text nameText;

	void Start ()
	{
		speedText = GameObject.Find ("SpeedText").GetComponent<Text> ();
		strengthText = GameObject.Find ("StrengthText").GetComponent<Text> ();
		nameText = GameObject.Find ("NameText").GetComponent<Text> ();
	}

	void Update ()
	{
		if (Input.GetKeyDown ("a"))
		{
			SetName ("APA BEPA");
		}
		else if (Input.GetKeyDown ("s"))
		{
			SetSpeed (10);
		}
		else if (Input.GetKeyDown ("d"))
		{
			SetStrength (99);
		}
	}

	public void SetSpeed(int speed)
	{
		speedText.text = "" + speed;
	}

	public void SetStrength(int strength)
	{
		strengthText.text = "" + strength;
	}

	public void SetName(string name)
	{
		nameText.text = name;
	}
}
