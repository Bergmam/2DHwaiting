using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
﻿using System.IO;

/// <summary>
///  Class for handling the grid in the MoveSelection screen
/// </summary>
public class MoveSelectionGridBehaviour : MonoBehaviour 
{
	PanelSelection[,] panels; //TODO: Switch to something more like PanelSelection[][] panels; so that not every row has to be the same length.
	int currentX;
	int currentY;
	int rowLength = 10; //TODO: Make this dependant on the screen width and gui settings.
	int columnLength;

	void Start () 
	{
		List<Move> moves = AvailableMoves.GetMoves ();

		//### TEST ### TODO: Remove this
		moves = new List<Move>();
		for (int i = 0; i < 45; i++) {
			moves.Add (new Move ());
		}
		//### TEST END ###

		//TODO: Make column length dynamic. Or simply use the panels.Length to make it more dynamic.
		/*double columnLengthDouble = moves.Count / rowLength;
		columnLength = (int) Math.Ceiling (columnLengthDouble);*/
		columnLength = 5;

		string previewPath = "Prefabs" + Path.DirectorySeparatorChar + "MovePreviewPanel";
		Move[] movesArr = moves.ToArray ();
		panels = new PanelSelection[rowLength,columnLength];
		RectTransform rectTransform = gameObject.GetComponentInChildren<RectTransform> ();
		//TODO: Make the last row shorter than the others.
		if (rectTransform != null) {
			for (int i = 0; i < moves.Count; i++) {
				currentX = i % rowLength;
				currentY = i / rowLength;
				Move move = movesArr [i];
				GameObject previewPanelObject = (GameObject) Resources.Load (previewPath);
				GameObject previewPanel = Instantiate (previewPanelObject, rectTransform);
				panels [currentX,currentY] = previewPanel.GetComponent<PanelSelection> ();
			}
		}
		currentX = 0;
		currentY = 0;
	}
	
	// Move selection within the grid of available moves.
	void Update () {
		//TODO: Compare with the length of the arrays instead of the columnLength and rowLength.
		//This would remove the problem with index out of bounds exceptions on the last row.
		if (Input.GetKeyDown (KeyCode.UpArrow)) {
			if (currentY > 0) {
				panels [currentX, currentY].DeSelect ();
				currentY--;
				panels [currentX, currentY].Select ();
			}
		}
		if (Input.GetKeyDown (KeyCode.DownArrow)) {
			if (currentY < columnLength - 1) {
				panels [currentX, currentY].DeSelect ();
				currentY++;
				panels [currentX, currentY].Select ();
			}
		}
		if (Input.GetKeyDown (KeyCode.LeftArrow)) {
			if (currentX > 0) {
				panels [currentX, currentY].DeSelect ();
				currentX--;
				panels [currentX, currentY].Select ();
			}
		}
		if (Input.GetKeyDown (KeyCode.RightArrow)) {
			if (currentX < rowLength - 1) {
				panels [currentX, currentY].DeSelect ();
				currentX++;
				panels [currentX, currentY].Select ();
			}
		}
		//Make sure the currently selected panel is acutally selected. Without this, no panel is selected before an arrow key is pressed.
		if (!panels [currentX, currentY].IsSelected ()) {
			panels [currentX, currentY].Select ();
		}
		//TODO: Add selection of moves through pressing a button corresponding to an attack.
	}
}
