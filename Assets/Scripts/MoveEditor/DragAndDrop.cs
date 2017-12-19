using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum PivotDirection {North, West, South, East};

/// <summary>
/// Class for handling the dragging and dropping of limbs in the MoveEditor.
/// </summary>
public class DragAndDrop : MonoBehaviour {

	private bool mouseDown;

	private bool outHardDown;
	private bool outHardUp;

	private bool outFrameDown;
	private bool outFrameUp;

	private bool outHigh;
	private bool outLow;

	private float highFrameTwistLimit;
	private float lowFrameTwistLimit;
	public float frameTwistLimit;

	public float highHardTwistLimit;
	public float lowHardTwistLimit;

	public PivotDirection pivotDirection = PivotDirection.North;
	private int pivotRotationOffset;

	private GameObject highFrameLimitMarker;
	private GameObject lowFrameLimitMarker;
	private GameObject highHardLimitMarker;
	private GameObject lowHardLimitMarker;

	void Awake()
	{
		highFrameLimitMarker = GameObject.Find ("HighFrameLimitMarker");
		lowFrameLimitMarker = GameObject.Find ("LowFrameLimitMarker");
		highHardLimitMarker = GameObject.Find ("HighHardLimitMarker");
		lowHardLimitMarker = GameObject.Find ("LowHardLimitMarker");
	}

	void Start()
	{
		UpdateFrameLimits ();
		switch (pivotDirection)
		{
			case PivotDirection.North:
				pivotRotationOffset = 0;
				break;
			case PivotDirection.West:
				pivotRotationOffset = 90;
				break;
			case PivotDirection.South:
				pivotRotationOffset = 180;
				break;
			default : 
				pivotRotationOffset = 270; //PivotDirection.East:
				break;
		}
	}

    void OnMouseDown()
	{
		mouseDown = true;
		EnableMarker (highFrameLimitMarker, highFrameTwistLimit);
		EnableMarker (lowFrameLimitMarker, lowFrameTwistLimit);
		EnableMarker (highHardLimitMarker, highHardTwistLimit);
		EnableMarker (lowHardLimitMarker, lowHardTwistLimit);
	}

	private void EnableMarker(GameObject marker, float rotation)
	{
		marker.transform.position = transform.parent.position;
		marker.transform.parent = transform.parent.parent;
		marker.transform.localEulerAngles = new Vector3 (0, 0, rotation + pivotRotationOffset);
		marker.SetActive (true);
	}

	void OnMouseUp()
	{
		mouseDown = false;
		highFrameLimitMarker.SetActive (false);
		lowFrameLimitMarker.SetActive (false);
		highHardLimitMarker.SetActive (false);
		lowHardLimitMarker.SetActive (false);
	}

	/// <summary>
	/// Updates the twist limits to fit the new angle of the moved bodypart.
	/// </summary>
	public void UpdateFrameLimits()
	{
		float previousRotation = transform.parent.localEulerAngles.z;
		//Mod 360 to make sure both limits are within [0 - 360]
		highFrameTwistLimit = (previousRotation + frameTwistLimit) % 360;
		lowFrameTwistLimit = (previousRotation - frameTwistLimit + 360) % 360;
	}

	/// <summary>
	/// Converts the mouse pointer world rotation to the local rotation of the bodypart pivot.
	/// </summary>
	/// <returns>The local rotation of the pointer around the bodypart pivot.</returns>
	float LocalMouseRotation()
	{
		// Position relative to current camera bounds.
		Vector3 mousePos = Input.mousePosition;
		Vector3 parentPos = Camera.main.WorldToScreenPoint(transform.parent.position);

		bool mouseXGreater = mousePos.x > parentPos.x;
		bool mouseYGreater = mousePos.y > parentPos.y;

		float arctan = 0f;
		float newRot = 0f;

		// 1st quadrant
		if(mouseXGreater && mouseYGreater)  
		{
			arctan = Mathf.Atan2(mousePos.y - parentPos.y, mousePos.x - parentPos.x);
			newRot = Mathf.Rad2Deg * arctan + 90;
		}
		// 2nd quadrant
		else if (!mouseXGreater && mouseYGreater) 
		{
			arctan = Mathf.Atan2(parentPos.x - mousePos.x, mousePos.y - parentPos.y);
			newRot = Mathf.Rad2Deg * arctan + 180;
		}
		// 3rd quadrant
		else if (!mouseXGreater && !mouseYGreater) 
		{
			arctan = Mathf.Atan2(parentPos.y - mousePos.y, parentPos.x - mousePos.x);
			newRot = Mathf.Rad2Deg * arctan + 270;
		}
		// 4th quadrant
		else 
		{
			arctan = Mathf.Atan2(mousePos.x - parentPos.x, parentPos.y - mousePos.y);
			newRot = Mathf.Rad2Deg * arctan;
		}
		return newRot;
	}

	/// <summary>
	/// Main method for getting the mouse position, regognizing a click, and moving the relevant bodypart accordingly.
	/// </summary>
	void OnMouseDrag() 
	{
		if (mouseDown) 
		{ 
			float newRot = LocalMouseRotation ();
            //TODO: Look at twist limits based on mouse position and not one test update.

			newRot += pivotRotationOffset;

            transform.parent.eulerAngles = new Vector3(0, 0, newRot); //Update rotation previous to checking limits

			//Find biggest of low limits and smallest of high limis to create the smallest allowed intervall
			float tmpLowLimit = RotationUtils.InCounterClockwiseLimits (lowFrameTwistLimit, lowHardTwistLimit, highHardTwistLimit) ? lowFrameTwistLimit : lowHardTwistLimit;
			float tmpHighLimit = RotationUtils.InCounterClockwiseLimits (highFrameTwistLimit, lowHardTwistLimit, highHardTwistLimit) ? highFrameTwistLimit : highHardTwistLimit;

			float rotation = transform.parent.localEulerAngles.z;

			if (RotationUtils.InCounterClockwiseLimits (rotation, tmpLowLimit, tmpHighLimit)) 
			{ 
				//Angle in limit
				outHigh = false;
				outLow = false;	
			} else 
			{
                if (outHigh) //Rotation is still outside limits to one side
				{
					transform.parent.localEulerAngles = new Vector3 (0, 0, tmpHighLimit);
				}
				else if (outLow) //Rotation is still outside limits to the other side
				{ 
					transform.parent.localEulerAngles = new Vector3 (0, 0, tmpLowLimit);
				}
				//Check to which side rotation has exited the limits intervall
				else if (RotationUtils.InCounterClockwiseLimits(rotation, RotationUtils.MiddleOfRotations (tmpHighLimit, tmpLowLimit), tmpLowLimit))
				{
					outLow = true;
					transform.parent.localEulerAngles = new Vector3 (0, 0, tmpLowLimit);
				}
				else
				{
					outHigh = true;
					transform.parent.localEulerAngles = new Vector3 (0, 0, tmpHighLimit);
				}
			}
        }
	}
}
