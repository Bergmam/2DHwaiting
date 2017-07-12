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

	void Start ()
	{
		healthBars = new ProgressBarBehaviour[StaticCharacterHolder.characters.Count];
		ProgressBarBehaviour character1HealthBar = GameObject.Find ("Character1HealthBar").GetComponent<ProgressBarBehaviour> ();
		healthBars [0] = character1HealthBar;
		ProgressBarBehaviour character2HealthBar = GameObject.Find ("Character2HealthBar").GetComponent<ProgressBarBehaviour> ();
		character2HealthBar.SetDirection (-1); //Flip the health bar on the right.
		healthBars[1] = character2HealthBar;
		winnerText = GameObject.Find ("WinnerText").GetComponent<Text> ();
		gameOver = false;
	}

	void Update()
	{
		if (gameOver && Input.anyKeyDown)
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
		SetCharacterEnabled(GameObject.Find ("Character 1"), false);
		SetCharacterEnabled(GameObject.Find ("Character 2"), false);
	}

	/// <summary>
	/// Uns the pause game.
	/// Unpauses the game by enabling animation and control on both characters.
	/// </summary>
	public void UnPauseGame()
	{
		SetCharacterEnabled(GameObject.Find ("Character 1"), true);
		SetCharacterEnabled(GameObject.Find ("Character 2"), true);
	}

	public void GameOver(Character winner)
	{
		PauseGame ();
		int loserNbr = (winner.GetNbr () % 2) + 1;
		Animator loserAnimator = GameObject.Find ("Character " + loserNbr).GetComponentInChildren<Animator> ();
		loserAnimator.enabled = true;
		loserAnimator.SetBool ("Dead", true);
		winnerText.text = "PLAYER" + winner.GetNbr () + " WINS!";
		winnerText.enabled = true;
		gameOver = true;
	}

	/// <summary>
	/// Disable or enable the specified character according to the bool parameter.
	/// The enabled bool should be true for enabling and false for disabling.
	/// </summary>
	/// <param name="characterObject">Character object.</param>
	/// <param name="enabled">If set to <c>true</c> enabled.</param>
	private void SetCharacterEnabled(GameObject characterObject, bool enabled)
	{
		characterObject.GetComponent<InputController> ().enabled = enabled; //Change state of character controls.
		//Change state of collision detection.
		foreach (BodyPartTriggerHandler collisionHandler in characterObject.GetComponentsInChildren<BodyPartTriggerHandler>()) {
			collisionHandler.enabled = enabled;
		}
		//Change state of animation.
		foreach (Animator animator in characterObject.GetComponentsInChildren<Animator>()) {
			animator.enabled = enabled;
		}
	}
}
