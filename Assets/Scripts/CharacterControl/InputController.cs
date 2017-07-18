using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputController : MonoBehaviour
{
    public float speed;
    public string horizontalAxis;
	public int characterIndex;
	private bool collisionLeft;
	private bool collisionRight;

	private bool paused;

	// isPlayingMove exists in addition to the MovePlayer.CheckIsPlaying() method to avoid concurrency issues.
	bool isPlayingMove = false;
	string pressedButton = "";
    string damageDealerName;
    Collider2D damageDealerCollider;
    Character character;
    Move currentlyPlayedMove;

	Animator animator;
	MovePlayer characterMovePlayer;
    Rigidbody2D thisBody;

	// stupidCounter exists to make sure that the transition between unity animations and a MovePlayer animation does not happen too fast.
	// TODO: Remove the counter
	int stupidCounter = 0;

	void Start () {
        // characterIndex-1 to make character 1 have index 1 etc.
        character = StaticCharacterHolder.characters[characterIndex - 1];
        animator = GameObject.Find ("Character " + characterIndex +"/Torso").GetComponent<Animator> ();
		characterMovePlayer = gameObject.GetComponent<MovePlayer> ();
        thisBody = gameObject.GetComponent<Rigidbody2D>();
		this.paused = false;
	}

    void Update() {
		if (paused) {
			return;
		}
		// Get information about the next position of the Character
        float horizontal = Input.GetAxisRaw(horizontalAxis);
		if (horizontal < 0 && !collisionLeft)
        {
            thisBody.velocity = new Vector3(-10, thisBody.velocity.y);
			collisionRight = false; //If moving left, there is no longer a collision to the right.
        }
		else if (horizontal > 0 && !collisionRight)
        {
			thisBody.velocity = new Vector3(10, thisBody.velocity.y);
			collisionLeft = false; //If moving right, there is no longer a collision to the left.
        }
        else if (horizontal == 0)
        {
            thisBody.velocity = new Vector3(0, thisBody.velocity.y);
        }
        pressedButton = "";

        // If previous animation is finished, reset isPlayingMove and enable the animator.
        if (!characterMovePlayer.CheckIsPlaying())
        {
            isPlayingMove = false;
            animator.enabled = true;
            currentlyPlayedMove = null;

            // Only set the collider to false if we have enabled it once before
            if(damageDealerCollider != null)
            {
                damageDealerCollider.enabled = false;
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
						print ("StaticCharacterHolder.characters.Count = " + StaticCharacterHolder.characters.Count);
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
                    damageDealerCollider = damageDealer.GetComponent<Collider2D>();
                    // Enables the Collider component of the 
                    damageDealerCollider.enabled = true;
				}
			}
		}

		// Check isPlayingMove again since it can be set to true in the if-block above.
		if (!isPlayingMove){
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

	/// <summary>
	/// Pauses this instance, freezing all animation and disable buttons.
	/// </summary>
	public void Pause(){
		this.paused = true;
		this.animator.enabled = false;
		characterMovePlayer.Pause ();
	}

	/// <summary>
	/// Unpauses this instance, enabling buttons and resuming the animation that is currently playing.
	/// </summary>
	public void UnPause(){
		this.paused = false;
		this.animator.enabled = !characterMovePlayer.CheckIsPlaying ();
		characterMovePlayer.UnPause ();
	}

	public void CollisionLeft()
	{
		this.collisionLeft = true;
	}

	public void CollisionRight()
	{
		this.collisionRight = true;
	}
}
