using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
﻿using System.IO;

/// <summary>
///  Class for handling the grid in the MoveSelection screen
/// </summary>
public class MovePanelListBehaviour : MonoBehaviour 
{
	private int currentY = 0;
	private float panelHeight;
	private int nbrOfVisiblePanels;
	private MovePanelBehaviour[] movePanelBehaviours;
	private List<Move> moves;
	private SelectionPanelBahviour[] selectionPanels;

	MovePlayer character1;
	bool hasMoved;

	void Start () 
	{
		hasMoved = false;

		moves = AvailableMoves.GetMoves ();
        character1 = GameObject.Find("Character 1").GetComponent<MovePlayer>();
		selectionPanels = new SelectionPanelBahviour[2];
		for (int i = 0; i < selectionPanels.Length; i++) {
			selectionPanels[i] = GameObject.Find ("Panel" + (i+1)).GetComponent<SelectionPanelBahviour> ();
			Character character = StaticCharacterHolder.characters [i];
			if (character == null) {
				continue;
			}
			selectionPanels [i].SetOwner (character);
		}

        movePanelBehaviours = new MovePanelBehaviour[moves.Count];

        for (int i = 0; i < moves.Count; i++)
		{
			Move move = moves [i];
			GameObject previewPanel = CreateMovePanel (move, transform);
			movePanelBehaviours[i] = previewPanel.GetComponent<MovePanelBehaviour> ();
        }

        float viewPortHeight = transform.parent.GetComponent<RectTransform>().rect.height;
        panelHeight = movePanelBehaviours[0].gameObject.GetComponent<RectTransform>().rect.height;
        nbrOfVisiblePanels = (int)Mathf.Round(viewPortHeight / panelHeight);

        movePanelBehaviours[0].Select();
        character1.SetAutoLoopEnabled(true);
        character1.PlayMove(moves[currentY]);
    }
    
	// Move selection within the grid of available moves.
	void Update () 
	{
        if (Input.GetKeyDown (KeyCode.UpArrow))
        {
            bool inTopPanels = currentY <= (nbrOfVisiblePanels / 2);

            if (currentY > 0) {
				movePanelBehaviours [currentY].DeSelect ();
				currentY--;
				movePanelBehaviours [currentY].Select ();
			}

            bool inBotPanels = currentY >= movePanelBehaviours.Length - (nbrOfVisiblePanels / 2) - 1;

            if (!inTopPanels && !inBotPanels)
            {
                Vector3 currentPosition = GetComponent<RectTransform>().localPosition;
                Vector3 newPosition = new Vector3(currentPosition.x, currentPosition.y - panelHeight, currentPosition.z);
                GetComponent<RectTransform>().localPosition = newPosition;
			}
			hasMoved = true;
        } 
		else if (Input.GetKeyDown (KeyCode.DownArrow))
        {
            bool inBotPanels = currentY >= movePanelBehaviours.Length - (nbrOfVisiblePanels / 2) - 1;
            
            if (currentY < movePanelBehaviours.Length - 1) {
				movePanelBehaviours [currentY].DeSelect ();
				currentY++;
				movePanelBehaviours [currentY].Select ();
            }
            
            bool inTopPanels = currentY <= nbrOfVisiblePanels / 2;         
            
            if (!inTopPanels && !inBotPanels)
            {
                Vector3 currentPosition = GetComponent<RectTransform>().localPosition;
                Vector3 newPosition = new Vector3(currentPosition.x, currentPosition.y + panelHeight, currentPosition.z);
                GetComponent<RectTransform>().localPosition = newPosition;
            }
            hasMoved = true;
		}
		else if (Input.anyKeyDown)
		{
			foreach (string button in InputSettings.registeredButtons)
			{
				if(Input.GetKeyDown(button))
				{
					RegisterPlayerMoveToButton (button);
				}
			}
		}
		if (hasMoved)
		{
			character1.SetAutoLoopEnabled(true);
			character1.PlayMove(moves[currentY]);
			hasMoved = false;
		}
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
		Move selectedMove = movePanelBehaviours [currentY].getMove ();
		Character registeredCharacter = InputSettings.Register (button, selectedMove.GetName ());
		if (registeredCharacter != null)
		{
			registeredCharacter.AddMove (selectedMove);
			Color characterColor = registeredCharacter.GetColor ();
			int characterNbr = registeredCharacter.GetNbr ();
			movePanelBehaviours [currentY].AssignButton (button, characterColor, characterNbr);
			for (int i = 0; i < movePanelBehaviours.Length; i++)
			{
				if (i != currentY)
				{
					movePanelBehaviours [i].ClearAssignedButton (button, characterNbr);
				}
			}
			AddPanelToPlayerMoves (registeredCharacter, button);
		}
	}

	private void AddPanelToPlayerMoves(Character character, string button)
	{
		foreach (SelectionPanelBahviour selectionPanel in selectionPanels)
		{
			if (selectionPanel.GetOwner().Equals(character))
			{
				GameObject original = movePanelBehaviours [currentY].gameObject;
				selectionPanel.AddPanelClone (original, button, character.GetColor());
			}
		}
	}
}
