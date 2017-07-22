using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// Class containing functionality and help functions for controlling the sliders 
/// in the moveeditor.
/// </summary>
public class SliderScript : MonoBehaviour
{
	private Text strengthText;
	private Text speedText;

	private Slider strengthSlider;
    private Slider speedSlider;

    private int strengthValue;
    private int speedValue;

	private string topSliderString;
	private string botSliderString;

    void Start ()
    {
		strengthSlider = GameObject.Find("StrengthSlider").GetComponent<Slider>();
		speedSlider = GameObject.Find("SpeedSlider").GetComponent<Slider>();

		strengthText = GameObject.Find ("StrengthText").GetComponent<Text>();
		speedText = GameObject.Find("SpeedText").GetComponent<Text>();

        strengthValue = (int)strengthSlider.value;
        speedValue = (int)speedSlider.value;

		SetSliderStrings ("Strength", "Speed");
	}


    /// <summary>
    /// When the value of the speed slider changes, update the value of the text field
    /// and the speed slider.
    /// </summary>
    /// <param name="newValue">The new value of the speedslider</param>
    public void SpeedSliderChanged(float newValue)
    {
        speedValue = (int)newValue;
        strengthValue = 100 - speedValue;

        UpdateSliderValues(strengthValue, speedValue);
    }

    /// <summary>
    /// When the value of the strength slider changes, update the value of the text field
    /// and the sleed slider.
    /// </summary>
    /// <param name="newValue"></param>
    public void StrengthSliderChanged(float newValue)
    {
        strengthValue = (int)newValue;
        speedValue = 100 - strengthValue;

        UpdateSliderValues(strengthValue, speedValue);
    }

    /// <summary>
    /// Help function to update the text fields of the sliders
    /// </summary>
    /// <param name="strengthValue"></param>
    /// <param name="speedValue"></param>
    public void UpdateSliderValues(int strengthValue, int speedValue)
    {
        strengthSlider.value = strengthValue;
        speedSlider.value = speedValue;

        speedText.text = botSliderString + ":" + speedValue.ToString();
        strengthText.text = topSliderString + ":" + strengthValue.ToString();
    }


    /// <summary>
    /// Disables the GameObject holding the sliders
    /// </summary>
    public void DisableSliders()
    {
        gameObject.SetActive(false);
    }

    public void EnableSliders()
    {
        gameObject.SetActive(false);
    }


    public int GetStrength()
    {
        return strengthValue;
    }

    public int GetSpeed()
    {
        return speedValue;
    }

	/// <summary>
	/// Reset sliders to 50/50 strength/speed.
	/// </summary>
	public void ResetSliders()
	{
		StrengthSliderChanged (50); //Automatically updates speed as well.
	}

	public void SetSliderStrings(string top, string bot)
	{
		this.topSliderString = top;
		this.botSliderString = bot;
		strengthText.text = topSliderString + ": " + strengthValue.ToString();
		speedText.text = botSliderString + ": " + speedValue.ToString();
	}
}
