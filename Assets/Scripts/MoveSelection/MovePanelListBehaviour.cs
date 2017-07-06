using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
﻿using System.IO;

/// <summary>
///  Class for handling the list of available moves in the MoveSelectionScene.
/// </summary>
public class MovePanelListBehaviour : MonoBehaviour 
{
	private int selectedListIndex = 0; //The index of the currently selected list item.
	private float listItemHeight; //The height of one list element.
	private int nbrOfVisiblePanels;
	private List<Move> moves;
	private MovePanelBehaviour[] listItems; //Object used for interacting with the underlying list items.
	private SelectionPanelBahviour[] selectedMovesPanels;
	private MovePlayer character1; //used for displaying the selected move on one of the visible characters.

	void Start () 
	{
		//Create a list item for each available move.
		moves = AvailableMoves.GetMoves ();
        listItems = new MovePanelBehaviour[moves.Count];
        for (int i = 0; i < moves.Count; i++)
		{
			Move move = moves [i];
			GameObject previewPanel = CreateMovePanel (move, transform);
			listItems[i] = previewPanel.GetComponent<MovePanelBehaviour> (); //Get the interaction script from the created list item.
        }

		//Calculate the number of visible panels in the scroll view.
        float viewPortHeight = transform.parent.GetComponent<RectTransform>().rect.height;
        listItemHeight = listItems[0].gameObject.GetComponent<RectTransform>().rect.height;
        nbrOfVisiblePanels = (int)Mathf.Round(viewPortHeight / listItemHeight);

		//Get a reference to the interaction script of each panel viewing the currently selected moves of each character and place them in a list.
		selectedMovesPanels = new SelectionPanelBahviour[2];
		for (int i = 0; i < selectedMovesPanels.Length; i++) {
			// (Panel(i+1) is used becuase the panels are named Panel1 and Panel2, not Panel0 and Panel1)
			selectedMovesPanels[i] = GameObject.Find ("Panel" + (i+1)).GetComponent<SelectionPanelBahviour> ();
			Character character = StaticCharacterHolder.characters [i];
			//Make sure there is actually a characters present for each selected moves panel.
			if (character == null) {
				continue;
			}
			selectedMovesPanels [i].SetOwner (character);
		}

		listItems[0].Select(); //Select the top list item.
		character1 = GameObject.Find("Character 1").GetComponent<MovePlayer>(); //Choose the character to use for animating move previews.
		PlayAnimation (); //Start previewing the animation corresponding to the first list item.
    }
    
	void Update () 
	{
        if (Input.GetKeyDown (KeyCode.UpArrow)) //Up arrow pressed
        {
            bool inTopPanels = selectedListIndex <= (nbrOfVisiblePanels / 2);
            if (selectedListIndex > 0) {
				listItems [selectedListIndex].DeSelect ();
				selectedListIndex--;
				listItems [selectedListIndex].Select ();
			}
            bool inBotPanels = selectedListIndex >= listItems.Length - (nbrOfVisiblePanels / 2) - 1;
            if (!inTopPanels && !inBotPanels)
            {
                Vector3 currentPosition = GetComponent<RectTransform>().localPosition;
                Vector3 newPosition = new Vector3(currentPosition.x, currentPosition.y - listItemHeight, currentPosition.z);
                GetComponent<RectTransform>().localPosition = newPosition;
			}
			PlayAnimation ();
        } 
		else if (Input.GetKeyDown (KeyCode.DownArrow))
        {
            bool inBotPanels = selectedListIndex >= listItems.Length - (nbrOfVisiblePanels / 2) - 1;
            
            if (selectedListIndex < listItems.Length - 1) {
				listItems [selectedListIndex].DeSelect ();
				selectedListIndex++;
				listItems [selectedListIndex].Select ();
            }
            
            bool inTopPanels = selectedListIndex <= nbrOfVisiblePanels / 2;         
            
            if (!inTopPanels && !inBotPanels)
            {
                Vector3 currentPosition = GetComponent<RectTransform>().localPosition;
                Vector3 newPosition = new Vector3(currentPosition.x, currentPosition.y + listItemHeight, currentPosition.z);
                GetComponent<RectTransform>().localPosition = newPosition;
            }
			PlayAnimation ();
		}
		else if (Input.anyKeyDown)
		{
			foreach (string button in InputSettings.allUsedButtons)
			{
				if(Input.GetKeyDown(button))
				{
					RegisterPlayerMoveToButton (button);
				}
			}
		}
	}

	private void PlayAnimation()
	{
		character1.SetAutoLoopEnabled(true);
		character1.PlayMove(moves[selectedListIndex]);
	}

	public GameObject CreateMovePanel(Move move, Transform parent){
		string previewPath = "Prefabs" + Path.DirectorySeparatorChar + "MovePanel";
		GameObject previewPanelObject = (GameObject)Resources.Load(previewPath);
		GameObject previewPanel = Instantiate (previewPanelObject, previewPanelObject.transform.position, previewPanelObject.transform.rotation, parent);
		MovePanelBehaviour panelBehaviour = previewPanel.GetComponent<MovePanelBehaviour> ();
		panelBehaviour.setMove (move);
		return previewPanel;
	}

	private void RegisterPlayerMoveToButton(string button)
	{
		Move selectedMove = listItems [selectedListIndex].getMove ();
		Character registeredCharacter = InputSettings.Register (button, selectedMove.GetName ());
		if (registeredCharacter != null)
		{
			registeredCharacter.AddMove (selectedMove);
			Color characterColor = registeredCharacter.GetColor ();
			int characterNbr = registeredCharacter.GetNbr ();
			listItems [selectedListIndex].AssignButton (button, characterColor, characterNbr);
			for (int i = 0; i < listItems.Length; i++)
			{
				if (i != selectedListIndex)
				{
					listItems [i].ClearAssignedButton (button, characterNbr);
				}
			}
			AddPanelToPlayerMoves (registeredCharacter, button);
		}
	}

	private void AddPanelToPlayerMoves(Character character, string button)
	{
		foreach (SelectionPanelBahviour selectionPanel in selectedMovesPanels)
		{
			if (selectionPanel.GetOwner().Equals(character))
			{
				GameObject original = listItems [selectedListIndex].gameObject;
				selectionPanel.AddPanelClone (original, button, character.GetColor());
			}
		}
	}
}
