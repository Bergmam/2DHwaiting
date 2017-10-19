﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputController : MonoBehaviour
{
    public float speed;
    public string horizontalAxis;
    public string verticalAxis;
	public int characterIndex;

    private float pauseTime;
    private bool pausedForTime;

	private bool paused;
	private Vector2 prePauseVelocity;
    private LayerHandler layerHandler;

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

	private JumpController jumpController;
	private MovementController movementController;

	void Start () {
		this.jumpController = gameObject.AddComponent<JumpController> ();
		this.movementController = gameObject.AddComponent<MovementController> ();
        pauseTime = 0;
		pausedForTime = false;
		// characterIndex-1 to make character 1 have index 1 etc.
        character = StaticCharacterHolder.characters[characterIndex - 1];
        animator = GameObject.Find("Character " + characterIndex).GetComponent<Animator>();
        characterMovePlayer = gameObject.GetComponent<MovePlayer> ();
        thisBody = gameObject.GetComponent<Rigidbody2D>();
        thisBody.mass = Parameters.mass;
		this.paused = false;
        layerHandler = GameObject.Find("Handler").GetComponent<LayerHandler>();
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
		if (!characterMovePlayer.CheckIsPlaying())
		{
			isPlayingMove = false;
			SetAnimatorEnabled (true);
			currentlyPlayedMove = null;

			// Only set the collider to false if we have enabled it once before
			if(activeBodypartCollider != null)
			{
				activeBodypartCollider.enabled = false;
			}
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
				string moveName = InputSettings.GetMoveName (pressedButton);
				currentlyPlayedMove = character.GetMove (moveName);

				Move move = character.GetMove (moveName);

				//Make sure the character cannot start playing another animation until this one is finished.
				isPlayingMove = true;
                layerHandler.sendToCharacterLayer(this.gameObject);
                thisBody.velocity = new Vector2(0, thisBody.velocity.y);
				SetAnimatorEnabled (false);
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
        return currentlyPlayedMove;
    }

	/// <summary>
	/// Pauses this instance, freezing all animation and disable buttons.
	/// </summary>
	public void Pause(bool stopAnimator){
		prePauseVelocity = thisBody.velocity;
		thisBody.velocity = Vector2.zero;
		this.paused = true;
		SetAnimatorEnabled (!stopAnimator);
		characterMovePlayer.Pause ();
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
		SetAnimatorEnabled (!characterMovePlayer.CheckIsPlaying ());
		characterMovePlayer.UnPause ();
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
