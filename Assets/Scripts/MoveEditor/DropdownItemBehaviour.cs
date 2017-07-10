using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

/// <summary>
/// This script is automatically attached to any item in the bodypart dropdown. 
/// It notifies the <see cref="DropdownBehaviour"/> class when the mouse hovers on the list item it is attached to.
/// </summary>
public class DropdownItemBehaviour : MonoBehaviour, IPointerEnterHandler
{
	private DropdownBehaviour dropdown;

	void Start()
	{
		dropdown = GameObject.Find ("BodypartDropdown").GetComponent<DropdownBehaviour> ();
	}

	/// <summary>
	/// Notifies the <see cref="DropdownBehaviour"/> class that the mouse has entered the item this script is attached to.
	/// </summary>
	/// <param name="eventData">Event data.</param>
	public void OnPointerEnter(PointerEventData eventData)
	{
		string bodyPartName = transform.Find ("Item Label").GetComponent<Text> ().text;
		bodyPartName.Replace (" ", string.Empty); //Label automatically inserts spaces when it reads camel case. This removes those spaces.
		dropdown.HighlightChanged (bodyPartName);
	}
}

