using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Class to handle trigger events when one character uses a move on another character
/// </summary>
public class BodyPartTriggerHandler : MonoBehaviour {

    Character character;
    InputController inputController;
    

    void Start ()
    {
        inputController = transform.root.GetComponent<InputController>();
        character = inputController.GetCharacter();	
	}

    /// <summary>
    /// When a Body Part collides with a Character Boxcollider, checks which move was played and 
    /// does damage to the other character accordingly.
    /// </summary>
    /// <param name="other">The 2D Collider which this object has been triggered by.</param>
    void OnTriggerEnter2D(Collider2D other)
    {
        GameObject otherGameObject = other.gameObject;
        Move move = inputController.GetCurretlyPlayedMove();
        
        // If the collided object is not part of the same character
        if (otherGameObject.transform.root.name != transform.root.name &&  move != null)
        {
            print("other: " + otherGameObject.name);
            print("me: " + transform.name);
            print("self: " + transform.root.name);
            InputController otherInputController = otherGameObject.GetComponent<InputController>();
            Character otherCharacter = otherInputController.GetCharacter();
            
            // Do damage to the other character
            otherCharacter.ApplyMoveTo(move);

            // TODO: Remove once healthbars are implemented
            print("Move '" + inputController.GetCurretlyPlayedMove().GetName() + "' applied to character " + otherCharacter.GetNbr());
            print("Character " + otherCharacter.GetNbr() + " now has " + otherCharacter.GetHealth() + " health");

            Vector3 thisPosition = transform.root.transform.position;
            Vector3 otherPosition = otherGameObject.transform.position;
            Rigidbody2D otherBody = otherGameObject.GetComponent<Rigidbody2D>();
            
            if (otherPosition.x > thisPosition.x)
            {
                otherBody.AddForce(Vector2.right * 250);
            }
            else
            {
                otherBody.AddForce(Vector2.left * 250);
            }
        }
    }
}