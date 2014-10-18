using UnityEngine;
using System.Collections;

public class LoadMap : MonoBehaviour {



	
	public GameObject[] squares;
	public int worldWidth;
	public int worldHeight;
	[System.NonSerialized] public Square[,] map;
	public GameObject[] enemyTypes;
	public int[] pieceStartCount;

	public Team[] teams;

	// Use this for initialization
	void Start () {
		map = new Square[worldHeight, worldWidth];
		int xRand = Random.Range (0, 1000);
		int zRand = Random.Range (0, 1000);
		for (int z = 0; z < worldHeight; z ++) {
				for (int x = 0; x < worldWidth; x ++) {
						int value = (int)(Mathf.PerlinNoise (xRand + x / 10f, zRand + z / 10f) * squares.Length);
						GameObject squareObj = Instantiate (squares [value], LoadMap.GetSquarePosition(new Position(x, value, z)), new Quaternion ()) as GameObject;
						squareObj.transform.localScale = new Vector3 (1, value + 1, 1);
						Square square = squareObj.GetComponent<Square> ();
						square.position = new Position(x,value,z);
						square.origMaterials = squareObj.renderer.materials;
						square.squareObject = squareObj;
						map [z, x] = square; 
				}
		}
		int totalEnemies = 0;
		for (int i =0; i < pieceStartCount.Length; i ++) {
				totalEnemies += pieceStartCount[i];		
		}

		int groupWidth = (int)Mathf.Ceil (Mathf.Sqrt (totalEnemies));



		foreach (Team team in teams) {
			int currentEnemyType = 0;
			int currentPieceCount = 0;
			for (int z = team.startY; z < groupWidth + team.startY; z ++) {
				for (int x = team.startX; x < groupWidth + team.startX; x++) {
					if (currentEnemyType >= enemyTypes.Length)
							break;
					if (currentPieceCount >= pieceStartCount [currentEnemyType]) {
							currentPieceCount = 0;
							currentEnemyType ++;
					}
					GameObject gameObj = Instantiate (enemyTypes [currentEnemyType], GetPiecePosition(enemyTypes[currentEnemyType].transform, new Position(x,map[z,x].position.Y, z)), new Quaternion ()) as GameObject;
					gameObj.renderer.material.color = team.teamColor;
					gameObj.GetComponent<Piece>().position = new Position(x, map[z,x].position.Y, z);
				}
			}
		}
	}

	public static Vector3 GetPiecePosition(Transform transform, Position position){
		return new Vector3 ((float)position.X, position.Y + 1 + 0.5f * transform.lossyScale.y, (float) position.Z);
	}

	public static Vector3 GetSquarePosition(Position position){
		return new Vector3 (position.X, (position.Y + 1) / 2f, position.Z);
	}

}
