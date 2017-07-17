using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterCollisionDetector : MonoBehaviour
{
	private Character thisCharacter;
	private GameState gameState;
	private Rigidbody2D thisRigidbody;

	void Start ()
	{
		thisCharacter = GetComponent<InputController> ().GetCharacter ();
		thisRigidbody = GetComponent<Rigidbody2D> ();
		this.gameState = GameObject.Find ("Handler").GetComponent<GameState> ();
	}
	
	void OnTriggerEnter2D (Collider2D otherCollider)
	{
		Transform otherRootTransform = otherCollider.transform.root;
		GameObject otherCharacterObject = otherRootTransform.gameObject;
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

		//Apply knockback.
		Vector3 thisPosition = transform.position;
		Vector3 otherPosition = otherRootTransform.position;
		if (otherPosition.x < thisPosition.x)
		{
			thisRigidbody.AddForce (Vector2.right * 1000f);
		}
		else
		{
			thisRigidbody.AddForce (Vector2.left * 1000f);
		}

		otherCollider.enabled = false;
	}
}
