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
    float panelHeight;
    int nbrOfVisiblePanels;
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
		if (Input.GetKeyDown (KeyCode.DownArrow))
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
	}
}
