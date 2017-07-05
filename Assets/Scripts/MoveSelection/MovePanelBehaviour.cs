using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MovePanelBehaviour : MonoBehaviour {

    private Text speedText;
    private Text strengthText;
	private Text nameText;
	private Text[] assignedButtonTexts;

    private Color32 defaultColor = new Color32(58,149,255,255);

    void Awake()
    {
        speedText = transform.Find("SpeedText").GetComponent<Text>();
        strengthText = transform.Find("StrengthText").GetComponent<Text>();
		nameText = transform.Find("NameText").GetComponent<Text>();

		assignedButtonTexts = new Text[2];
		assignedButtonTexts[0] = transform.Find("AssignedButton1Text").GetComponent<Text>();
		assignedButtonTexts[1] = transform.Find("AssignedButton2Text").GetComponent<Text>();
    }

    void Start ()
	{

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

    public void Select()
    {
        gameObject.GetComponent<Image>().color = Color.yellow;
    }

    public void DeSelect()
    {
        gameObject.GetComponent<Image>().color = defaultColor;
    }

	public void AssignButton(string button, Color32 color, int playerNumber)
	{
		playerNumber--;
		if (button.Length == 1) //Make sure the assigned button is not more than one character
		{
			assignedButtonTexts[playerNumber].color = color;
			assignedButtonTexts[playerNumber].text = button;
		}
	}

	public void ClearAssignedButton(string button, int playerNumber)
	{
		playerNumber--;
		if(assignedButtonTexts[playerNumber].text.Equals(button))
		{
			assignedButtonTexts[playerNumber].color = Color.black;
			assignedButtonTexts[playerNumber].text = "";
		}
	}

	public void ClearAssignedButton(int playerNumber)
	{
		playerNumber--;
		assignedButtonTexts[playerNumber].color = Color.black;
		assignedButtonTexts[playerNumber].text = "";
	}
}
