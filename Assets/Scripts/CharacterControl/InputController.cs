using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputController : MonoBehaviour
{
    public float speed;
    public string horizontalAxis;
    public string verticalAxis;
	public int characterIndex;
	private Character character;

    private float pauseTime;
	private bool pausedForTime;
	private bool paused;
	private Vector2 prePauseVelocity;

	// isPlayingMove exists in addition to the MovePlayer.CheckIsPlaying() method to avoid concurrency issues.
	bool isPlayingMove = false;
	string pressedButton = "";

	Animator animator;
    Rigidbody2D thisBody;

	private JumpController jumpController;
	private MovementController movementController;
	private FightMoveController fightMoveController;

	void Start () {
		// characterIndex-1 to make character 1 have index 1 etc.
		this.character = StaticCharacterHolder.characters[characterIndex - 1];
		this.jumpController = gameObject.AddComponent<JumpController> ();
		this.movementController = gameObject.AddComponent<MovementController> ();
		this.fightMoveController = gameObject.AddComponent<FightMoveController> ();
		fightMoveController.SetCharacter (this.character);
        pauseTime = 0;
		pausedForTime = false;
        animator = GameObject.Find("Character " + characterIndex).GetComponent<Animator>();
        thisBody = gameObject.GetComponent<Rigidbody2D>();
        thisBody.mass = Parameters.mass;
		this.paused = false;
	}

    void Update() {

        if (pauseTime > 0)
        {
            pauseTime -= Time.deltaTime;
        }

        if (pauseTime <= 0 && pausedForTime)
        {
            UnPause();
        }

		if (paused) {
			return;
		}

		// If previous animation is finished, reset isPlayingMove and enable the animator.
		if (!fightMoveController.IsDoingMove ())
		{
			isPlayingMove = false;
			SetAnimatorEnabled (true);
		}


		if (Input.anyKeyDown)
		{
			foreach (string button in InputSettings.allUsedButtons)
			{
				if (Input.GetKeyDown (button))
				{
					pressedButton = button;
				}
			}
			if (InputSettings.HasButton (characterIndex, pressedButton) && !isPlayingMove)
			{
				isPlayingMove = true;
				SetAnimatorEnabled (false);
				string moveName = InputSettings.GetMoveName (pressedButton);
				fightMoveController.DoMove (moveName);
			}
		}
		pressedButton = "";

		// Get information about the next position of the Character
        float horizontal = Input.GetAxisRaw(horizontalAxis);
        float vertical = Input.GetAxisRaw(verticalAxis);
        
		if (horizontal < 0 && !isPlayingMove)
        {
			this.movementController.MoveLeft ();
        }
		else if (horizontal > 0 && !isPlayingMove)
        {
			this.movementController.MoveRight ();
        }
		else if (horizontal == 0 && !isPlayingMove)
        {
			this.movementController.Stop ();
        }

        if (vertical > 0)
        {
			jumpController.Jump ();
        }

        // Check isPlayingMove again since it can be set to true in the if-block above.
        if (!isPlayingMove)
		{
			if (Mathf.Abs(horizontal) > 0)
			{
				SetAnimatorBool ("Running", true);
			}
			else
			{
				SetAnimatorBool ("Running", false);
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
		prePauseVelocity = thisBody.velocity;
		thisBody.velocity = Vector2.zero;
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
		thisBody.velocity = prePauseVelocity;
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
