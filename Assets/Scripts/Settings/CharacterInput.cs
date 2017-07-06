using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterInput
{
	private List<string> buttons;
	private Dictionary<string,string> buttonMoveNameDict;
	private Character character;

	public CharacterInput(Character character)
	{
		this.buttons = new List<string> ();
		this.buttonMoveNameDict = new Dictionary<string, string> ();
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
			foreach(KeyValuePair<string,string> entry in buttonMoveNameDict)
			{
				if (entry.Value.Equals (moveName))
				{
					sameMoveButton = entry.Key;
				}
			}
			if(!sameMoveButton.Equals(""))
			{
				buttonMoveNameDict.Remove (sameMoveButton);
			}
			if (buttonMoveNameDict.ContainsKey (button)) {
				character.DeleteMove (buttonMoveNameDict[button]);
				buttonMoveNameDict.Remove (button);
			}
			buttonMoveNameDict.Add (button, moveName);
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
}
