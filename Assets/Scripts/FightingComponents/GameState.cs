using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// Game state. Updates controls and GUI based on the current state of the game.
/// </summary>
public class GameState : MonoBehaviour
{
	private ProgressBarBehaviour[] healthBars;
	private Text winnerText;
	private bool gameOver;
	private bool paused;
	private ColorModifier pausePanelToggel;

	void Start ()
	{
		pausePanelToggel = GameObject.Find ("PauseGreyPanel").GetComponent<ColorModifier> ();
		pausePanelToggel.SetDefaultColor (new Color32 (0, 0, 0, 0));
		pausePanelToggel.SetSelectedColor (new Color32 (120, 120, 120, 160));
		healthBars = new ProgressBarBehaviour[StaticCharacterHolder.characters.Count];
		ProgressBarBehaviour character1HealthBar = GameObject.Find ("Character1HealthBar").GetComponent<ProgressBarBehaviour> ();
		healthBars [0] = character1HealthBar;
		ProgressBarBehaviour character2HealthBar = GameObject.Find ("Character2HealthBar").GetComponent<ProgressBarBehaviour> ();
		character2HealthBar.SetDirection (-1); //Flip the health bar on the right.
		healthBars[1] = character2HealthBar;
		winnerText = GameObject.Find ("WinnerText").GetComponent<Text> ();
		gameOver = false;
		paused = false;
	}

	void Update()
	{
		if (Input.GetKeyDown (KeyCode.Escape))
		{
			if (paused) {
				UnPauseGame ();
			} 
			else
			{
				PauseGame ();
			}
		}
		else if (gameOver && Input.anyKeyDown)
		{
			InputSettings.ClearRegisteredMoves ();
			StaticCharacterHolder.ResetCharacters ();
			SceneHandler.GoBack ();
		}
	}

	/// <summary>
	/// Called when the health of a character changes. Updates the health bar of that character.
	/// </summary>
	/// <param name="character">Character.</param>
	public void UpdateCharacterHealth (Character character)
	{
		float maxHealth = (float)character.GetMaxHealth ();
		float health = (float)character.GetHealth ();
		float percentage = health / maxHealth;
		int characterIndex = character.GetNbr() - 1;
		healthBars [characterIndex].UpdateFill (percentage);

		//Freeze game if a player dies.
		if (health <= 0)
		{
			GameOver (StaticCharacterHolder.characters [(characterIndex + 1) % 2]); //Other player wins
		}
	}

	/// <summary>
	/// Pauses the game by disabling animation and control on both characters.
	/// </summary>
	public void PauseGame()
	{
		paused = true;
		pausePanelToggel.Select ();
		GameObject.Find ("Character 1").GetComponent<InputController> ().Pause ();
		GameObject.Find ("Character 2").GetComponent<InputController> ().Pause ();
	}

	/// <summary>
	/// Uns the pause game.
	/// Unpauses the game by enabling animation and control on both characters.
	/// </summary>
	public void UnPauseGame()
	{
		paused = false;
		pausePanelToggel.DeSelect ();
		GameObject.Find ("Character 1").GetComponent<InputController> ().UnPause ();
		GameObject.Find ("Character 2").GetComponent<InputController> ().UnPause ();
	}

	/// <summary>
	/// Call when game is over. The losing character's death animation is played and a text saying who won is displayed.
	/// </summary>
	/// <param name="winner">Winner.</param>
	public void GameOver(Character winner)
	{
		GameObject.Find ("Character 1").GetComponent<InputController> ().enabled = false;
		GameObject.Find ("Character 2").GetComponent<InputController> ().enabled = false;
		int loserNbr = (winner.GetNbr () % 2) + 1;
		GameObject.Find ("Character " + loserNbr).GetComponentInChildren<Animator> ().SetBool ("Dead", true);
		winnerText.text = "PLAYER" + winner.GetNbr () + " WINS!";
		winnerText.enabled = true;
		gameOver = true;
	}
}
