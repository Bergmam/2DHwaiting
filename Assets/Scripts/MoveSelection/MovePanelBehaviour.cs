using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MovePanelBehaviour : MonoBehaviour {

    private Text speedText;
    private Text strengthText;
    private Text nameText;

    private Color32 defaultColor = new Color32(58,149,255,255);

    void Awake()
    {
        speedText = transform.Find("SpeedText").GetComponent<Text>();
        strengthText = transform.Find("StrengthText").GetComponent<Text>();
        nameText = transform.Find("NameText").GetComponent<Text>();
    }

    void Start ()
	{

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

    public void Select()
    {
        gameObject.GetComponent<Image>().color = Color.yellow;
    }

    public void DeSelect()
    {
        gameObject.GetComponent<Image>().color = defaultColor;
    }
}
