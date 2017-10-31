using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EditorCharacterSpawner : MonoBehaviour
{

    public float x1, y1;
    private MoveCreator moveCreator;

    void Start()
    {
        SpawnCharacter();
    }


    public void SpawnCharacter()
    {
        GameObject preInitCharacter = Resources.Load("Prefabs/Character", typeof (GameObject)) as GameObject;
        GameObject character = Instantiate(preInitCharacter);

        character.transform.name = "Character";
        character.GetComponent<Recorder>().enabled = true;
        moveCreator = character.GetComponent<MoveCreator>();
        moveCreator.enabled = true;
        GameObject.Find("BlockToggleButton").GetComponent<Toggle>().onValueChanged.AddListener(moveCreator.SetBlockMove);
        
        Destroy(character.GetComponent<Rigidbody2D>());

        foreach (GameObject dragPoint in UnityUtils.RecursiveContains(character.transform, "DragPoint"))
        {
            dragPoint.SetActive(true);
        }

        foreach (Transform child in character.GetComponentsInChildren<Transform>(true))
        {
            Destroy(child.transform.GetComponent<Rigidbody2D>());
        }

        character.transform.position = new Vector3(x1, y1, 0);
    }

    public void MakeCharacterBlockMove(bool isBlock)
    {
        moveCreator.SetBlockMove(isBlock);
    }
}
