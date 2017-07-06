using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class InputSettings
{

	private static List<CharacterInput> characterInputs = new List<CharacterInput>();
	public static List<string> registeredButtons = new List<string>();

	/*private static Dictionary<int,List<string>> playerButtons = new Dictionary<int, List<string>> ();

	public static void AddPlayer1Button(string button){
		AddPlayerButton (button, 0);
	}

	public static void AddPlayer2Button(string button){
		AddPlayerButton (button, 1);
	}

	private static void AddPlayerButton(string button, int playerNumber){
		if (button.Length == 1) {
			playerButtons [playerNumber].Add (button);
		}
	}*/

	public static void Init()
	{
		foreach (Character character in StaticCharacterHolder.characters) {
			CharacterInput characterInput = new CharacterInput (character);
			characterInputs.Add (characterInput);
		}

		//TODO: Read buttons from settings
		foreach (char c in "qwe") {
			AddButton (c + "", StaticCharacterHolder.characters [0]);
		}
		foreach (char c in "iop") {
			AddButton (c + "", StaticCharacterHolder.characters [1]);
		}
	}

	public static void AddButton(string button, Character character)
	{
		if (!registeredButtons.Contains (button)) {
			registeredButtons.Add (button);
			foreach (CharacterInput characterInput in characterInputs)
			{
				if (characterInput.GetCharacter ().Equals (character))
				{
					characterInput.AddButton (button);
				}
			}
		}
	}

	public static Character Register(string button, string moveName)
	{
		foreach (CharacterInput characterInput in characterInputs)
		{
			if (characterInput.HasButton (button))
			{
				characterInput.RegisterButton (button, moveName);
				return characterInput.GetCharacter();
			}
		}
		return null;
	}
}
