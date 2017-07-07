using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using UnityEngine.UI;

/// <summary>
/// Used for manipulating the panels next to each character showing the currently selected moves.
/// </summary>
public class SelectionPanelBahviour : MonoBehaviour
{
	private List<GameObject> panels;
	private Character owner;

	void Start () {
		panels = new List<GameObject> ();
	}

	/// <summary>
	/// Adds a clone of a list item from the move list of the move selection screen to the panel this script is attached to.
	/// </summary>
	/// <param name="original">Original.</param>
	/// <param name="button">Button.</param>
	public void AddPanelClone (GameObject original, string button)
	{
		Transform originalTransform = original.transform;
		Move originalMove = original.GetComponent<MovePanelBehaviour> ().getMove ();
		string moveName = originalMove.GetName ();
		RemovePanelWithButton (button); //Remove any panel currently using the same button.
		RemovePanelWithMove (moveName); //Remove any panel currently holding the move.
		GameObject previewPanel = Instantiate (original.gameObject, originalTransform.position, originalTransform.rotation, transform);
		MovePanelBehaviour panelBehaviour = previewPanel.GetComponent<MovePanelBehaviour> ();
		panelBehaviour.DeSelect ();
		panelBehaviour.setMove (originalMove);
		//Remove button of player1 and assign the button for player2 (even if the panel belongs to player1) because it is the furthest to the right in the panel.
		//Purely for visual effect. These list items are just select by 1 player so the furthest to the right looks better.
		panelBehaviour.ClearAssignedButton (1);
		panelBehaviour.AssignButton (button, owner.GetColor(), 2); 
		panels.Add (previewPanel);
	}

	/// <summary>
	/// Removes any panel with  move.
	/// </summary>
	/// <param name="moveName">Move name.</param>
	private void RemovePanelWithMove(string moveName)
	{
		//Cannot remove items from list while itterating.
		//Only one panel at a time can hold the same move. This fins it if it exists.
		GameObject panelWithMove = null;
		foreach(GameObject panel in panels)
		{
			MovePanelBehaviour panelBehaviour = panel.GetComponent<MovePanelBehaviour> ();
			Move panelMove = panelBehaviour.getMove ();
			string panelMoveName = panelMove.GetName ();
			if (panelMoveName.Equals (moveName))
			{
				panelWithMove = panel;
				break;
			}
		}
		if (panelWithMove != null)
		{
			panels.Remove (panelWithMove);
			Destroy (panelWithMove);
		}
	}

	/// <summary>
	/// Removes any panel with button.
	/// </summary>
	/// <param name="button">Button.</param>
	private void RemovePanelWithButton(string button)
	{
		GameObject panelWithButton = null;
		foreach(GameObject panel in panels)
		{
			//Cannot remove items from list while itterating.
			//Only one panel at a time can hold the same move. This fins it if it exists.
			//Since all copies are added with button assigned as player2 for visual purposes, we don't need check player1 button.
			Text assignedButtonText = panel.transform.Find("AssignedButton2Text").GetComponent<Text>();
			if (assignedButtonText.text.Equals (button))
			{
				panelWithButton = panel;
				break;
			}
		}
		if (panelWithButton != null)
		{
			panels.Remove (panelWithButton);
			Destroy (panelWithButton);
		}
	}

	public void SetOwner(Character character)
	{
		this.owner = character;
	}

	public Character GetOwner()
	{
		return this.owner;
	}
}
