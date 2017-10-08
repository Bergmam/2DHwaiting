using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShieldCollisionDetector : MonoBehaviour {

	public int characterIndex;
	private Character character;
	private GameState gameState;
	private Rigidbody2D rigidBody;
	private InputController inputController;

	void Start ()
	{
		this.inputController = transform.root.gameObject.GetComponent<InputController> ();
		this.character = StaticCharacterHolder.characters[characterIndex - 1];
		this.rigidBody = transform.root.gameObject.GetComponent<Rigidbody2D> ();
		this.gameState = GameObject.Find ("Handler").GetComponent<GameState> ();
	}

	void OnTriggerEnter2D (Collider2D otherCollider)
	{
		Transform otherRootTransform = otherCollider.transform.root;
		GameObject otherCharacterObject = otherRootTransform.gameObject;
		InputController otherInputController = otherCharacterObject.GetComponent<InputController> ();
		if (otherInputController == null)
		{
			return; //If colliding object's root does not have an InputController, it is not a character.
		}
		Character otherCharacter = otherInputController.GetCharacter ();
		Move damagingMove = otherInputController.GetCurretlyPlayedMove ();
		if (otherCharacter == null || damagingMove == null || damagingMove.IsBlockMove() 
            || otherCharacter.Equals (this.character) || otherCollider.transform.tag.Equals("characterCollider"))
		{
			return; //Make sure other character object has all necessary info.
		}

		Move blockMove = this.inputController.GetCurretlyPlayedMove ();
		if (blockMove == null) {
			return;
		}

		this.character.ApplyMoveTo (damagingMove, blockMove); //Apply damage in model.
		otherCollider.enabled = false; //Make sure the other character's damaging bodypart does not also collide with character behind shield.
        otherInputController.PauseSeconds(Parameters.stunTimeModifier * damagingMove.GetStrength());
		gameState.UpdateCharacterHealth (this.character); //Update health bars and check winner.

		//Apply knockbacks.
		Vector3 thisPosition = transform.position;
		Vector3 otherPosition = otherRootTransform.position;
		if (otherPosition.x < thisPosition.x)
		{
			this.rigidBody.AddForce (Vector2.right * Parameters.blockKnockbackModifier);
			this.inputController.KnockBack (); //Make the character unable to move while being knocked back.
		}
		else
		{
			this.rigidBody.AddForce (Vector2.left * Parameters.blockKnockbackModifier);
			this.inputController.KnockBack (); //Make the character unable to move while being knocked back.
		}
	}
}
