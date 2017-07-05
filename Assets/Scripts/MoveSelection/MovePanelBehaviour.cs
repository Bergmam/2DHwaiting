using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MovePanelBehaviour : MonoBehaviour {

    private Text speedText;
    private Text strengthText;
	private Text nameText;
	private Text assignedButton1Text;
	private Text assignedButton2Text;

    private Color32 defaultColor = new Color32(58,149,255,255);

    void Awake()
    {
        speedText = transform.Find("SpeedText").GetComponent<Text>();
        strengthText = transform.Find("StrengthText").GetComponent<Text>();
		nameText = transform.Find("NameText").GetComponent<Text>();
		assignedButton1Text = transform.Find("AssignedButton1Text").GetComponent<Text>();
		assignedButton2Text = transform.Find("AssignedButton2Text").GetComponent<Text>();
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

	public void AssignButton1(string button, Color32 color)
	{
		if (button.Length == 1) //Make sure the assigned button is not more than one character
		{
			assignedButton1Text.color = color;
			assignedButton1Text.text = button;
		}
	}

	public void ClearAssignedButton1(string button)
	{
		if(assignedButton1Text.text.Equals(button))
		{
			assignedButton1Text.color = Color.black;
			assignedButton1Text.text = "";
		}
	}

	public void AssignButton2(string button, Color32 color)
	{
		if (button.Length == 1) //Make sure the assigned button is not more than one character
		{
			assignedButton2Text.color = color;
			assignedButton2Text.text = button;
		}
	}

	public void ClearAssignedButton2(string button)
	{
		if(assignedButton2Text.text.Equals(button))
		{
			assignedButton2Text.color = Color.black;
			assignedButton2Text.text = "";
		}
	}
}
