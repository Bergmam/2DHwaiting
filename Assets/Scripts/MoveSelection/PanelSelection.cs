using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

//Used for enabling or disabling the child GameObject which holds the border around the panel.
public class PanelSelection : MonoBehaviour {

	private Transform borderImageTransform;
	private GameObject borderImage;
	private bool selected;

	void Start(){
		RectTransform rectTransform = gameObject.GetComponentInChildren<RectTransform> ();
		if (rectTransform != null) {
			borderImageTransform = rectTransform.Find ("BorderImage"); //BorderImage is the name of the border in the MovePreviewPanel prefab.
		}
		if (borderImageTransform != null) {
			borderImage = borderImageTransform.gameObject;
		}
	}

	public void Select(){
		if (borderImage != null) {
			selected = true;
			borderImage.SetActive (true);
		}
	}

	public void DeSelect(){
		if (borderImage != null) {
			selected = false;
			borderImage.SetActive (false);
		}
	}

	public bool IsSelected(){
		return selected;
	}
}
