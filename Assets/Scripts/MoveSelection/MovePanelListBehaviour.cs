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
	int currentY = 0;
	MovePanelBehaviour[] movePanelBehaviours;

	void Start () 
	{
		List<Move> moves = AvailableMoves.GetMoves ();

		//### TEST ### TODO: Remove this
		moves = new List<Move>();
		for (int i = 0; i < 10; i++) 
		{
            Move move = new Move();
            move.SetName(i.ToString());
            moves.Add (move);
		}
		//### TEST END ###

		movePanelBehaviours = new MovePanelBehaviour[moves.Count];

		string previewPath = "Prefabs" + Path.DirectorySeparatorChar + "MovePanel";
        GameObject previewPanelObject = (GameObject)Resources.Load(previewPath);

        for (int i = 0; i < moves.Count; i++)
		{
			Move move = moves [i];
			GameObject previewPanel = Instantiate (previewPanelObject, transform);
			MovePanelBehaviour panel = previewPanel.GetComponent<MovePanelBehaviour> ();
            panel.SetName(move.GetName());
		}
	}

	// Move selection within the grid of available moves.
	/*void Update () 
	{
		//TODO: Compare with the length of the arrays instead of the columnLength and rowLength.
		//This would remove the problem with index out of bounds exceptions on the last row.
		if (Input.GetKeyDown (KeyCode.UpArrow)) 
		{
			if (currentY > 0) {
				panels [currentX, currentY].DeSelect ();
				currentY--;
				panels [currentX, currentY].Select ();
			}
		}
		if (Input.GetKeyDown (KeyCode.DownArrow)) 
		{
			if (currentY < columnLength - 1) {
				panels [currentX, currentY].DeSelect ();
				currentY++;
				panels [currentX, currentY].Select ();
			}
		}
		if (Input.GetKeyDown (KeyCode.LeftArrow)) 
		{
			if (currentX > 0) 
			{
				panels [currentX, currentY].DeSelect ();
				currentX--;
				panels [currentX, currentY].Select ();
			}
		}
		if (Input.GetKeyDown (KeyCode.RightArrow)) 
		{
			if (currentX < rowLength - 1) 
			{
				panels [currentX, currentY].DeSelect ();
				currentX++;
				panels [currentX, currentY].Select ();
			}
		}
		//Make sure the currently selected panel is acutally selected. Without this, no panel is selected before an arrow key is pressed.
		if (!panels [currentX, currentY].IsSelected ()) 
		{
			panels [currentX, currentY].Select ();
		}
		//TODO: Add selection of moves through pressing a button corresponding to an attack.
	}*/
}
