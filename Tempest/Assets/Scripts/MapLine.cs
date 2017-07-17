﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// MapLine is used to define a line (between two vertices) in a map.
// It should be generated by MapManager and used by ships to move around.
public class MapLine {

	// The starting vertice (left hand side) of the line
	public Vector3 startPos;
	// The ending vertice (right hand side) of the line
	public Vector3 endPos;

	public MapLine leftLine;
	public MapLine rightLine;

	// The direction vector
	private Vector3 _dir;
	private float _length;

	// Initializaiton code for MapLine
	public MapLine(Vector3 startpos, Vector3 endpos){
		startPos = startpos;
		endPos = endpos;
		_dir = GetDirectionVector ();
		_length = GetLength ();
	}
	public MapLine(Vector3 startpos, Vector3 endpos, MapLine left, MapLine right){
		startPos = startpos;
		endPos = endpos;
		leftLine = left;
		rightLine = right;
		_dir = GetDirectionVector ();
		_length = GetLength ();
	}

	// Called by ships to get information on what the actual movement should be
	public MapLine UpdateMovement(Vector3 curPos, float relativeMovement, out Vector3 newPos) // , out Quaternion newRotation
	{
		MapLine newMapLine = null;
 		// Get the Vector3 normal that represents the direction in which the movement is
		// Multiply normal by relativeMovement, adding onto curPos to get newPos
		newPos = curPos + _dir.normalized * relativeMovement;

		// If either distance is longer than the length, then it means that curPos is out of bounds
		if (Vector3.Distance(startPos, curPos) > _length || Vector3.Distance(endPos, curPos) > _length) {
			// Length from start to cur longer means that the point is to the right
			if (Vector3.Distance (startPos, curPos) > _length) {
				float tempDist = Vector3.Distance (endPos, curPos);
				newMapLine = rightLine;
				// Start from the end of the line, and continue the rest of the distance on the next line
				newPos = endPos + rightLine.GetDirectionVector() * (relativeMovement - tempDist);
			// Otherwise it is to the left
			} else {
				float tempDist = Vector3.Distance (startPos, curPos);
				newMapLine = leftLine;
				// Start from the end of the line, and continue the rest of the distance on the next line
				newPos = startPos + leftLine.GetDirectionVector() * (relativeMovement - tempDist);
			}

			
		}


		// TODO get newRotation

		// TODO assign the newRotation (if new MapLine)

		// Return null, or a new MapLine if out of bounds
		return newMapLine;
	}


	// Return whether a Vector3 point is on this MapLine.
	public bool IsOnLine(Vector3 pos){
		// TODO check pos

	}

	// Return the length of the ship
	public float GetLength()
	{
		return (endPos - startPos).magnitude;
	}

	// Return the direction vector of the line
	public Vector3 GetDirectionVector()
	{
		return (endPos - startPos);
	}

	// Sets the MapLine references
	public void SetLeftMapLine(MapLine mapLine) {
		leftLine = mapLine;
	}

	public void SetRightMapLine(MapLine mapLine) {
		rightLine = mapLine;
	}

	// Override the MapLine.Equals function
	public override bool Equals(object o)
	{
		if (o is MapLine)
			return Equals((MapLine)o);
		else
			return base.Equals(o);
	}

	// Used to compare another MapLine with this one
	public bool Equals(MapLine otherMapLine){
		if (otherMapLine.startPos == startPos && otherMapLine.endPos == endPos)
			return true;
		else
			return false;
	}

	// Without GetHashCode() Unity will complain about not having one
	// Have it your way scumbag
	public override int GetHashCode(){
		int hash = 17;
		hash = hash * 23 + startPos.magnitude.GetHashCode ();
		hash = hash * 23 + endPos.magnitude.GetHashCode ();
		return hash;
	}	


}
