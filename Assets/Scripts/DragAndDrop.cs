using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DragAndDrop : MonoBehaviour {

	private bool mouseDown;

    private bool outDown;
    private bool outUp;

    public float highTwistLimit;
    public float lowTwistLimit;

    void OnMouseDown() {
		mouseDown = true;
	}

	void OnMouseDrag() {
		if (mouseDown) {
            // Position relative to current camera bounds.
            Vector3 mousePos = Input.mousePosition;
            Vector3 parentPos = Camera.main.WorldToScreenPoint(transform.parent.position);

            bool mouseXGreater = mousePos.x > parentPos.x;
            bool mouseYGreater = mousePos.y > parentPos.y;

            float arctan = 0f;
            float newRot = 0f;

            if(mouseXGreater && mouseYGreater)  // 1st quadrant
            {
                arctan = Mathf.Atan2(mousePos.y - parentPos.y, mousePos.x - parentPos.x);
                newRot = Mathf.Rad2Deg * arctan + 90;
            }
            else if (!mouseXGreater && mouseYGreater) // 2nd quadrant
            {
                arctan = Mathf.Atan2(parentPos.x - mousePos.x, mousePos.y - parentPos.y);
                newRot = Mathf.Rad2Deg * arctan + 180;
            }
            else if (!mouseXGreater && !mouseYGreater) // 3rd quadrant
            {
                arctan = Mathf.Atan2(parentPos.y - mousePos.y, parentPos.x - mousePos.x);
                newRot = Mathf.Rad2Deg * arctan + 270;
            }
            else // 4th quadrant
            {
                arctan = Mathf.Atan2(mousePos.x - parentPos.x, parentPos.y - mousePos.y);
                newRot = Mathf.Rad2Deg * arctan;
            }

            //TODO: Look at twist limits based on mouse position and not one test update.

            transform.parent.eulerAngles = new Vector3(0, 0, newRot); //Update rotation previous to checking limits

            if (transform.parent.localEulerAngles.z < highTwistLimit) //Rotation is within limits, reset variables saying we are not
            {
                outUp = false;
                outDown = false;
            }else if (outDown) //Rotation is still outside limits to one side
            {
                transform.parent.localEulerAngles = new Vector3(0, 0, lowTwistLimit);
            }
            else if (outUp) //Rotation is still outside limits to other side
            {
                transform.parent.localEulerAngles = new Vector3(0, 0, highTwistLimit);

            //Check to which side rotation is going outside limits. Save this to keep rotation at that side until limit is reached again
            }else if(transform.parent.localEulerAngles.z > highTwistLimit + ((360 - highTwistLimit) / 2)) //TODO: Look this over
            {
                transform.parent.localEulerAngles = new Vector3(0, 0, lowTwistLimit);
                outDown = true;
            }
            else
            {
                transform.parent.localEulerAngles = new Vector3(0, 0, highTwistLimit);
                outUp = true;
            }
        }
	}

	void OnMouseUp() {
		mouseDown = false;
	}
}
