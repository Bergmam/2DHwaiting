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
	private Character player1Character;
	private Character player2Character;

	MovePlayer character1;
	bool hasMoved;

	void Start () 
	{
		hasMoved = false;

		//TODO: Get all characters from settingss
		player1Character = new Character ();
		player1Character.AddKey ("q");
		player1Character.AddKey ("w");
		player1Character.AddKey ("e");
		player2Character = new Character ();
		player2Character.AddKey ("i");
		player2Character.AddKey ("o");
		player2Character.AddKey ("p");

        StaticCharacterHolder.character1 = player1Character;
        StaticCharacterHolder.character2 = player2Character;

		moves = AvailableMoves.GetMoves ();
        character1 = GameObject.Find("Character 1").GetComponent<MovePlayer>();
		selectionPanels = new SelectionPanelBahviour[2];
		selectionPanels[0] = GameObject.Find ("Panel1").GetComponent<SelectionPanelBahviour> ();
		selectionPanels[1] = GameObject.Find ("Panel2").GetComponent<SelectionPanelBahviour> ();

        movePanelBehaviours = new MovePanelBehaviour[moves.Count];

        for (int i = 0; i < moves.Count; i++)
		{
			Move move = moves [i];
			GameObject previewPanel = CreateMovePanel (move.GetName (), move.GetSpeed (), move.GetStrength (), transform);
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
			SelectPlayerMove (player1Character, Color.red, 1);
			SelectPlayerMove (player2Character, Color.blue, 2);
		}
		if (hasMoved)
		{
			character1.SetAutoLoopEnabled(true);
			character1.PlayMove(moves[currentY]);
			hasMoved = false;
		}
	}

	public GameObject CreateMovePanel(string name, int speed, int strength, Transform parent){
		string previewPath = "Prefabs" + Path.DirectorySeparatorChar + "MovePanel";
		GameObject previewPanelObject = (GameObject)Resources.Load(previewPath);
		GameObject previewPanel = Instantiate (previewPanelObject, previewPanelObject.transform.position, previewPanelObject.transform.rotation, parent);
		MovePanelBehaviour panelBehaviour = previewPanel.GetComponent<MovePanelBehaviour> ();
		panelBehaviour.SetName(name);
		panelBehaviour.SetSpeed(speed);
		panelBehaviour.SetStrength(strength);
		return previewPanel;
	}

	private void SelectPlayerMove(Character playerCharacter, Color playerColor, int playerNumber)
	{
		foreach(string key in playerCharacter.GetKeys())
		{
			if(Input.GetKeyDown(key))
			{
				bool successfulSelection = playerCharacter.SetMove (key, moves [currentY]);
				if(successfulSelection)
				{
					movePanelBehaviours [currentY].AssignButton (key, Color.red, playerNumber);
					GameObject original = movePanelBehaviours [currentY].gameObject;
					selectionPanels [playerNumber - 1].AddPanelClone (original, key, playerColor);

					for (int i = 0; i < movePanelBehaviours.Length; i++)
					{
						if (i != currentY)
						{
							movePanelBehaviours [i].ClearAssignedButton (key, playerNumber);
						}
					}
				}
			}
		}
	}
}
