using System.Collections;
using System.Collections.Generic;
using UnityEngine;
﻿using System.IO;

public class InputListBehaviour : MonoBehaviour {

	// Use this for initialization
	void Start () {
		
		List<string> characterOneButtons = InputSettings.GetCharacterButtons (1);
		Color32 color1 = StaticCharacterHolder.character1.GetColor ();
		List<string> characterTwoButtons = InputSettings.GetCharacterButtons (2);
		Color32 color2 = StaticCharacterHolder.character2.GetColor ();

		for (int i = 0; i < InputSettings.MaxNrOfMoves (); i++)
		{
			Move move = new Move ();
			move.SetName ("Move " + (i + 1));
			string previewPath = "Prefabs" + Path.DirectorySeparatorChar + "MovePanel";
			GameObject previewPanelObject = (GameObject)Resources.Load (previewPath);
			GameObject previewPanel = Instantiate (previewPanelObject, previewPanelObject.transform.position, previewPanelObject.transform.rotation, transform);
			MovePanelBehaviour panelBehaviour = previewPanel.GetComponent<MovePanelBehaviour> ();
			panelBehaviour.setMove (move);
			panelBehaviour.RemoveSpeedText ();
			panelBehaviour.RemoveStrengthText ();
			panelBehaviour.AssignButton (characterOneButtons[i], color1, 1);
			panelBehaviour.AssignButton (characterTwoButtons[i], color2, 2);
		}
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
