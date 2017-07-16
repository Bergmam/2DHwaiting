using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterCollisionDetector : MonoBehaviour
{
	private Character thisCharacter;
	private GameState gameState;

	void Start ()
	{
		thisCharacter = transform.root.gameObject.GetComponent<InputController> ().GetCharacter ();
		this.gameState = GameObject.Find ("Handler").GetComponent<GameState> ();
	}
	
	void OnTriggerEnter2D (Collider2D otherCollider)
	{
		GameObject otherCharacterObject = otherCollider.transform.root.gameObject;
		InputController otherInputController = otherCharacterObject.GetComponent<InputController> (); //Get attacking character's InputController
		if (otherInputController == null)
		{
			return; //If colliding object's root does not have an InputController, it is not a character.
		}
		Character otherCharacter = otherInputController.GetCharacter ();
		Move move = otherInputController.GetCurretlyPlayedMove ();
		if (otherCharacter == null || move == null || otherCharacter.Equals (thisCharacter))
		{
			return; //Make sure other character object has all necessary info.
		}
		thisCharacter.ApplyMoveTo (move); //Apply damage in model.
		gameState.UpdateCharacterHealth (thisCharacter); //Update health bars and check winner.
		otherCollider.enabled = false;

		//TODO: Implement knockback.
	}
}
