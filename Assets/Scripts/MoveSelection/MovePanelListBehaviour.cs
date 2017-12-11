using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.IO;
using UnityEngine.UI;

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
    private MovePlayer character2;
    private GameObject playButton;
    private float scrollDelay;
	private bool hasDeleted = false;
    SceneButtonBehaviour playButtonBehaviour;

	public GameObject deleteMovePrompt;
	private Move moveToBeDeleted;
	private bool controlsActive = true;


    void Start () 
	{

		deleteMovePrompt.SetActive(false);
        scrollDelay = 0;
		//Hide the play button untill each character has a move assigned to each of its used buttons.
		playButton = GameObject.Find ("PlayButton");
		playButton.SetActive (false);
		//Create a list item for each available move.
		moves = AvailableMoves.GetMoves ();
		if (moves.Count == 0) { //Don't view anything if there are no moves.
			return;
		}

		//Make list items as high as the MeasuingPanel which is 1/7 of the viewport.
		RectTransform measuringPanel = GameObject.Find ("MeasuringPanel").GetComponent<RectTransform> ();
		listItemHeight = measuringPanel.rect.height;
		nbrOfVisiblePanels = 7;

		//Update viewport content size to make room for all moves.
		GameObject listViewContent = GameObject.Find ("Content");
		RectTransform listViewContentRect = listViewContent.GetComponent<RectTransform> ();
		Vector2 viewContentSize = listViewContentRect.sizeDelta;
		Vector2 viewContentNewSize = new Vector2 (viewContentSize.x, (moves.Count + 1) * listItemHeight); // +1 for margin
		listViewContentRect.sizeDelta = viewContentNewSize;

        listItems = new MovePanelBehaviour[moves.Count];
        for (int i = 0; i < moves.Count; i++)
		{
			Move move = moves [i];
			GameObject previewPanel = CreateMovePanel (move, transform);
			listItems[i] = previewPanel.GetComponent<MovePanelBehaviour> (); //Get the interaction script from the created list item.
        }

		//Get a reference to the interaction script of each panel viewing the currently selected moves of each character and place them in a list.
		selectedMovesPanels = new SelectionPanelBahviour[2];
		for (int i = 0; i < selectedMovesPanels.Length; i++) {
			// (Panel(i+1) is used becuase the panels are named Panel1 and Panel2, not Panel0 and Panel1)
			selectedMovesPanels[i] = GameObject.Find ("SelectedMovesPanel" + (i+1)).GetComponent<SelectionPanelBahviour> ();
			Character character = StaticCharacterHolder.characters [i];
			//Make sure there is actually a characters present for each selected moves panel.
			if (character == null) {
				continue;
			}
			selectedMovesPanels [i].SetOwner (character);
		}
		listItems[selectedListIndex].Select();
		//listItems[0].Select(); //Select the top list item.
		character1 = GameObject.Find("Character 1").GetComponent<MovePlayer>(); //Choose the character to use for animating move previews.
        character2 = GameObject.Find("Character 2").GetComponent<MovePlayer>();
        PlayAnimation (1); //Start previewing the animation corresponding to the first list item.
        playButtonBehaviour = GameObject.Find("Handler").GetComponent<SceneButtonBehaviour>(); //Used to switch scene by pressing Enter.

		if (selectedListIndex >= nbrOfVisiblePanels / 2 && hasDeleted) {
			ScrollList(-1);
			hasDeleted = false;
		}

		ReselectMoves ();
    }

	/// <summary>
	/// Goes through all previously selected moves from InputSettings and selects them in the GUI.
	/// </summary>
	public void ReselectMoves()
	{
		List<string> usedButtons = InputSettings.allUsedButtons;
		//For every used button, if the move asigned to it matches that of a panel list item. Mark that list item in GUI.
		foreach (string button in usedButtons) {
			string buttonMoveName = InputSettings.GetMoveName (button);
			for (int i = 0; i < listItems.Length; i++) {
				MovePanelBehaviour listItem = listItems [i];
				Move panelMove = listItem.getMove ();
				if (panelMove == null)
				{
					continue;
				}
				if (panelMove.GetName ().Equals (buttonMoveName))
				{
					RegisterPlayerMoveToButton (button, i);
				}
			}
		}
	}
    
	void Update () 
	{
		if (controlsActive) {
			bool vertical1Up = Input.GetAxisRaw("Vertical") > 0;
			bool vertical1Down = Input.GetAxisRaw("Vertical") < 0;
			bool vertical2Up = Input.GetAxisRaw("Vertical2") > 0;
			bool vertical2Down = Input.GetAxisRaw("Vertical2") < 0;

			if (scrollDelay > 0)
			{
				scrollDelay -= Time.deltaTime;
			} 

			else
			{
				if ((vertical1Up || vertical2Up) && scrollDelay <= 0) //Up arrow pressed
				{
					scrollDelay = Parameters.scrollDelay;
					bool movedOutOfTopPanels = selectedListIndex <= (nbrOfVisiblePanels / 2);
					MoveSelection(-1);
					bool movedIntoBotPanels = selectedListIndex >= listItems.Length - (nbrOfVisiblePanels / 2) - 1;
					if (!movedOutOfTopPanels && !movedIntoBotPanels)
					{
						ScrollList(-1);
					}
				}
				else if ((vertical1Down || vertical2Down) && scrollDelay <= 0) //Down arrow pressed
				{
					scrollDelay = Parameters.scrollDelay;
					bool movedOutOfBotPanels = selectedListIndex >= listItems.Length - (nbrOfVisiblePanels / 2) - 1;
					MoveSelection(1);
					bool moveIntoTopPanels = selectedListIndex <= nbrOfVisiblePanels / 2;
					if (!moveIntoTopPanels && !movedOutOfBotPanels)
					{
						ScrollList(1);
					}
				}
				if (vertical1Up || vertical1Down)
				{
					PlayAnimation(1);
				}
				else if (vertical2Up || vertical2Down)
				{
					PlayAnimation(2);
				}
			}

			if ((Input.GetKeyDown("enter") || Input.GetKeyDown("return")) && InputSettings.AllButtonsAssigned())
			{
				playButtonBehaviour.SwitchScene("FightScene");
			}
			if (Input.GetKeyDown("delete")) {
				//DeleteMove();
				ShowDeleteMovePanel();
			}
			if (Input.anyKeyDown)
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
	private void PlayAnimation(int characterNumber)
	{
        if (characterNumber == 1)
        {
            character2.reset();
            character1.SetAutoLoopEnabled(true);
            character1.PlayMove(moves[selectedListIndex]);
        }
        else if (characterNumber == 2)
        {
            character1.reset();
            character2.SetAutoLoopEnabled(true);
            character2.PlayMove(moves[selectedListIndex]);
        }
		
	}

	/// <summary>
	/// Creates a move panel.
	/// </summary>
	/// <returns>The move panel.</returns>
	/// <param name="move">Move.</param>
	/// <param name="parent">Parent.</param>
	public GameObject CreateMovePanel(Move move, Transform parent)
	{
		string previewPath = "Prefabs" + Path.DirectorySeparatorChar + "MovePanel";
		GameObject previewPanelObject = (GameObject)Resources.Load (previewPath);
		GameObject previewPanel = Instantiate (previewPanelObject, previewPanelObject.transform.position, previewPanelObject.transform.rotation, parent);
		previewPanel.GetComponent<LayoutElement> ().preferredHeight = listItemHeight; //Set the preferred height to 1/7 the height of the viewport.
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
		RegisterPlayerMoveToButton (button, selectedListIndex);
	}

	/// <summary>
	/// Registers the move of a list item at a specific index to a button.
	/// </summary>
	/// <param name="button">The button to assign the move to.</param>
	/// <param name="index">The index of the list item.</param>
	private void RegisterPlayerMoveToButton(string button, int index)
	{
		Move selectedMove = listItems [index].getMove ();
		Character registeredCharacter = InputSettings.Register (button, selectedMove.GetName ());
		//The character returned by InputSettings.Register is the character that uses the button.
		//If the returned character is null, no character uses the button.
		if (registeredCharacter != null)
		{
			registeredCharacter.AddMove (selectedMove);
			Color characterColor = registeredCharacter.GetColor ();
			int characterNbr = registeredCharacter.GetNbr ();
			listItems [index].AssignButton (button, characterColor, characterNbr); //Mark the selected list item with button and player color.
			//Remove the marking from any list item of a move previously registered to the button.
			for (int i = 0; i < listItems.Length; i++)
			{
				if (i != index) //Do not remove the marking from the list item that was just marked.
				{
					listItems [i].ClearAssignedButton (button);
				}
			}
			AddPanelToCharacterMoves (registeredCharacter, button, index);
			ShowOrHidePlayButton ();
		}
	}

	private void ShowDeleteMovePanel() {
		moveToBeDeleted = listItems[selectedListIndex].getMove();
		controlsActive = false;
		deleteMovePrompt.SetActive(true);
	}

	/// <summary>
	/// Deletes the currently selected move from the list of moves
	///
	public void DeleteMove() {
		
		foreach(SelectionPanelBahviour panel in selectedMovesPanels)
		{
			panel.RemovePanelWithMove(moveToBeDeleted.GetName());
		}

		AvailableMoves.DeleteMove(moveToBeDeleted);
		playButton.SetActive(true);
		foreach(MovePanelBehaviour panel in listItems)
		{
			Destroy(panel.gameObject);
		}
		selectedListIndex = Mathf.Max(selectedListIndex - 1, 0);
		
		hasDeleted = true;
		controlsActive = true;

		SaveLoad.Save(moves);
		Start();
	}

	public void CancelDeleteMove() {
		controlsActive = true;
		deleteMovePrompt.SetActive(false);
	}

	/// <summary>
	/// Adds a panel to the selected moves of a character.
	/// </summary>
	/// <param name="character">Character.</param>
	/// <param name="button">Button.</param>
	private void AddPanelToCharacterMoves(Character character, string button, int index)
	{
		//Find the panel of the character.
		foreach (SelectionPanelBahviour selectionPanel in selectedMovesPanels)
		{
			if (selectionPanel.GetOwner ().Equals (character))
			{
				GameObject original = listItems [index].gameObject; //Copy the selected list item.
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
