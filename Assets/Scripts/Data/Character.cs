using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Character {

	private Dictionary<string,Move> moves;
	private List<string> keys;

	public Character(){
		moves = new Dictionary<string,Move> ();
		keys = new List<string> ();
	}

	public Character(List<string> keys){
		moves = new Dictionary<string,Move> ();
		keys = new List<string> ();
		foreach (string key in keys)
		{
			if (key.Length == 1)
			{
				keys.Add (key);
				moves.Add (key, null);
			}
		}
	}

	public void AddKey(string key)
	{
		if (key.Length == 1)
		{
			keys.Add (key);
			moves.Add (key, null);
		}
	}

	public bool SetMove(string key, Move move){
		if (keys.Contains(key))
		{
			DeleteMove (move);
			moves.Remove (key);
			moves.Add (key, move);
			return true;
		}
		else
		{
			return false;
		}
	}

	public void DeleteMove(Move move)
	{
		string key = "";
		foreach (KeyValuePair<string, Move> entry in moves)
		{
			if(entry.Value != null && entry.Value.Equals(move))
			{				
				key = entry.Key;
			}
		}
		if (!key.Equals (""))
		{
			moves [key] = null;
		}
	}

	public bool HasKey(string key)
	{
		return moves.ContainsKey (key);
	}

	public List<string> GetKeys()
	{
		return keys;
	}
}
