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

        character1.transform.name = "Character 1";
        character2.transform.name = "Character 2";

        character1.transform.position = new Vector3(x1, y1, 0);
        character2.transform.position = new Vector3(x2, y2, 0);
        character2.transform.localScale = new Vector3(-character2.transform.localScale.x, character2.transform.localScale.y, character2.transform.localScale.z);

		SetIndex (character1, 1);
		SetIndex (character2, 2);

        character1.SetActive(true);
        character2.SetActive(true);

        character1.GetComponent<InputController>().enabled = true;
        character2.GetComponent<InputController>().enabled = true;
        character1.GetComponent<BoxCollider2D>().enabled = true;
        character2.GetComponent<BoxCollider2D>().enabled = true;
        character1.GetComponent<CharacterCollisionDetector>().enabled = true;
        character2.GetComponent<CharacterCollisionDetector>().enabled = true;

        foreach (DamageTriggerDetector damageDetector in character1.GetComponentsInChildren<DamageTriggerDetector>())
        {
            damageDetector.enabled = true;
        }

        foreach (DamageTriggerDetector damageDetector in character2.GetComponentsInChildren<DamageTriggerDetector>())
        {
            damageDetector.enabled = true;
        }

        foreach (ShieldControl shieldControl in character2.GetComponentsInChildren<ShieldControl>())
        {
            shieldControl.enabled = true;
        }

        foreach (ShieldControl shieldControl in character1.GetComponentsInChildren<ShieldControl>())
        {
            shieldControl.enabled = true;
        }

        foreach (Transform child in character1.transform)
        {
            if (child.name.Contains("DragPoint"))
            {
                Destroy(child.gameObject);
            }
        }

        foreach (Transform child in character2.transform)
        {
            if (child.name.Contains("DragPoint"))
            {
                Destroy(child.gameObject);
            }
        }

        character2.GetComponent<InputController>().horizontalAxis = "Horizontal2";
        character2.GetComponent<InputController>().verticalAxis = "Vertical2";
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
