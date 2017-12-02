using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EditorCharacterSpawner : MonoBehaviour
{

    public float x1, y1;
    public float x2, y2;

    private MoveCreator moveCreator;

    void Start()
    {
        SpawnCharacter();
        SpawnOnionCharacter();
    }

    /// <summary>
    /// Method to spawn a version of the character prefab with the necessary components to record moves in the editor scene.
    /// See google document for more information about what parts should be active in the edtior scene.
    /// </summary>
    public void SpawnCharacter()
    {
        GameObject preInitCharacter = Resources.Load("Prefabs/Character", typeof (GameObject)) as GameObject;
        GameObject character = Instantiate(preInitCharacter);

        character.transform.name = "Character";
        character.GetComponent<Recorder>().enabled = true;
        moveCreator = character.GetComponent<MoveCreator>();
        moveCreator.enabled = true;
        
        Destroy(character.GetComponent<Rigidbody2D>());

        foreach (GameObject dragPoint in UnityUtils.RecursiveContains(character.transform, "DragPoint"))
        {
            dragPoint.SetActive(true);
        }

        // Destroy all rigid bodies since dragpoints does not work with them attached.
        foreach (Transform child in character.GetComponentsInChildren<Transform>(true))
        {
            Destroy(child.transform.GetComponent<Rigidbody2D>());
        }

        character.transform.position = new Vector3(x1, y1, 0);
    }

    public void SpawnOnionCharacter()
    {
        GameObject preInitCharacter = Resources.Load("Prefabs/Character", typeof(GameObject)) as GameObject;
        GameObject character = Instantiate(preInitCharacter);

        foreach (SpriteRenderer sprite in character.GetComponentsInChildren<SpriteRenderer>())
        {
            sprite.sortingOrder = 0;
        }

        character.transform.name = "Onion Character";

        Destroy(character.GetComponent<Rigidbody2D>());

        // Destroy all rigid bodies since dragpoints does not work with them attached.
        foreach (Transform child in character.GetComponentsInChildren<Transform>(true))
        {
            Destroy(child.transform.GetComponent<Rigidbody2D>());
        }

        character.transform.position = new Vector3(x2, y2, 0);
    }

    /// <summary>
    /// Method called by the button to change a move to a block
    /// </summary>
    /// <param name="isBlock"></param>
    public void MakeCharacterBlockMove(bool isBlock)
    {
        moveCreator.SetBlockMove(isBlock);
    }

    public void Save()
    {
        moveCreator.SaveMove();
    }

    public void Reset()
    {
        moveCreator.ResetMoveEditor();
    }
}
