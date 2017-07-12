using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Game state. Updates controls and GUI based on the current state of the game.
/// </summary>
public class GameState : MonoBehaviour
{

	private ProgressBarBehaviour[] healthBars;
	private List<Character> characters;

	void Start ()
	{
		characters = StaticCharacterHolder.characters;
		healthBars = new ProgressBarBehaviour[StaticCharacterHolder.characters.Count];
		ProgressBarBehaviour character1HealthBar = GameObject.Find ("Character1HealthBar").GetComponent<ProgressBarBehaviour> ();
		healthBars [0] = character1HealthBar;
		ProgressBarBehaviour character2HealthBar = GameObject.Find ("Character2HealthBar").GetComponent<ProgressBarBehaviour> ();
		character2HealthBar.SetDirection (-1); //Flip the health bar on the right.
		healthBars[1] = character2HealthBar;
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
	}
}
