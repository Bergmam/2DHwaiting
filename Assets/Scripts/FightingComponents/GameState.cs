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
	private Text pressAnyKeyText;
	private bool gameOver;
	private bool pressAnyKeyToContinue;
	private bool paused;
	private ColorModifier pauseBackgroundToggel;
	private float timeUntilPressAnyKey;
	private GameObject pauseMenu;

	void Start ()
	{
		pauseMenu = GameObject.Find ("PauseMenu");
		pauseMenu.SetActive (false);
		timeUntilPressAnyKey = 2.5f;
		pauseBackgroundToggel = GameObject.Find ("PauseBackground").GetComponent<ColorModifier> ();
		pauseBackgroundToggel.SetDefaultColor (new Color32 (0, 0, 0, 0));
		pauseBackgroundToggel.SetSelectedColor (new Color32 (120, 120, 120, 160));
		healthBars = new ProgressBarBehaviour[StaticCharacterHolder.characters.Count];
		ProgressBarBehaviour character1HealthBar = GameObject.Find ("Character1HealthBar").GetComponent<ProgressBarBehaviour> ();
		healthBars [0] = character1HealthBar;
		ProgressBarBehaviour character2HealthBar = GameObject.Find ("Character2HealthBar").GetComponent<ProgressBarBehaviour> ();
		character2HealthBar.SetDirection (-1); //Flip the health bar on the right.
		healthBars[1] = character2HealthBar;
		winnerText = GameObject.Find ("WinnerText").GetComponent<Text> ();
		pressAnyKeyText = GameObject.Find ("PressAnyKeyText").GetComponent<Text> ();
		gameOver = false;
		paused = false;
	}

	void Update()
	{
		//Wait for 2.5 seconds then show the text saying "Press any key to continue".
		if (gameOver)
		{
			if (timeUntilPressAnyKey > 0)
			{
				timeUntilPressAnyKey -= Time.deltaTime;
			}
			else
			{
				pressAnyKeyText.enabled = true;
				pressAnyKeyToContinue = true;
			}
		}
		//Pause game when a player presses the escape key.
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
		//If presAnyKeyToContinue is true (which it is 2.5 seconds after a player dies), go back to move selection on key press.
		else if (pressAnyKeyToContinue && Input.anyKeyDown)
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
		pauseBackgroundToggel.Select ();
		pauseMenu.SetActive (true);
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
		pauseBackgroundToggel.DeSelect ();
		pauseMenu.SetActive (false);
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
		GameObject.Find ("Character " + loserNbr).GetComponent<Animator> ().SetBool ("Dead", true);
		winnerText.text = "PLAYER" + winner.GetNbr () + " WINS!";
		winnerText.enabled = true;
		gameOver = true;
	}
}
