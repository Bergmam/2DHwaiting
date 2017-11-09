using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Takes input from the player and delegates it to the corresponding controller. Delegates input to FightMoveController, MovementController and JumpController.
/// </summary>
public class InputController : MonoBehaviour
{
	// Unity specific strings related to movement keys in our case.
    public string horizontalAxis;
    public string verticalAxis;

	public int characterIndex;
	private Character character;

    private float pauseTime;
	private bool pausedForTime;
	private bool paused;

    // Booleans for coordinating transitions between animations
    private bool isRunning = false;
    private bool isCrouching = false;
    private bool isJumping = false;
    private bool collisionDown = true;

	// isPlayingMove exists in addition to the MovePlayer.CheckIsPlaying() method to avoid concurrency issues.
	bool isPlayingMove = false;
	Animator animator;

	private JumpController jumpController;
	private MovementController movementController;
	private FightMoveController fightMoveController;

	void Start () {
		// characterIndex-1 to make character 1 have index 1 etc.
		this.character = StaticCharacterHolder.characters[characterIndex - 1];
		this.jumpController = gameObject.AddComponent<JumpController> ();
		this.movementController = gameObject.AddComponent<MovementController> ();
		this.fightMoveController = gameObject.AddComponent<FightMoveController> ();
		this.fightMoveController.SetCharacter (this.character);
		this.pauseTime = 0;
		this.pausedForTime = false;
		this.paused = false;
		this.animator = GetComponent<Animator>();
	}

    void Update() {

		// Reduce pause time or unpause if pause time has run out.
        if (pauseTime > 0)
        {
            pauseTime -= Time.deltaTime;
        }

        if (pauseTime <= 0 && pausedForTime)
        {
            UnPause();
        }

		if (paused) {
			return; //End method if game is paused.
		}
		// If previous move finished, reset isPlayingMove and enable the animator.
		if (!fightMoveController.IsDoingMove ())
		{
			isPlayingMove = false;
			SetAnimatorEnabled (true);
		}

		//Check fight move input.
		if (Input.anyKeyDown)
		{
			string pressedButton = "";
			foreach (string button in InputSettings.allUsedButtons)
			{
				if (Input.GetKeyDown (button))
				{
					pressedButton = button;
				}
			}
			if (InputSettings.HasButton (characterIndex, pressedButton) && !isPlayingMove && !isCrouching)
			{
				isPlayingMove = true;
				SetAnimatorEnabled (false);
				string moveName = InputSettings.GetMoveName (pressedButton);
				fightMoveController.DoMove (moveName);
			}
		}

		// Get information about the next position of the Character
        float horizontal = Input.GetAxisRaw(horizontalAxis);
        float vertical = Input.GetAxisRaw(verticalAxis);
        
		//isPlaying is whether a fight move is being played. If it is, don't move the character.
		if (!isPlayingMove)
		{
			// Move sideways
			if (horizontal < 0 && !this.isCrouching)
	        {
				this.movementController.MoveLeft ();
	        }
			else if (horizontal > 0 && !this.isCrouching)
	        {
				this.movementController.MoveRight ();
	        }
			else if (horizontal == 0)
	        {
				this.movementController.Stop ();
	        }
            print(this.isCrouching);
            if (vertical > 0)
            {
                SetAnimatorBool("Crouching", false);
                SetAnimatorBool("Jumping", true);
                this.collisionDown = false;
                jumpController.Jump();
            }
            else if (Mathf.Abs(horizontal) > 0 && this.collisionDown && !this.isCrouching)
            {
                SetAnimatorBool("Running", true);
                this.isRunning = true;
            }
            else if (vertical < 0 && !this.isJumping)
            {
                SetAnimatorBool("Crouching", true);
                this.isCrouching = true;
            }

            if (vertical == 0 && collisionDown)
            {
                SetAnimatorBool("Jumping", false);
                SetAnimatorBool("Crouching", false);
                this.isCrouching = false;
            }

            if (Mathf.Abs(horizontal) == 0)
            {
                SetAnimatorBool("Running", false);
            }
		}
    }

    public Character GetCharacter()
    {
        return character;
    }

    public Move GetCurretlyPlayedMove()
    {
		return this.fightMoveController.GetCurretlyPlayedMove ();
    }

	/// <summary>
	/// Pauses this instance, freezing all animation and disable buttons.
	/// </summary>
	public void Pause(bool stopAnimator){
		this.movementController.Pause ();
		this.paused = true;
		SetAnimatorEnabled (!stopAnimator);
		fightMoveController.Pause ();
	}

    /// <summary>
    /// Calls the pause method of this class for a set amount of time
    /// </summary>
    public void PauseSeconds(float ms)
    {
        pausedForTime = true;
        Pause(false);
        pauseTime = ms;
    }
    

	/// <summary>
	/// Unpauses this instance, enabling buttons and resuming the animation that is currently playing.
	/// </summary>
	public void UnPause()
	{
		pausedForTime = false;
		animator.SetBool("Stunned", false);
		this.movementController.UnPause ();
		this.paused = false;
		SetAnimatorEnabled (!fightMoveController.IsDoingMove ());
		fightMoveController.UnPause ();
	}

	public void CollisionLeft()
	{
		this.movementController.CollisionLeft ();
	}

	public void CollisionRight()
	{
		this.movementController.CollisionRight ();
	}

    public void CollisionExitLeft()
    {
		this.movementController.CollisionExitLeft ();
    }

    public void CollisionExitRight()
    {
		this.movementController.CollisionExitRight ();
    }

    public void CollisionDown()
    {
        this.collisionDown = true;
    }

    public void KnockBack()
	{
		this.movementController.KnockBack ();
	}

	private void SetAnimatorEnabled(bool enabled)
	{
		if (this.animator != null) {
			this.animator.enabled = enabled;
		}
	}

	private void SetAnimatorBool(string boolName, bool enabled)
	{
		if (this.animator != null) {
			this.animator.SetBool (boolName, enabled);
		}
	}
}
