using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DropdownBehaviour : MonoBehaviour
{
	private Dropdown dropdown;
	private bool blockMove; //True if the move is a block, false otherwise.
	private string highlightedLabelText; //The text of the label in the dropdown that has been selected but not yet pressed.
	private string activeBodypart;
	private Text label; //The label of the panel. Used to swich label text depending on move type.

	void Start ()
	{
		this.label = GameObject.Find ("DamageDealerLabelText").GetComponent<Text> ();
		dropdown = gameObject.GetComponent<Dropdown> ();
		dropdown.ClearOptions (); //Clear dropdown of any default items.
		GameObject character = GameObject.Find ("Character");
		//Find all parent objects of objects with a DragAndDrop script attached (bodyparts)
		// and add their names to the dropdown list.
		DragAndDrop[] dragAndDropInstances = character.GetComponentsInChildren<DragAndDrop> ();
		List<string> bodyPartNames = new List<string> ();
		foreach (DragAndDrop dragAndDropInstance in dragAndDropInstances)
		{
			GameObject dragPoint = dragAndDropInstance.gameObject;
			Transform bodyPartTransform = dragPoint.transform.parent;
			bodyPartNames.Add (bodyPartTransform.name);
		}
		dropdown.AddOptions (bodyPartNames);
		DropdownValueChanged ();
	}

	/// <summary>
	/// When a new bodypart is highlighted in the list, deselect the currrent active bodypart on the character and select the new one.
	/// If move is a block, show the shield.
	/// </summary>
	/// <param name="highlightedLabel">Highlighted label.</param>
	public void HighlightChanged(string newHighlightedLabelText)
	{
		if (newHighlightedLabelText.Equals (highlightedLabelText))
		{
			return; //Do nothing if the mouse moves out and back in of the same label.
		}
		if (highlightedLabelText != null)
		{
			GameObject.Find (highlightedLabelText).GetComponent<ColorModifier> ().DeSelect (); //Deselect any previous attacking bodypart.
			GameObject previousShield = GameObject.Find (highlightedLabelText.Replace (" ", "") + "Shield"); //Shields use camel case naming without spaces.
			if (previousShield != null)
			{
				previousShield.GetComponent<SpriteRenderer> ().enabled = false; //Hide any previous shield.
			}
		}
		if (blockMove)
		{
			string noSpaceText = newHighlightedLabelText.Replace (" ", ""); //Shields use camel case naming without spaces.
			GameObject newShield = GameObject.Find (noSpaceText + "Shield");
			if (newShield != null)
			{
				newShield.GetComponent<SpriteRenderer> ().enabled = true; //Show the new shield.
				//Keep the scale.
				Vector3 shieldScale = GameObject.Find (activeBodypart.Replace (" ", "") + "Shield").transform.localScale;
				newShield.transform.localScale = shieldScale;
			}
		}
		else
		{ //If move is not a block move, change color of attacking bodypart.
			GameObject.Find (newHighlightedLabelText).GetComponent<ColorModifier> ().Select ();
		}
		highlightedLabelText = newHighlightedLabelText;
	}

	/// <summary>
	/// When an item is selected in the dropdown, it becomes the new active bodypart.
	/// </summary>
	public void DropdownValueChanged()
	{
		//Make sure mouse does not hover on a list item after method has executed, highlighting the wrong bodypart.
		foreach (DropdownItemBehaviour itemBehaviour in GetComponentsInChildren<DropdownItemBehaviour>())
		{
			itemBehaviour.enabled = false;
		}
		string activeBodypart = dropdown.options [dropdown.value].text;

		GameObject newShield = GameObject.Find (activeBodypart.Replace (" ", "") + "Shield");
		if (newShield != null)
		{
			newShield.GetComponent<SpriteRenderer> ().enabled = blockMove;
		}
		GameObject.Find (activeBodypart).GetComponent<ColorModifier> ().SetSelected (!blockMove);
		SetActiveBodypart (activeBodypart);
	}

	public void SetActiveBodypart(string newActiveBodypart)
	{
		this.activeBodypart = newActiveBodypart;
	}

	public string GetActiveBodypart()
	{
		return this.activeBodypart;
	}

	public void SetLabelText(string text)
	{
		this.label.text = text;
	}

	public void SetBlockMove(bool blockMove)
	{
		this.blockMove = blockMove;
	}
}
