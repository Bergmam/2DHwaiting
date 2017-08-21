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
	private bool knockedBack;
	private float previousPushedVelocity = 0; //Used to compare when the character has been knocked back and is approaching velocity 0 again.
	private bool paused;
	private Vector2 prePauseVelocity;

	// isPlayingMove exists in addition to the MovePlayer.CheckIsPlaying() method to avoid concurrency issues.
	bool isPlayingMove = false;
	string pressedButton = "";
    string activeBodypartName;
    Collider2D activeBodypartCollider;
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

		// If previous animation is finished, reset isPlayingMove and enable the animator.
		if (!characterMovePlayer.CheckIsPlaying())
		{
			isPlayingMove = false;
			animator.enabled = true;
			currentlyPlayedMove = null;

			// Only set the collider to false if we have enabled it once before
			if(activeBodypartCollider != null)
			{
				activeBodypartCollider.enabled = false;
			}
		}

		if (!isPlayingMove)
		{
			if (Input.anyKeyDown)
			{
				foreach (string button in InputSettings.allUsedButtons)
				{
					if (Input.GetKeyDown (button))
					{
						pressedButton = button;
					}
				}
				if (InputSettings.HasButton (characterIndex, pressedButton))
				{
					string moveName = InputSettings.GetMoveName (pressedButton);
					currentlyPlayedMove = character.GetMove (moveName);

					Move move = character.GetMove (moveName);

					//Make sure the character cannot start playing another animation until this one is finished.
					isPlayingMove = true;
					thisBody.velocity = Vector2.zero;
					animator.enabled = false;
					// Sets MovePlayer.isPlaying before calling MovePlayer.PlayMove() to avoid concurrency issues.
					characterMovePlayer.SetIsPlaying ();
					characterMovePlayer.PlayMove (currentlyPlayedMove);
					// Get the name of the move assigned to do damage.
					activeBodypartName = currentlyPlayedMove.GetActiveBodypart();
					if (currentlyPlayedMove.IsBlockMove ()) {
						activeBodypartName = activeBodypartName.Replace (" ", "") + "Shield";
					}
					//Enable the collider of the active bodypart or shield.
					Transform activeBodypart = UnityUtils.RecursiveFind (transform, activeBodypartName);
					activeBodypartCollider = activeBodypart.GetComponent<Collider2D> ();
					activeBodypartCollider.enabled = true;
				}
			}
		}

		// Get information about the next position of the Character
        float horizontal = Input.GetAxisRaw(horizontalAxis);
		//Check if player has been knocked back and is now standing still again.
		//Without previousPushedVelocity, the velocity is zero in the first frame and no knockback is applied.
		if (Mathf.Abs (thisBody.velocity.x) < 0.1 && knockedBack && previousPushedVelocity > Mathf.Abs (thisBody.velocity.x)) {
			//Remove any colision that was detected before the character was knocked back.
			this.collisionLeft = false;
			this.collisionRight = false;
			//Reset for the first comparison next time KnockBack is called.
			knockedBack = false;
			previousPushedVelocity = 0;
		}
		if (horizontal < 0 && !collisionLeft && !knockedBack && !isPlayingMove)
        {
            thisBody.velocity = new Vector3(-10, thisBody.velocity.y);
			collisionRight = false; //If moving left, there is no longer a collision to the right.
        }
		else if (horizontal > 0 && !collisionRight && !knockedBack && !isPlayingMove)
        {
			thisBody.velocity = new Vector3(10, thisBody.velocity.y);
			collisionLeft = false; //If moving right, there is no longer a collision to the left
        }
		else if (horizontal == 0 && !knockedBack && !isPlayingMove)
        {
            thisBody.velocity = new Vector3(0, thisBody.velocity.y); //Without the knockedBack bool, this stops the character.
        }
        pressedButton = "";

		// Check isPlayingMove again since it can be set to true in the if-block above.
		if (!isPlayingMove)
		{
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

	//Check previous velocity after update has run.
	void LateUpdate()
	{
		if (knockedBack)
		{
			previousPushedVelocity = Mathf.Abs (thisBody.velocity.x);
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
		prePauseVelocity = thisBody.velocity;
		thisBody.velocity = Vector2.zero;
		this.paused = true;
		this.animator.enabled = false;
		characterMovePlayer.Pause ();
	}

	/// <summary>
	/// Unpauses this instance, enabling buttons and resuming the animation that is currently playing.
	/// </summary>
	public void UnPause()
	{
		thisBody.velocity = prePauseVelocity;
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

	public void KnockBack()
	{
		this.knockedBack = true;
	}
}
