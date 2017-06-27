using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Class containing general util methods for calculating rotations in the game.
/// </summary>
public class RotationUtils{
	/// <summary>
	/// Returns true if the angle is between low and high.
	/// </summary>
	/// <param name="angle">Angle.</param>
	/// <param name="low">Low.</param>
	/// <param name="high">High.</param>
	public static bool InLimits(float angle, float low, float high){
		bool zeroInLimits = high < low;
		bool inLimitsAroundZero = zeroInLimits && (angle < high || angle > low);
		bool inLimitsWithoutZero = !zeroInLimits && (angle > low && angle < high);
		return inLimitsAroundZero || inLimitsWithoutZero;
	}	

	/// <summary>
	/// The method returns midpoint between two angles low and high. Angles from 0 to 360.
	/// </summary>
	/// <param name="low">Low.</param>
	/// <param name="high">High.</param>
	public static float MiddleOfRotations(float low, float high){
		return ((high + 360 - low) / 2) % 360;
	}
}
