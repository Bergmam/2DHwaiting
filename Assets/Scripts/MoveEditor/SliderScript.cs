using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SliderScript : MonoBehaviour {

    public Text strengthText;
    public Text speedText;
    public Slider strengthSlider;
    public Slider speedSlider;

    private int strengthValue;
    private int speedValue;

    // Use this for initialization
    void Start () {
      //  strengthSlider = GameObject.Find("StrengthSlider").GetComponent<Slider>();
        //speedSlider = GameObject.Find("SpeedSlider").GetComponent<Slider>();

        strengthValue = (int)strengthSlider.value;
        speedValue = (int)speedSlider.value;

        strengthText.text = "Strength: " + strengthValue.ToString();
        speedText.text = "Speed: " + speedValue.ToString();
	}
	
	// Update is called once per frame
	void Update () {
		
	}


    public void SpeedSliderChanged(float newValue)
    {
        speedValue = (int)newValue;
        strengthValue = 100 - speedValue;

        UpdateSliderValues(strengthValue, speedValue);
    }

    public void StrengthSliderChanged(float newValue)
    {
        strengthValue = (int)newValue;
        speedValue = 100 - strengthValue;

        UpdateSliderValues(strengthValue, speedValue);
    }

    public void UpdateSliderValues(int strengthValue, int speedValue)
    {
        strengthSlider.value = strengthValue;
        speedSlider.value = speedValue;

        speedText.text = "Speed: " + speedValue.ToString();
        strengthText.text = "Strength: " + strengthValue.ToString();
    }
}
