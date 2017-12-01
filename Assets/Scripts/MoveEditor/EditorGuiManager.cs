using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EditorGuiManager : MonoBehaviour {

	private GameObject[] statePanels;
	private int currentStateIndex;

	void Start ()
	{
		GameObject moveTypePanel = GameObject.Find ("MoveTypePanel");
		GameObject slidersPanel = GameObject.Find ("SlidersPanel");
		GameObject activeBodypartPanel = GameObject.Find ("ActiveBodypartPanel");
		GameObject recordFramesPanel = GameObject.Find ("RecordFramesPanel");
		GameObject nameAndSavePanel = GameObject.Find ("NameAndSavePanel");
		statePanels = new GameObject[] {
			moveTypePanel,
			slidersPanel,
			activeBodypartPanel,
			recordFramesPanel,
			nameAndSavePanel
		};
	}

	/// <summary>
	/// Init this instance. This is needed because other classes need to register the components of several phases.
	/// If this class hides those components before the other classes can access them, errors are produced.
	/// </summary>
	public void Init()
	{
		currentStateIndex = 0;
		ShowOnlyCurrentPanel ();
	}

	/// <summary>
	/// Shows the only panel related to the current phase.
	/// </summary>
	private void ShowOnlyCurrentPanel()
	{
		for (int i = 0; i < statePanels.Length; i++)
		{
			bool isCurrentPanel = (i == currentStateIndex);
			statePanels [i].SetActive (isCurrentPanel);
		}
	}

	/// <summary>
	/// Moves to the next state if not already at the last state.
	/// Hides all components except the ones used in the new state.
	/// </summary>
	public void NextState()
	{
		if (currentStateIndex < statePanels.Length - 1)
		{
			currentStateIndex++;
		}
		ShowOnlyCurrentPanel ();
	}

	/// <summary>
	/// Moves to the previous state if not already at the first state.
	/// Hides all components except the ones used in the new state.
	/// </summary>
	public void PreviousState()
	{
		if (currentStateIndex > 0)
		{
			currentStateIndex--;
		}
		ShowOnlyCurrentPanel ();
	}
}
