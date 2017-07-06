using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterInput
{
	private List<string> buttons;
	private Dictionary<string,string> assignedButtons;
	private Character character;

	public CharacterInput(Character character)
	{
		this.buttons = new List<string> ();
		this.assignedButtons = new Dictionary<string, string> ();
		this.character = character;
	}

	public void AddButton(string button)
	{
		if (button.Length == 1)
		{
			buttons.Add (button);
		}
	}

	public void RegisterButton(string button, string moveName)
	{
		if (buttons.Contains (button)) {
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
			if (assignedButtons.ContainsKey (button)) {
				character.DeleteMove (assignedButtons[button]);
				assignedButtons.Remove (button);
			}
			assignedButtons.Add (button, moveName);
		}
	}

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
}
