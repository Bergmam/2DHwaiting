using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Class containing general util methods for calculating rotations in the game.
/// </summary>
public class RotationUtils
{
	/// <summary>
	/// Returns true if the angle is between low and high. The values are always counted in a clockwise manner.
	/// The "low"-angle is always the one with the lowest angle, and the "high" is the one with the second highest angle,
	/// regardless of which is the "from"-angle and which is the "to"-angle.
	/// </summary>
	/// <param name="angle">The angle to be checked.</param>
	/// <param name="low">The low value for angle to be compared with.</param>
	/// <param name="high">The high value for angle to be compared with.</param>
	public static bool InLimits(float angle, float low, float high)
	{
		bool zeroInLimits = high < low;
		bool inLimitsAroundZero = zeroInLimits && (angle < high || angle > low);
		bool inLimitsWithoutZero = !zeroInLimits && (angle > low && angle < high);
		return inLimitsAroundZero || inLimitsWithoutZero;
	}	

	/// <summary>
	/// The method returns midpoint between two angles low and high. Angles from 0 to 360.
	/// </summary>
	/// <param name="low">The low angle.</param>
	/// <param name="high">The High angle.</param>
	public static float MiddleOfRotations(float low, float high)
	{
		return ((high + 360 - low) / 2) % 360;
	}
}
