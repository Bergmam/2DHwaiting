using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ListSelector : MonoBehaviour {

	private MovePanelListBehaviour attackList;
	private MovePanelListBehaviour blockList;
	GameObject attackViewport;
	GameObject blockViewport;
	GameObject blockMovesText;
	GameObject attackMovesText;
	MovePanelBehaviour moveListHeader;
	private GameObject deleteMovePrompt;

	void Awake()
	{
		this.attackList = GameObject.Find ("AttackContent").GetComponent<MovePanelListBehaviour> ();
		this.blockList = GameObject.Find ("BlockContent").GetComponent<MovePanelListBehaviour> ();
		this.attackViewport = GameObject.Find ("AttackViewport");
		this.blockViewport = GameObject.Find ("BlockViewport");
		this.blockMovesText = GameObject.Find ("BlockMovesText");
		this.attackMovesText = GameObject.Find ("AttackMovesText");
		this.moveListHeader = GameObject.Find ("MoveListHeader").GetComponent<MovePanelBehaviour> ();
		this.deleteMovePrompt = GameObject.Find ("DeleteMovePrompt");
	}

	void Start () {
		blockList.SetBlock (true);
		attackList.SetBlock (false);
		blockList.Init ();
		attackList.Init ();
		EnableAttackList ();
	}

	void Update () {

		bool horizontal1Right = Input.GetAxisRaw("Horizontal") > 0;
		bool horizontal1Left = Input.GetAxisRaw("Horizontal") < 0;
		bool horizontal2Right = Input.GetAxisRaw("Horizontal2") > 0;
		bool horizontal2Left = Input.GetAxisRaw("Horizontal2") < 0;

		bool leftPressed = (horizontal1Left || horizontal2Left);
		bool rightPressed = (horizontal1Right || horizontal2Right);

		if (leftPressed && !rightPressed && !deleteMovePrompt.activeSelf)
		{
			EnableAttackList ();
		}
		else if (rightPressed && !leftPressed && !deleteMovePrompt.activeSelf)
		{
			EnableBlockList ();
		}
	}

	private void EnableAttackList(){
		attackList.SetSelected (true);
		blockList.SetSelected (false);
		blockViewport.SetActive (false);
		attackViewport.SetActive (true);
		attackMovesText.SetActive (false);
		blockMovesText.SetActive (true);
		moveListHeader.SetNameText ("Attack move name");
		moveListHeader.SetSpeedText ("Spd");
		moveListHeader.SetStrengthText ("Str");
	}

	private void EnableBlockList(){
		attackList.SetSelected (false);
		blockList.SetSelected (true);
		attackViewport.SetActive (false);
		blockViewport.SetActive (true);
		attackMovesText.SetActive (true);
		blockMovesText.SetActive (false);
		moveListHeader.SetNameText ("Block move name");
		moveListHeader.SetSpeedText ("Blk");
		moveListHeader.SetStrengthText ("Cvr");
	}

	public void ClearButton(string button)
	{
		attackList.ClearButton (button);
		blockList.ClearButton (button);
	}

	/// <summary>
	/// Deletes the currently selected move from the list of moves
	///
	public void DeleteMove()
	{
		if (attackList.IsSelected ())
		{
			attackList.DeleteMove ();
		}
		else if (blockList.IsSelected ())
		{
			blockList.DeleteMove ();
		}
	}

	public void CancelDeleteMove() {
		attackList.CancelDeleteMove ();
		blockList.CancelDeleteMove ();
	}
}
