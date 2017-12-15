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
	private bool inited;

	/// <summary>
	/// Init this instance. Adds slots for the buttons of the current owner to the list of moves.
	/// This is init and not start because the owner is not set form the start.
	/// </summary>
	void Init ()
	{
		//TODO: In case we ever set owner twice, we would have to delete all children of the panel.
		this.inited = true;
		panels = new List<GameObject> ();
		List<string> character1Buttons = InputSettings.GetCharacterButtons (owner.GetNbr ());
		foreach (string characterButton in character1Buttons)
		{
			string previewPath = "Prefabs" + Path.DirectorySeparatorChar + "MovePanel";
			GameObject previewPanelObject = (GameObject)Resources.Load (previewPath);
			GameObject previewPanel = Instantiate (previewPanelObject, previewPanelObject.transform.position, previewPanelObject.transform.rotation, transform);
			if (owner.GetNbr () == 1)
			{
				foreach (Transform child in previewPanel.transform)
				{
					//Flip all text fields.
					//The transform of player 1 is flipped. So if all children are not flipped as well, the text is reversed.
					child.GetComponent<RectTransform> ().localScale = new Vector3 (-1, 1, 1);
				}
			}
			MovePanelBehaviour panelBehaviour = previewPanel.GetComponent<MovePanelBehaviour> ();
			if (owner != null)
			{
				panelBehaviour.AssignButton (characterButton, owner.GetColor (), 2);
			}
		}
	}

	/// <summary>
	/// Adds the move to the list of selected moves in the slot with the correct button.
	/// </summary>
	/// <param name="button">Button.</param>
	/// <param name="move">Move.</param>
	public void AddMove(string button, Move move)
	{
		foreach (Transform child in transform)
		{
			MovePanelBehaviour panelBehaviour = child.GetComponent<MovePanelBehaviour> ();

			//Panel has to be a move panel.
			if (panelBehaviour == null) {
				continue;
			}
			if (panelBehaviour.GetAssignedButton () == null) {
				continue;
			}

			//Add move to correct panel and remove it from any other panel.
			if (panelBehaviour.GetAssignedButton ().Equals (button)) {
				RemovePanelWithMove (move.GetName ()); //Remove the move from any panel currently holding the move.
				panelBehaviour.setMove (move);
			}
		}
	}

	/// <summary>
	/// Clears move from any panel that has a specific move.
	/// </summary>
	/// <param name="moveName">Move name.</param>
	public void RemovePanelWithMove(string moveName)
	{
		//Cannot remove items from list while itterating.
		//Only one panel at a time can hold the same move. This fins it if it exists.
		Transform panelWithMove = null;
		foreach(Transform panel in transform)
		{
			MovePanelBehaviour panelBehaviour = panel.GetComponent<MovePanelBehaviour> ();
			Move panelMove = panelBehaviour.getMove ();
			if (panelMove == null) {
				continue; // Skip panel if it does not have a move.
			}
			string panelMoveName = panelMove.GetName ();
			if (panelMoveName.Equals (moveName))
			{
				panelWithMove = panel;
				break; //Quit loop when the correct panel is found.
			}
		}
		if (panelWithMove != null)
		{
			//Clear the panel.
			panelWithMove.GetComponent<MovePanelBehaviour>().setMove(null);
			panelWithMove.GetComponent<MovePanelBehaviour> ().RemoveSpeedText ();
			panelWithMove.GetComponent<MovePanelBehaviour> ().RemoveStrengthText ();
			panelWithMove.GetComponent<MovePanelBehaviour> ().RemoveNameText ();
		}
	}

	public void SetOwner(Character character)
	{
		this.owner = character;
		//The first time owner is set, add that owners' buttons to the slots of this list.
		if (!inited && character != null && character.GetColor () != null)
		{
			this.Init ();
		}
	}

	public Character GetOwner()
	{
		return this.owner;
	}
}
