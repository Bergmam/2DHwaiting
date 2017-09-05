using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterCollisionDetector : MonoBehaviour
{
	public int characterIndex;
	private Character character;
	private GameState gameState;
	private Rigidbody2D rigidBody;
	private InputController inputController;

    AudioSource audioCenter;
    AudioClip punch;

    void Start ()
	{
		this.inputController = GetComponent<InputController> ();
		this.character = StaticCharacterHolder.characters[characterIndex - 1];
		this.rigidBody = GetComponent<Rigidbody2D> ();
		this.gameState = GameObject.Find ("Handler").GetComponent<GameState> ();

        audioCenter = GameObject.Find("AudioCenter").GetComponent<AudioSource>();
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
		Move move = otherInputController.GetCurretlyPlayedMove ();
		if (otherCharacter == null || move == null || otherCharacter.Equals (this.character))
		{
			return; //Make sure other character object has all necessary info.
		}
        audioCenter.Play();

        this.character.ApplyMoveTo (move); //Apply damage in model.
		gameState.UpdateCharacterHealth (this.character); //Update health bars and check winner.

		//Apply knockback.
		Vector3 thisPosition = transform.position;
		Vector3 otherPosition = otherRootTransform.position;
		if (otherPosition.x < thisPosition.x)
		{
			this.rigidBody.AddForce (Vector2.right * 50f);
			this.inputController.KnockBack (); //Make the character unable to move while being knocked back.
		}
		else
		{
			this.rigidBody.AddForce (Vector2.left * 50f);
			this.inputController.KnockBack (); //Make the character unable to move while being knocked back.
		}

		otherCollider.enabled = false;
	}
		
	//Detect collisions with other non-trigger colliders.
	void OnCollisionEnter2D(Collision2D collision)
	{
		Collider2D otherCollider = collision.collider; //The collider the object this script is attached to collides with.
		GameObject otherCharacterObject = otherCollider.gameObject;
		InputController otherInputController = otherCharacterObject.GetComponent<InputController> ();
		if (otherInputController == null)
		{
			return; //If colliding object's root does not have an InputController, it is not a character.
		}
		//Tell this inputcontroller something is blocking the way in the right direction.
		if (otherCollider.transform.position.x < transform.position.x)
		{
			inputController.CollisionLeft ();
		}
		else
		{
			inputController.CollisionRight ();
		}
	}

    void OnCollisionExit2D(Collision2D collision)
    {
        Collider2D otherCollider = collision.collider; //The collider the object this script is attached to collides with.
        GameObject otherCharacterObject = otherCollider.gameObject;
        InputController otherInputController = otherCharacterObject.GetComponent<InputController>();
        if (otherInputController == null)
        {
            return; //If colliding object's root does not have an InputController, it is not a character.
        }
        //Tell this inputcontroller something is blocking the way in the right direction.
        if (otherCollider.transform.position.x < transform.position.x)
        {
            inputController.CollisionExitLeft();
        }
        else
        {
            inputController.CollisionExitRight();
        }
    }
}
