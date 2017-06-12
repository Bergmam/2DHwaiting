using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class Frame {

	private Dictionary<string,float> bodyPartRotations;
	private float defaultRotation;

	public Frame(){
		bodyPartRotations = new Dictionary<string, float> ();
	}

	public void addBodyPartRoation(string bodyPart, float rotation) {
		if (bodyPartRotations.ContainsKey (bodyPart)) {
			bodyPartRotations.Remove (bodyPart);
		}
		bodyPartRotations.Add (bodyPart, rotation);
	}

	public float getRotation(string bodyPart){
		return bodyPartRotations.TryGetValue (bodyPart, defaultRotation);
	}

}
