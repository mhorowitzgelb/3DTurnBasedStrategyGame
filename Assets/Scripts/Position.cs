using UnityEngine;
using System.Collections;

public class Position{
	public int X;
	public int Y;
	public int Z;

	public Position(int x, int y, int z){
		X = x;
		Y = y;
		Z = z;
	}

	public static Position operator+(Position a, Position b){
		return new Position (a.X + b.X, a.Y + b.Y, a.Z + b.Z);
	}
}
