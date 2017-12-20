using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ListKeyboardManager : MonoBehaviour {

	private int selectedListIndex = 0; //The index of the currently selected list item.
	private Button[] listItems; //Object used for interacting with the underlying list items.
	private float scrollDelay;

	void Awake()
	{
		listItems = new Button[transform.childCount];
		for (int i = 0; i < transform.childCount; i++)
		{
			Transform child = transform.GetChild (i);
			Button button = child.GetComponent<Button> ();
			if (button != null)
			{
				listItems [i] = button;
			}
		}


		for (int i = 0; i < listItems.Length - 1; i++)
		{
			float biggerY = listItems [i].gameObject.GetComponent<RectTransform> ().anchorMin.y;
			float smallerY = listItems [i + 1].gameObject.GetComponent<RectTransform> ().anchorMin.y;
			print (listItems [i].transform.name + " is at " + smallerY + ", " + listItems [i + 1].transform.name + " is at " + biggerY);
			if (smallerY > biggerY) {
				Button tmp = listItems [i];
				listItems [i] = listItems [i + 1];
				listItems [i + 1] = tmp;
			}
		}

		for (int i = 0; i < listItems.Length; i++)
		{
			print (i + ": " + listItems [i].transform.name);
		}
	}

	void Start()
	{
		MoveSelection (0);
	}

	void Update ()
	{
		bool vertical1Up = Input.GetAxisRaw("Vertical") > 0;
		bool vertical1Down = Input.GetAxisRaw("Vertical") < 0;
		bool vertical2Up = Input.GetAxisRaw("Vertical2") > 0;
		bool vertical2Down = Input.GetAxisRaw("Vertical2") < 0;

		if (scrollDelay > 0)
		{
			scrollDelay -= Time.deltaTime;
		}

		else
		{
			if ((vertical1Up || vertical2Up) && scrollDelay <= 0 && listItems != null) //Up arrow pressed
			{
				scrollDelay = Parameters.scrollDelay;
				MoveSelection(-1);
			}
			else if ((vertical1Down || vertical2Down) && scrollDelay <= 0 && listItems != null) //Down arrow pressed
			{
				scrollDelay = Parameters.scrollDelay;
				MoveSelection(1);
			}
		}

		if (Input.GetKeyDown("enter") || Input.GetKeyDown("return"))
		{
			listItems [selectedListIndex].onClick.Invoke ();
		}
	}

	private void MoveSelection(int steps)
	{
		int newIndex = selectedListIndex + steps;
		if (newIndex >= 0 && newIndex < listItems.Length)
		{
			listItems [selectedListIndex].Select ();
			selectedListIndex = newIndex;
		}
	}
}
