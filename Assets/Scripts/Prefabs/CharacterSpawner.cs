using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterSpawner : MonoBehaviour
{

    public float x1, x2, y1, y2;

    void Start()
    {
        SpawnFightCharacters();
    }

    public void SpawnSelectionCharacters()
    {

    }

    public void SpawnFightCharacters()
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

        character2.GetComponent<InputController>().horizontalAxis = "Horizontal2";
        character2.GetComponent<InputController>().verticalAxis = "Vertical2";
    }

    public void SpawnEditorCharacter()
    {

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
