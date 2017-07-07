using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

/// <summary>
/// Used to select a body part as the damage dealer by clicking it with the mouse.
/// </summary>
public class DamageDealerSelector : MonoBehaviour
{

	private Move move;
	private string name; //Name of the body part.
	private bool selected;

	void Start ()
	{
		this.selected = false;
		this.name = gameObject.name;
	}

	void OnMouseDown()
	{
		if (this.move != null)
		{
			move.SetDamageDealer (name);
			this.Select ();
		}
	}

	/// <summary>
	/// Sets the move and sets DamageDealerChanged as the method to be called when the damage dealer changes.
	/// </summary>
	/// <param name="move">Move.</param>
	public void SetMove(Move move)
	{
		if (move != null)
		{
			this.move = move;
			this.move.onDamageDealerChanged += DamageDealerChanged; //Register DamageDealerChanged as listener to the onDamageDealerChanged event in Move.
		}
	}

	/// <summary>
	/// Called by the event of the move this selector listens to when the damage dealing body part changes.
	/// </summary>
	/// <param name="newDamageDealerName">New damage dealer name.</param>
	void DamageDealerChanged(string newDamageDealerName)
	{
		if (!newDamageDealerName.Equals (name))
		{
			this.DeSelect ();
		}
	}

	public void Select()
	{
		this.selected = true;
		//Change color or highlight in some way.
	}

	public void DeSelect()
	{
		//TODO: remove this print.
		if (selected)
		{
			print ("Deselected " + name);
		}
		this.selected = false;
		//Change back to original color and remove any highlighting.
	}
}
