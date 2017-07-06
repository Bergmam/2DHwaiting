using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Holds data and moves of a character.
/// </summary>
public class Character
{

	private Dictionary<string,Move> moves;
	private Color color;
	private int nbr; //Used for comparisons and, in some cases, as index in lists.

	/// <summary>
	/// Initializes a new instance of the <see cref="Character"/> class.
	/// </summary>
	/// <param name="color">Color.</param>
	/// <param name="nbr">Nbr.</param>
	public Character(Color color, int nbr){
		moves = new Dictionary<string,Move> ();
		this.color = color;
		this.nbr = nbr;
	}

	public Color GetColor()
	{
		return this.color;
	}

	public int GetNbr()
	{
		return this.nbr;
	}

	/// <summary>
	/// Adds the move if it is not already added.
	/// </summary>
	/// <param name="move">Move.</param>
	public void AddMove(Move move)
	{
		if (!moves.ContainsKey (move.GetName ()))
		{
			moves.Add (move.GetName (), move);
		}
	}

	/// <summary>
	/// Deletes the move.
	/// </summary>
	/// <param name="moveName">Move name.</param>
	public void DeleteMove(string moveName)
	{
		if (moves.ContainsKey (moveName))
		{
			moves.Remove (moveName);
		}
	}

	/// <summary>
	/// Gets move with specified name.
	/// </summary>
	/// <returns>The move if it is in list of moves. Null otherwise.</returns>
	/// <param name="moveName">Move name.</param>
	public Move GetMove(string moveName)
	{
		if (moves.ContainsKey (moveName)) {
			return moves [moveName];
		} else 
		{
			return null;
		}
	}
}
