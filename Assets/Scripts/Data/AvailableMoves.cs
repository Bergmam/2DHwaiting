using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Holds all recorded moves that can be chosen in the Move Selection Screen.
/// </summary>
public class AvailableMoves
{

	private static List<Move> moves;

	//TODO: store moves as they are completed in the move editor.

	/// <summary>
	/// Returns a list of all the available moves.
	/// </summary>
	/// <returns>The moves.</returns>
	public static List<Move> GetMoves()
	{
		return moves;
	}

}
