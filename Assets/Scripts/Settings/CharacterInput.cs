using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterInput
{
	private List<string> buttons; //All buttons used by the character.
	private Dictionary<string,string> assignedButtons; //All buttons that have an assigned move together with the name of that move.
	private Character character;

	/// <summary>
	/// Initializes a new instance of the <see cref="CharacterInput"/> class.
	/// </summary>
	/// <param name="character">Character.</param>
	public CharacterInput(Character character)
	{
		this.buttons = new List<string> ();
		this.assignedButtons = new Dictionary<string, string> ();
		this.character = character;
	}

	/// <summary>
	/// Adds a button to the list of used buttons.
	/// </summary>
	/// <param name="button">The button. Has to be of length 1.</param>
	public void AddButton(string button)
	{
		if (button.Length == 1)
		{
			buttons.Add (button);
		}
	}

	/// <summary>
	/// Register the name of a move to a button used by the character.
	/// </summary>
	/// <param name="button">Button.</param>
	/// <param name="moveName">Move name.</param>
	public void RegisterButton(string button, string moveName)
	{
		if (buttons.Contains (button)) { //The button must be used by the character in order for a move to be assigned to it.
			//If the move is used by another button, remove that assignment.
			string sameMoveButton = "";
			foreach(KeyValuePair<string,string> entry in assignedButtons)
			{
				if (entry.Value.Equals (moveName))
				{
					sameMoveButton = entry.Key;
					break;
				}
			}
			if(!sameMoveButton.Equals(""))
			{
				assignedButtons.Remove (sameMoveButton);
			}
			//If the button is currently used for another move, remove that move from the character and remove the assignment.
			if (assignedButtons.ContainsKey (button)) {
				character.DeleteMove (assignedButtons[button]);
				assignedButtons.Remove (button);
			}
			assignedButtons.Add (button, moveName); //Assign the new move to the specified button.
		}
	}

	/// <summary>
	/// Determines whether this instance has the specified button.
	/// </summary>
	/// <returns><c>true</c> if this instance has the specified button; otherwise, <c>false</c>.</returns>
	/// <param name="button">The button.</param>
	public bool HasButton(string button)
	{
		return buttons.Contains (button);
	}

	public void SetCharacter(Character character)
	{
		this.character = character;
	}

	public Character GetCharacter()
	{
		return this.character;
	}
		
	public string GetMoveName(string button)
	{
		return assignedButtons [button];
	}

	public bool AllButtonsAssigned()
	{
		return buttons.Count == assignedButtons.Keys.Count;
	}
}
