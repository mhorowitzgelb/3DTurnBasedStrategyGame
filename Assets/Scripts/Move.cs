using UnityEngine;
using System.Collections;

public class Move {

	public Position Position{ get; private set; }
	public int MovesLeft{ get; private set; }
	public Move LastMove{ get; private set; }

	public Move(Position position, int moves, Move lastMove){
		Position = position;
		MovesLeft = moves;
		LastMove = lastMove;
	}

	public int GetHash(){
		return Move.GetHash(Position.X, Position.Z);
	}

	public static int GetHash(int x, int z){
		return (x * 73856093) ^ (z * 19349663);	
	}
}
