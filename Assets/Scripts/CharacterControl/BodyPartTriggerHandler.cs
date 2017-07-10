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
        // If the collided object is not part of the same character
        if (otherGameObject.name != transform.root.name)
        {
            InputController otherInputController = otherGameObject.GetComponent<InputController>();
            Character otherCharacter = otherInputController.GetCharacter();

            // Get the move currently playing from the inputController.
            Move move = inputController.GetCurretlyPlayedMove();
            // Do damage to the other character
            otherCharacter.ApplyMoveTo(move);

            // TODO: Remove once healthbars are implemented
            print("Move '" + inputController.GetCurretlyPlayedMove().GetName() + "' applied to character " + otherCharacter.GetNbr());
            print("Character " + otherCharacter.GetNbr() + " now has " + otherCharacter.GetHealth() + " health");
        }
    }
}