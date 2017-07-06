using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Holds information on which buttons are used in the game.
/// Handles registrations of moves to buttons by delegating the registrations to the <see cref="CharacterInput"/> that uses the button to be registrated.
/// </summary>
public static class InputSettings
{
	private static List<CharacterInput> characterInputs = new List<CharacterInput>();

	//All buttons used in the game. Can be itterated over to check if any of them has been pressed.
	public static List<string> allUsedButtons = new List<string>();

	/// <summary>
	/// Create a CharacterInput object for each character in the game.
	/// Read all buttons from the saved settings.
	/// </summary>
	public static void Init()
	{
		//Create a CharacterInput object for each character in the game.
		foreach (Character character in StaticCharacterHolder.characters) {
			CharacterInput characterInput = new CharacterInput (character);
			characterInputs.Add (characterInput);
		}

		//Register some buttons for each character TODO: Read buttons from file
		foreach (char c in "qwe") {
			AddButton (c + "", StaticCharacterHolder.characters [0]);
		}
		foreach (char c in "iop") {
			AddButton (c + "", StaticCharacterHolder.characters [1]);
		}
	}

	/// <summary>
	/// Add a button to the list of used buttons for the specified character.
	/// </summary>
	/// <param name="button">The button to be added.</param>
	/// <param name="character">The character which is to use the button.</param>
	public static void AddButton(string button, Character character)
	{
		if (button.Length == 1 && !allUsedButtons.Contains (button)) { //Make sure button is not already in use and that it is just one character long.
			allUsedButtons.Add (button);
			foreach (CharacterInput characterInput in characterInputs)
			{
				if (characterInput.GetCharacter ().Equals (character))
				{
					characterInput.AddButton (button);
					break;
				}
			}
		}
	}

	/// <summary>
	/// Register the move name to the specified button in the CharacterInput object which handles that button.
	/// </summary>
	/// <param name="button">Button.</param>
	/// <param name="moveName">Move name.</param>
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


	/// <summary>
	/// Checks if the specified character has a move assigned to the specified button.
	/// </summary>
	/// <returns><c>true</c> if the character has the specified button; otherwise, <c>false</c>.</returns>
	/// <param name="characterIndex">The characterIndex to check.</param>
	/// <param name="button">The button that was pressed</param>
	public static bool HasButton(int characterIndex, string button)
	{
		foreach(CharacterInput charInput in characterInputs)
		{
			MonoBehaviour.print ("charInput.GetCharacter().GetNbr(): " + charInput.GetCharacter().GetNbr() + ", characterIndex: " + characterIndex);
			if (charInput.HasButton(button) && charInput.GetCharacter().GetNbr() == characterIndex)
			{
				return true;
			}
		}
		return false;
	}


	/// <summary>
	/// Returns a move linked to the specified button.
	/// </summary>
	/// <returns>The move.</returns>
	/// <param name="button">The pressed button.</param>
	public static string GetMoveName(string button)
	{
		foreach (CharacterInput charInput in characterInputs) 
		{
			if(charInput.HasButton(button))
			{
				return charInput.GetMoveName(button);
			}
		}
		return null;
	}
}
