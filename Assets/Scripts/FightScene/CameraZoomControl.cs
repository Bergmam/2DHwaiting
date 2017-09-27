using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraZoomControl : MonoBehaviour {

    public GameObject character1;
    public GameObject character2;

    private float leftCharPos;
    private float rightCharPos;

    private float extraCanvasSpace = 5.6f;
    private float minCharDistance;
    private float startHeight;

    private Camera gameCamera;

	// Use this for initialization
	void Start () {
        gameCamera = GetComponent<Camera>();
        startHeight = gameCamera.orthographicSize;
    }
	
	// Update is called once per frame
	void Update () {

        //TODO: Check case where characters switch places.

        leftCharPos = character1.transform.position.x;
        rightCharPos = character2.transform.position.x;
        float charDistance = Mathf.Abs(rightCharPos - leftCharPos);
        float zoomSize = charDistance + extraCanvasSpace;
        float halfCamHeight = (zoomSize / gameCamera.aspect) / 2;

        float camY = gameCamera.transform.position.y;
        float camX = gameCamera.transform.position.x;

        float middleOfChars = ((rightCharPos - leftCharPos) / 2) + leftCharPos;

        if (charDistance > 4.5f) //Don't update cam height if chars are close to each other.
        {
            gameCamera.orthographicSize = halfCamHeight;
            camY = gameCamera.orthographicSize - startHeight;
            camX = middleOfChars;
        }
        else
        {
            //TODO: Update camera x position if characters are close to each other but not close to the edge of the world.
        }

        gameCamera.transform.position = new Vector3(camX, camY, gameCamera.transform.position.z);
    }
}
