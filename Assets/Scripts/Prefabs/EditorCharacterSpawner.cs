﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EditorCharacterSpawner : MonoBehaviour
{

    public float x1, y1;

    void Start()
    {
        SpawnCharacter();
    }


    public void SpawnCharacter()
    {
        GameObject character = Instantiate(Resources.Load("Prefabs/Character", typeof(GameObject))) as GameObject;

        character.transform.name = "Character";
        character.GetComponent<Recorder>().enabled = true;
        character.GetComponent<MoveCreator>().enabled = true;
        character.SetActive(true);
        Destroy(character.GetComponent<Rigidbody2D>());

        foreach (GameObject dragPoint in UnityUtils.RecursiveContains(character.transform, "DragPoint"))
        {
            print(dragPoint.name);
            dragPoint.SetActive(true);
        }

        foreach (Transform child in character.GetComponentsInChildren<Transform>(true))
        {
            Destroy(child.transform.GetComponent<Rigidbody2D>());
        }

        character.transform.position = new Vector3(x1, y1, 0);
    }
}
