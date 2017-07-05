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
	private bool localMultiplayer = true;


	Character player1Character;
	Character player2Character;

	void Start () 
	{
		//TODO: Get all characters from settingss
		player1Character = new Character ();
		player1Character.AddKey ("q");
		player1Character.AddKey ("w");
		player1Character.AddKey ("e");
		player2Character = new Character ();
		player2Character.AddKey ("i");
		player2Character.AddKey ("o");
		player2Character.AddKey ("p");

		moves = AvailableMoves.GetMoves ();


        // For testing purposes
        // Makes sure we always have 10 moves
        if (moves.Count < 10)
        {
            for (int i = moves.Count; i < 10; i++)
            {
                Move move = new Move();
                move.SetName(i.ToString());
                moves.Add(move);
            }
        }

        movePanelBehaviours = new MovePanelBehaviour[moves.Count];

		string previewPath = "Prefabs" + Path.DirectorySeparatorChar + "MovePanel";
        GameObject previewPanelObject = (GameObject)Resources.Load(previewPath);

        for (int i = 0; i < moves.Count; i++)
		{
			Move move = moves [i];
			GameObject previewPanel = Instantiate (previewPanelObject, previewPanelObject.transform.position, previewPanelObject.transform.rotation, transform);
			movePanelBehaviours[i] = previewPanel.GetComponent<MovePanelBehaviour> ();
            movePanelBehaviours[i].SetName(move.GetName());
            movePanelBehaviours[i].SetSpeed(move.GetSpeed());
            movePanelBehaviours[i].SetStrength(move.GetStrength());
        }

        float viewPortHeight = transform.parent.GetComponent<RectTransform>().rect.height;
        panelHeight = movePanelBehaviours[0].gameObject.GetComponent<RectTransform>().rect.height;
        nbrOfVisiblePanels = (int)Mathf.Round(viewPortHeight / panelHeight);

        movePanelBehaviours[0].Select();
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
		}
		else if (Input.anyKeyDown)
		{
			foreach(string key in player1Character.GetKeys())
			{
				if(Input.GetKeyDown(key))
				{
					if(player1Character.SetMove(key,moves[currentY]))
					{
						movePanelBehaviours [currentY].AssignButton1 (key, Color.red);
						for (int i = 0; i < movePanelBehaviours.Length; i++)
						{
							if (i != currentY)
							{
								movePanelBehaviours [i].ClearAssignedButton1 (key);
							}
						}
					}
				}
			}
			foreach(string key in player2Character.GetKeys())
			{
				if(Input.GetKeyDown(key))
				{
					if(player2Character.SetMove(key,moves[currentY]))
					{
						movePanelBehaviours [currentY].AssignButton2 (key, Color.blue);
						for (int i = 0; i < movePanelBehaviours.Length; i++)
						{
							if (i != currentY)
							{
								movePanelBehaviours [i].ClearAssignedButton2 (key);
							}
						}
					}
				}
			}
		}
	}
}
