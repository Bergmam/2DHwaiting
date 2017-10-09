﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class Parameters {

    public static int health = 400;
    public static float mass = 1.0f; //
    public static float jumpForce = 300.0f; // InputController 
    public static float moveSpeed = 5.0f; // 
    public static float knockbackModifyer = 130.0f;
    public static float blockKnockbackModifier = 80.0f;

    public static int minSpeed = 20; // 
    public static int maxSpeed = 81; // 

    public static int minStrength = 10; // 
    public static int maxStrength = 101; // 

    public static int minBlock = 30; // 
    public static int maxBlock = 121; //

    public static float minStun = 0.1f; //Time in seconds
    public static float maxStun = 3.0f; //Time in seconds

    public static float characterCamMargin = 5f;

	public static int blockTime = 20; // Last frame of a move will be added blockTime*2 times (forward & reversed) extra to block moves.
	public static float blockSpeedPercentage = 0.9f; //How fast a block move extends [0.0 - 1.0].

	public static float minShieldScale = 0.25f;
	public static float maxShieldScale = 1.0f;

    public static float scrollDelay = 0.15f;
}
