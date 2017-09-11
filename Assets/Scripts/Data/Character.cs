﻿using System.Collections;
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
	private int maxHealth = 400; //Used for calculating percentage of health in health bar.
	private int health;

	/// <summary>
	/// Initializes a new instance of the <see cref="Character"/> class.
	/// </summary>
	/// <param name="color">Color.</param>
	/// <param name="nbr">Nbr.</param>
	public Character(Color color, int nbr){
		moves = new Dictionary<string,Move> ();
		this.color = color;
		this.nbr = nbr;
		this.health = maxHealth;
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

    public string PrintNames()
    {
        string name = "";
        foreach (string bajs in moves.Keys)
        {
            name += bajs;
            name += " ";
        }
        return name;
    }

    public int GetHealth()
    {
        return health;
    }

    public void SetHealth(int health)
    {
        this.health = health;
    }

    public void AddHealth(int number)
    {
        health += number;
    }

    public void SubHealth(int number)
    {
        health -= number;
    }

    public void ApplyMoveTo(Move move)
    {
		SubHealth(move.GetStrength() + 1); //Change range of move damage [0-100] -> [1-101]
	}

	/// <summary>
	/// Used for dealing damage to the character while a block move is defending it.
	/// </summary>
	/// <param name="damagingMove">Damaging move.</param>
	/// <param name="blockingMove">Blocking move.</param>
	public void ApplyMoveTo(Move damagingMove, Move blockingMove)
	{
		int damage = damagingMove.GetStrength () - blockingMove.GetSpeed ();
		if (damage > 0) {
			SubHealth(damage + 1); //Change range of move damage [0-100] -> [1-101]
		}
	}

	public override bool Equals(System.Object obj)
	{
		// If parameter is null return false.
		if (obj == null)
		{
			return false;
		}

		if (!(obj is Character))
		{
			return false;
		}

		// If parameter cannot be cast to Character return false(?)
		Character otherCharacter = obj as Character;
		if ((System.Object)otherCharacter == null)
		{
			return false;
		}

		// Return true if the fields match:
		return this.nbr == otherCharacter.nbr;
	}

	public int GetMaxHealth()
	{
		return this.maxHealth;
	}

	public void ClearMoves(){
		this.moves.Clear ();
	}
}
