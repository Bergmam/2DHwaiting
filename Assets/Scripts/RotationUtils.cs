using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotationUtils{
	//Returns true if angle is between low and high. Angles from 0 to 360.
	public static bool InLimits(float angle, float low, float high){
		bool zeroInLimits = high < low;
		bool inLimitsAroundZero = zeroInLimits && (angle < high || angle > low);
		bool inLimitsWithoutZero = !zeroInLimits && (angle > low && angle < high);
		return inLimitsAroundZero || inLimitsWithoutZero;
	}

	//Returns midpoint between low and high. Angles from 0 to 360.
	public static float MiddleOfRotations(float low, float high){
		return ((high + 360 - low) / 2) % 360;
	}
}
