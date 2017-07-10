using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputController : MonoBehaviour {


    
    public float speed;
    public string horizontalAxis;
	public int MaxHorizontalDiff;
	public int characterIndex;

	// isPlayingMove exists in addition to the MovePlayer.CheckIsPlaying() method to avoid concurrency issues.
	bool isPlayingMove = false;
	string pressedButton = "";
    string damageDealerName;
    BoxCollider2D damageDealerBox;
    Character character;
    Move currentlyPlayedMove;

	Animator animator;
	MovePlayer characterMovePlayer;

	// stupidCounter exists to make sure that the transition between unity animations and a MovePlayer animation does not happen too fast.
	// TODO: Remove the counter
	int stupidCounter = 0;

	void Start () {
		animator = GameObject.Find ("Character " + characterIndex +"/Torso").GetComponent<Animator> ();
		characterMovePlayer = gameObject.GetComponent<MovePlayer> ();
		// characterIndex-1 to make character 1 have index 1 etc.	
		character = StaticCharacterHolder.characters [characterIndex-1];
	}

    void Update() {
		// Get information about the next position of the Character
        float horizontal = Input.GetAxis(horizontalAxis);
        Vector3 newPosition = new Vector3(transform.position.x + speed * horizontal, transform.position.y, transform.position.z);
        pressedButton = "";

        // If previous animation is finished, reset isPlayingMove and enable the animator.
        if (!characterMovePlayer.CheckIsPlaying())
        {
            isPlayingMove = false;
            animator.enabled = true;
            currentlyPlayedMove = null;

            // Only set the boxcollider to false if we have enabled it once before
            if(damageDealerBox != null)
            {
                damageDealerBox.enabled = false;
            }
        }

		if (!isPlayingMove) {
			if (Input.anyKeyDown) {
				foreach (string button in InputSettings.allUsedButtons) {
					if (Input.GetKeyDown (button)) {
						pressedButton = button;
					}
				}
				if (InputSettings.HasButton (characterIndex, pressedButton)) {
					string moveName = InputSettings.GetMoveName (pressedButton);
					currentlyPlayedMove = character.GetMove (moveName);

					Move move = character.GetMove (moveName);
					// ### TEST ### TODO: Remove below
					if (currentlyPlayedMove == null) {
						print ("Move " + moveName + " is null! Character index is " + characterIndex + ", characterNbr = " + character.GetNbr ());
						foreach (Character ch in StaticCharacterHolder.characters) {
							foreach (string b in InputSettings.allUsedButtons) {
								if (ch.GetMove (InputSettings.GetMoveName (b)) == null) {
									print ("character has move " + null + " at button " + b);
								}else{
									print ("character has move " + InputSettings.GetMoveName (b) + " at button " + b);
								}
							}
						}
					}

					//Make sure the character cannot start playing another animation until this one is finished.
					isPlayingMove = true;
					animator.enabled = false;
					// Sets MovePlayer.isPlaying before calling MovePlayer.PlayMove() to avoid concurrency issues.
					characterMovePlayer.SetIsPlaying ();
					characterMovePlayer.PlayMove (currentlyPlayedMove);
                    // Get the name of the move assigned to do damage.
                    damageDealerName = currentlyPlayedMove.GetDamageDealer();
                    Transform damageDealer = UnityUtils.RecursiveFind(transform,damageDealerName);
                    damageDealerBox = damageDealer.GetComponent<BoxCollider2D>();
                    // Enables the Collider component of the 
                    damageDealerBox.enabled = true;
				}
			}
		}

		// Check isPlayingMove again since it can be set to true in the if-block above.
		if (!isPlayingMove){
			// Make sure the character is inside the specified area.
			if(Mathf.Abs(newPosition.x) <= MaxHorizontalDiff)
			{
				transform.position = newPosition;
			}
			if (Mathf.Abs(horizontal) > 0)
			{
				animator.SetBool("Running", true);
				stupidCounter = 1;
			}
			else if (stupidCounter == 0)
			{
				animator.SetBool("Running", false);
			}
			stupidCounter--;
        }
    }

    public Character GetCharacter()
    {
        return character;
    }

    public Move GetCurretlyPlayedMove()
    {
        return currentlyPlayedMove;
    }
}
