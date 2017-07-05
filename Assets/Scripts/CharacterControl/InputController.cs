using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputController : MonoBehaviour {

    private bool isAnimating = false;
    public float speed;
    public string horizontalAxis;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
        Vector3 newPosition = new Vector3(transform.position.x + speed*Input.GetAxis(horizontalAxis), transform.position.y, transform.position.z);
        transform.position = newPosition;
        
	}

    /// <summary>
    /// Method for moving the character left/right and changing the animation
    /// </summary>
    /// <param name="keyDirection"></param>
    /*
    void moveCharacter(string keyDirection)
    {
        Transform transform = GameObject.Find("Torso").GetComponent<Transform>();
        // Update transform
        // Play move animation
        gameObject.GetComponent<Animation>().Play("Walking");
        // When done moving, change back to idle

        if (Input.GetButtonUp(keyDirection))
        {
            gameObject.GetComponent<Animation>().Play("Idle");
        }

    }
    /// <summary>
    /// Play jump animation once, then go back to idle
    /// </summary>
    void Jump()
    {
        gameObject.GetComponent<Animation>().wrapMode = WrapMode.Once;
        gameObject.GetComponent<Animation>().Play("Jump");
        gameObject.GetComponent<Animation>().wrapMode = WrapMode.Loop;
        gameObject.GetComponent<Animation>().Play("Idle");
    }
    */
}
