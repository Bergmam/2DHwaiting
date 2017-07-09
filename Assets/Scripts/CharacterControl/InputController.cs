using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputController : MonoBehaviour {

    private bool isPlayingMove = false;
	private string pressedButton = "";
    public float speed;
    public string horizontalAxis;
	Animator animator;
	MovePlayer characterMovePlayer;

	public int characterIndex;
	private Character character;

	int stupidCounter = 0;

	// Use this for initialization
	void Start () {
		animator = GameObject.Find ("Character " + characterIndex +"/Torso").GetComponent<Animator> ();
		characterMovePlayer = gameObject.GetComponent<MovePlayer> ();
		// -1 to make character 1 have index 1 etc.	
		character = StaticCharacterHolder.characters [characterIndex-1];
	}

    // Update is called once per frame
    void Update() {
        float horizontal = Input.GetAxis(horizontalAxis);
        Vector3 newPosition = new Vector3(transform.position.x + speed * horizontal, transform.position.y, transform.position.z);
        transform.position = newPosition;
        pressedButton = "";

        //Check if we are finished with previous animation
        if (!characterMovePlayer.CheckIsPlaying())
        {
            isPlayingMove = false;
            animator.enabled = true;
        }

        if (!isPlayingMove)
        {
            if (Input.anyKeyDown)
            {
                foreach (string button in InputSettings.allUsedButtons)
                {
                    if (Input.GetKeyDown(button))
                    {
                        print("Input.GetKeyDown(button): " + Input.GetKeyDown(button) + ", Button: " + button);
                        pressedButton = button;
                    }
                }
                if (InputSettings.HasButton(characterIndex, pressedButton))
                {
                    string moveName = InputSettings.GetMoveName(pressedButton);
                    Move move = character.GetMove(moveName);
                    //Make sure the character cannot start playing another animation until this one is finished.
                    isPlayingMove = true;
                    animator.enabled = false;
                    characterMovePlayer.SetIsPlaying();
                    characterMovePlayer.PlayMove(move);
                }
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
}
