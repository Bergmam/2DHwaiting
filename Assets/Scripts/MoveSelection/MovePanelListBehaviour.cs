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
	private GameObject playButton;

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

		//Hide the play button untill each character has a move assigned to each of its used buttons.
		playButton = GameObject.Find ("PlayButton");
		playButton.SetActive (false);

		listItems[0].Select(); //Select the top list item.
		character1 = GameObject.Find("Character 1").GetComponent<MovePlayer>(); //Choose the character to use for animating move previews.
		PlayAnimation (); //Start previewing the animation corresponding to the first list item.
    }
    
	void Update () 
	{
        if (Input.GetKeyDown (KeyCode.UpArrow)) //Up arrow pressed
        {
			bool movedOutOfTopPanels = selectedListIndex <= (nbrOfVisiblePanels / 2);
			MoveSelection (-1);
			bool movedIntoBotPanels = selectedListIndex >= listItems.Length - (nbrOfVisiblePanels / 2) - 1;
            if (!movedOutOfTopPanels && !movedIntoBotPanels)
            {
				ScrollList (-1);
			}
			PlayAnimation ();
        } 
		else if (Input.GetKeyDown (KeyCode.DownArrow)) //Down arrow pressed
        {
			bool movedOutOfBotPanels = selectedListIndex >= listItems.Length - (nbrOfVisiblePanels / 2) - 1;
			MoveSelection (1);
			bool moveIntoTopPanels = selectedListIndex <= nbrOfVisiblePanels / 2;
            if (!moveIntoTopPanels && !movedOutOfBotPanels)
            {
				ScrollList (1);
            }
			PlayAnimation ();
		}
		else if (Input.anyKeyDown)
		{
			//Check if any button used in the game has been pressed.
			foreach (string button in InputSettings.allUsedButtons)
			{
				if(Input.GetKeyDown(button))
				{
					RegisterPlayerMoveToButton (button);
				}
			}
		}
	}

	/// <summary>
	/// Moves the selection a number of steps up or down.
	/// </summary>
	/// <param name="steps"> Number of steps to move. Positive for down and negative for up.</param>
	private void MoveSelection(int steps)
	{
		int newIndex = selectedListIndex + steps;
		if (newIndex >= 0 && newIndex < listItems.Length)
		{
			listItems [selectedListIndex].DeSelect ();
			selectedListIndex = newIndex;
			listItems [newIndex].Select ();
		}
	}

	/// <summary>
	/// Scrolls the list a number of steps up or down by moving the content of the scroll view (which this script is place on) up or down.
	/// </summary>
	/// <param name="steps">Number of steps to scroll. Positive for up and negative for down.</param>
	private void ScrollList(int steps)
	{
		Vector3 currentPosition = GetComponent<RectTransform>().localPosition;
		float newPositionY = currentPosition.y + steps * listItemHeight;
		Vector3 newPosition = new Vector3 (currentPosition.x, newPositionY, currentPosition.z);
		GetComponent<RectTransform>().localPosition = newPosition;
	}

	/// <summary>
	/// Plays the move animation of the currently selected list item on one of the characters in the scene.
	/// </summary>
	private void PlayAnimation()
	{
		character1.SetAutoLoopEnabled(true);
		character1.PlayMove(moves[selectedListIndex]);
	}

	/// <summary>
	/// Creates a move panel.
	/// </summary>
	/// <returns>The move panel.</returns>
	/// <param name="move">Move.</param>
	/// <param name="parent">Parent.</param>
	public GameObject CreateMovePanel(Move move, Transform parent){
		string previewPath = "Prefabs" + Path.DirectorySeparatorChar + "MovePanel";
		GameObject previewPanelObject = (GameObject)Resources.Load(previewPath);
		GameObject previewPanel = Instantiate (previewPanelObject, previewPanelObject.transform.position, previewPanelObject.transform.rotation, parent);
		MovePanelBehaviour panelBehaviour = previewPanel.GetComponent<MovePanelBehaviour> ();
		panelBehaviour.setMove (move);
		return previewPanel;
	}

	/// <summary>
	/// Registers the move of the currently selected list item to a button.
	/// </summary>
	/// <param name="button">The button to assign the move to.</param>
	private void RegisterPlayerMoveToButton(string button)
	{
		Move selectedMove = listItems [selectedListIndex].getMove ();
		Character registeredCharacter = InputSettings.Register (button, selectedMove.GetName ());
		//The character returned by InputSettings.Register is the character that uses the button.
		//If the returned character is null, no character uses the button.
		if (registeredCharacter != null)
		{
			registeredCharacter.AddMove (selectedMove);
			Color characterColor = registeredCharacter.GetColor ();
			int characterNbr = registeredCharacter.GetNbr ();
			listItems [selectedListIndex].AssignButton (button, characterColor, characterNbr); //Mark the selected list item with button and player color.
			//Remove the marking from any list item of a move previously registered to the button.
			for (int i = 0; i < listItems.Length; i++)
			{
				if (i != selectedListIndex) //Do not remove the marking from the list item that was just marked.
				{
					listItems [i].ClearAssignedButton (button);
				}
			}
			AddPanelToCharacterMoves (registeredCharacter, button);
			ShowOrHidePlayButton ();
		}
	}

	/// <summary>
	/// Adds a panel to the selected moves of a character.
	/// </summary>
	/// <param name="character">Character.</param>
	/// <param name="button">Button.</param>
	private void AddPanelToCharacterMoves(Character character, string button)
	{
		//Find the panel of the character.
		foreach (SelectionPanelBahviour selectionPanel in selectedMovesPanels)
		{
			if (selectionPanel.GetOwner().Equals(character))
			{
				GameObject original = listItems [selectedListIndex].gameObject; //Copy the selected list item.
				selectionPanel.AddPanelClone (original, button); //Add copy to the selected moves panel.
				break;
			}
		}
	}

	/// <summary>
	/// Shows the or hide play button depending on wether all characters have registered all moves.
	/// </summary>
	private void ShowOrHidePlayButton()
	{
		playButton.SetActive (InputSettings.AllButtonsAssigned ());
	}
}
