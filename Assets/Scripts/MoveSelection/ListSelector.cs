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

	void Awake()
	{
		this.attackList = GameObject.Find ("AttackContent").GetComponent<MovePanelListBehaviour> ();
		this.blockList = GameObject.Find ("BlockContent").GetComponent<MovePanelListBehaviour> ();
		this.attackViewport = GameObject.Find ("AttackViewport");
		this.blockViewport = GameObject.Find ("BlockViewport");
		this.blockMovesText = GameObject.Find ("BlockMovesText");
		this.attackMovesText = GameObject.Find ("AttackMovesText");
		this.moveListHeader = GameObject.Find ("MoveListHeader").GetComponent<MovePanelBehaviour> ();
	}

	void Start () {
		blockList.SetBlock (true);
		attackList.SetBlock (false);
		blockList.Init ();
		attackList.Init ();
		EnableAttackList ();
	}

	void Update () {
		bool leftPressed = Input.GetKey (KeyCode.LeftArrow) || Input.GetKey (KeyCode.A);
		bool rightPressed = Input.GetKey (KeyCode.RightArrow) || Input.GetKey (KeyCode.D);

		if (leftPressed && !rightPressed)
		{
			EnableAttackList ();
		}
		else if (rightPressed && !leftPressed)
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
