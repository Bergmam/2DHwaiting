using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ListSelector : MonoBehaviour {

	private MovePanelListBehaviour attackList;
	private MovePanelListBehaviour blockList;
	GameObject attackViewport;
	GameObject blockViewport;

	void Awake()
	{
		this.attackList = GameObject.Find ("AttackContent").GetComponent<MovePanelListBehaviour> ();
		this.blockList = GameObject.Find ("BlockContent").GetComponent<MovePanelListBehaviour> ();
		this.attackViewport = GameObject.Find ("AttackViewport");
		this.blockViewport = GameObject.Find ("BlockViewport");
	}

	void Start () {
		attackList.SetSelected (true);
		blockList.SetSelected (false);
		attackList.SetBlock (false);
		blockList.SetBlock (true);
		blockList.Init ();
		attackList.Init ();
		blockViewport.SetActive (false);
		attackViewport.SetActive (true);
	}

	void Update () {
		bool leftPressed = Input.GetKey (KeyCode.LeftArrow) || Input.GetKey (KeyCode.A);
		bool rightPressed = Input.GetKey (KeyCode.RightArrow) || Input.GetKey (KeyCode.D);

		if (leftPressed && !rightPressed)
		{
			attackList.SetSelected (true);
			blockList.SetSelected (false);
			blockViewport.SetActive (false);
			attackViewport.SetActive (true);
		}
		else if (rightPressed && !leftPressed)
		{
			attackList.SetSelected (false);
			blockList.SetSelected (true);
			attackViewport.SetActive (false);
			blockViewport.SetActive (true);
		}
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
		{;
			attackList.DeleteMove ();
		}
		else if (blockList.IsSelected ())
		{
			blockList.DeleteMove ();
		}
	}
}
