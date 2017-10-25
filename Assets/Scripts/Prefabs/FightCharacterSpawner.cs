using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FightCharacterSpawner : MonoBehaviour
{

	public float x1, x2, y1, y2;

	void Start()
	{
		SpawnCharacters();
	}



	public void SpawnCharacters()
	{
		GameObject character1 = Instantiate(Resources.Load("Prefabs/Character", typeof(GameObject))) as GameObject;
		GameObject character2 = Instantiate(Resources.Load("Prefabs/Character", typeof(GameObject))) as GameObject;

		character1.transform.localScale = new Vector3 (character1.transform.localScale.x, character1.transform.localScale.y, character1.transform.localScale.z);
		character2.transform.localScale = new Vector3 (-character2.transform.localScale.x, character2.transform.localScale.y, character2.transform.localScale.z);

		RemoveUnnecessaryComponents (character1, 1);
		RemoveUnnecessaryComponents (character2, 2);

		character2.GetComponent<InputController>().horizontalAxis = "Horizontal2";
		character2.GetComponent<InputController>().verticalAxis = "Vertical2";

		character1.transform.position = new Vector3(x1, y1, 0);
		character2.transform.position = new Vector3(x2, y2, 0);
	}

	private void RemoveUnnecessaryComponents(GameObject characterObject, int index)
	{
		Transform characterTransform = characterObject.transform;
		characterObject.transform.name = "Character " + index;

		SetIndex (characterObject, index);

		characterObject.SetActive(true);

		characterObject.GetComponent<DamageTriggerDetector> ().enabled = true;
		characterObject.GetComponent<DamageTriggerDetector> ().characterIndex = index;
		characterObject.GetComponent<InputController>().enabled = true;
		characterObject.GetComponent<BoxCollider2D>().enabled = true;
		characterObject.GetComponent<CharacterCollisionDetector>().enabled = true;

		foreach (DamageTriggerDetector damageDetector in characterObject.GetComponentsInChildren<DamageTriggerDetector>())
		{
			damageDetector.enabled = true;
			damageDetector.characterIndex = index;
		}

		foreach (GameObject shield in UnityUtils.RecursiveContains(characterTransform,"Shield"))
		{
			ShieldControl shieldControl = shield.GetComponent<ShieldControl> ();
			shieldControl.enabled = true;

			DamageTriggerDetector damageDetector = shield.GetComponent<DamageTriggerDetector> ();
			damageDetector.enabled = true;
			damageDetector.characterIndex = index;
		}

		foreach (GameObject child in UnityUtils.RecursiveContains(characterTransform, "DragPoint"))
		{
			if (child.name.Contains("DragPoint"))
			{
				Destroy(child);
			}
		}
	}


	/// <summary>
	/// Sets the index of a character object. The character object has to have an input controller or it will not work.
	/// </summary>
	/// <param name="character">Character.</param>
	/// <param name="index">Index.</param>
	private void SetIndex(GameObject character, int index)
	{
		if (character.GetComponent<InputController> () == null)
		{
			return;
		}
		character.GetComponent<InputController>().characterIndex = index;
		character.GetComponent<DamageTriggerDetector>().characterIndex = index;
		foreach (DamageTriggerDetector shieldDetector in character.GetComponentsInChildren<DamageTriggerDetector>())
		{
			shieldDetector.characterIndex = index;
		}
	}
}
