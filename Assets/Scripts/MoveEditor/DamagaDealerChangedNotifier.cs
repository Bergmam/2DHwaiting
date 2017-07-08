using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Move wrapper that tells listeners when a new damage dealer has been set.
/// Used instead of move directly in order to keep move serializable.
/// </summary>
public class DamagaDealerChangedNotifier
{
	private Move move;
	//Delegate is used as type for any listener of the event. New listeners are added by simply adding (+=) them to the OnDamageDealerChanged event object.
	//Any listener must be a function with the same return type and parameters as the DamageDealerChangedAction delegate.
	public delegate void DamageDealerChangedAction(string newDamageDealerName);
	public event DamageDealerChangedAction onDamageDealerChanged;

	public DamagaDealerChangedNotifier(Move move)
	{
		this.move = move;
	}

	/// <summary>
	/// Sets the damage dealer and notifies any listeners of the onDamageDealerChanged event.
	/// </summary>
	/// <param name="newDamageDealerName">New damage dealer name.</param>
	public void SetDamageDealer(string newDamageDealerName)
	{
		if (!newDamageDealerName.Equals (move.GetDamageDealer())) { //Make sure new is not the same as old.
			move.SetDamageDealer(newDamageDealerName);
			MonoBehaviour.print ("New damage dealer is " + newDamageDealerName);
			if (onDamageDealerChanged != null) { //event is null if no listeners have been attached.
				onDamageDealerChanged (newDamageDealerName);
			}
		}
	}

	public void SetMove(Move move)
	{
		this.move = move;
	}

	public Move GetMove()
	{
		return this.move;
	}
}
