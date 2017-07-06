using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Character {

	private Dictionary<string,Move> moves;
	private Color color;
	private int nbr;

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

	public void AddMove(Move move)
	{
		if (!moves.ContainsKey (move.GetName ()))
		{
			moves.Add (move.GetName (), move);
		}
	}

	public void DeleteMove(string moveName)
	{
		if (moves.ContainsKey (moveName))
		{
			moves.Remove (moveName);
		}
	}

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
