using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DropdownBehaviour : MonoBehaviour
{
	private Dropdown dropdown;
	private string highLightedLabelText;
	private string damageDealer;

	void Start ()
	{
		dropdown = gameObject.GetComponent<Dropdown> ();
		dropdown.ClearOptions (); //Clear dropdown of any default items.
		GameObject character = GameObject.Find ("Character");
		//Find all parent objects of objects with a <see cref="DragAndDrop"/> script attached (bodyparts)
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
	}

	/// <summary>
	/// When a new bodypart is highlighted in the list, deselect the currrent bodypart on the character and select the new one.
	/// </summary>
	/// <param name="highlightedLabel">Highlighted label.</param>
	public void HighlightChanged(string newHighlightedLabelText)
	{
		if (!newHighlightedLabelText.Equals (highLightedLabelText))
		{
			if (highLightedLabelText != null)
			{
				GameObject.Find (highLightedLabelText).GetComponent<ColorModifier> ().DeSelect ();
			}
			GameObject.Find (newHighlightedLabelText).GetComponent<ColorModifier> ().Select ();
			highLightedLabelText = newHighlightedLabelText; //The damage dealer is always the currently selected bodypart.
		}
	}

	/// <summary>
	/// When an item is selected in the dropdown, it becomes the new damage dealer.
	/// </summary>
	public void DropdownValueChanged(){
		//Make sure mouse does not hover on a list item after method has executed, highlighting the wrong bodypart.
		foreach (DropdownItemBehaviour itemBehaviour in GetComponentsInChildren<DropdownItemBehaviour>())
		{
			itemBehaviour.enabled = false;
		}
		string damageDealer = dropdown.options [dropdown.value].text;
		damageDealer.Replace (" ", string.Empty); //Label automatically inserts spaces when it reads camel case. This removes those spaces.
		GameObject.Find (damageDealer).GetComponent<ColorModifier> ().Select ();
		SetDamageDealer (damageDealer);
	}

	public void SetDamageDealer(string newDamageDealer){
		this.damageDealer = newDamageDealer;
	}

	public string GetDamageDealer()
	{
		return this.damageDealer;
	}
}
