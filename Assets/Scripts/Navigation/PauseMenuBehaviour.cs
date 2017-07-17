using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PauseMenuBehaviour : MonoBehaviour
{
	/// <summary>
	/// Goes back to main menu.
	/// </summary>
	public void GoToMainMenu()
	{
		InputSettings.ClearRegisteredMoves ();
		StaticCharacterHolder.ResetCharacters ();
		SceneHandler.GoBackToScene ("MainMenuScene");
	}

	/// <summary>
	/// Goes back to move selection.
	/// </summary>
	public void GoToMoveSelection()
	{
		InputSettings.ClearRegisteredMoves ();
		StaticCharacterHolder.ResetCharacters ();
		SceneHandler.GoBackToScene ("MoveSelectionScene");
	}
}
