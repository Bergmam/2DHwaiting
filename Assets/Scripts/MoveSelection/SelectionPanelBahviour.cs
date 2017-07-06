using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using UnityEngine.UI;

public class SelectionPanelBahviour : MonoBehaviour {

	private List<GameObject> panels;
	private Character owner;

	// Use this for initialization
	void Start () {
		panels = new List<GameObject> ();
	}

	public void AddPanelClone (GameObject original, string button, Color32 color)
	{
		Transform originalTransform = original.transform;
		string moveName = originalTransform.Find ("NameText").GetComponent<Text> ().text;
		RemovePanelWithButton (button);
		RemovePanelWithMove (moveName);
		GameObject previewPanel = Instantiate (original.gameObject, originalTransform.position, originalTransform.rotation, transform);
		MovePanelBehaviour panelBehaviour = previewPanel.GetComponent<MovePanelBehaviour> ();
		panelBehaviour.DeSelect ();
		panelBehaviour.ClearAssignedButton (1);
		panelBehaviour.AssignButton (button, color, 2);
		panels.Add (previewPanel);
	}

	private void RemovePanelWithMove(string moveName)
	{
		GameObject panelWithMove = null;
		foreach(GameObject panel in panels)
		{
			Text moveText = panel.transform.Find("NameText").GetComponent<Text>();
			if (moveText.text.Equals (moveName))
			{
				panelWithMove = panel;
			}
		}
		if (panelWithMove != null)
		{
			panels.Remove (panelWithMove);
			Destroy (panelWithMove);
		}
	}

	private void RemovePanelWithButton(string button)
	{

		GameObject panelWithButton1 = null;
		GameObject panelWithButton2 = null;
		foreach(GameObject panel in panels)
		{
			Text assignedButton1Text = panel.transform.Find("AssignedButton1Text").GetComponent<Text>();
			Text assignedButton2Text = panel.transform.Find("AssignedButton2Text").GetComponent<Text>();
			if (assignedButton1Text.text.Equals (button))
			{
				panelWithButton1 = panel;
			}
			if (assignedButton2Text.text.Equals (button))
			{
				panelWithButton2 = panel;
			}
		}
		if (panelWithButton1 != null)
		{
			panels.Remove (panelWithButton1);
			Destroy (panelWithButton1);
		}
		if (panelWithButton2 != null)
		{
			panels.Remove (panelWithButton2);
			Destroy (panelWithButton2);
		}
	}

	public void SetOwner(Character character)
	{
		this.owner = character;
	}

	public Character GetOwner()
	{
		return this.owner;
	}
}
