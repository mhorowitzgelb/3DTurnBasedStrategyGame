using UnityEngine;
using System.Collections;

public class LoadMap : MonoBehaviour {


	private int leftBlock = 0;
	private int centerBlock = 1;
	private int rightBlock = 2;
	private int upBlock = 3;
	private int downBlock = 4;

	public int blockSize = 10;
	public TerrainType[] terrains;
	public GameObject genericSquare;
	public int worldWidth;
	public int worldLength;
	[System.NonSerialized] public Square[][,,] map;
	public GameObject[] enemyTypes;
	public int[] pieceStartCount;
	private int xRand;
	private int zRand;
	private int currentBlockX = 0;
	private int currentBlockZ = 0;

	public Team[] teams;

	// Use this for initialization	
	void Start () {
		int worldHeight = terrains.Length;
		//map = new Square[worldLength, worldWidth, worldHeight];
		xRand = Random.Range (0, 1000);
		zRand = Random.Range (0, 1000);
		map = new Square[5][,,];
		for (int i =0; i < 5; i ++) {
			map[i] = new Square[blockSize, blockSize, terrains.Length];		
		}
		int totalEnemies = 0;
		for (int i =0; i < pieceStartCount.Length; i ++) {
				totalEnemies += pieceStartCount[i];		
		}

		int groupWidth = (int)Mathf.Ceil (Mathf.Sqrt (totalEnemies));
		/*
		for (int z = 0; z < worldLength; z ++) {
			for (int x = 0; x < worldWidth; x ++) {
				int value = (int)(Mathf.PerlinNoise(xRand + x / 10f, zRand + z / 10f) * worldHeight);
				for(int y = 0; y < value; y ++){
					GameObject squareObj = Instantiate(genericSquare, new Vector3(x,y,z) , new Quaternion ()) as GameObject;
					squareObj.renderer.materials = terrains[y].materials;
					Square square = new Square();
					square.position = new Position(x,y,z);
					square.squareObject = squareObj;
					square.terrainType = terrains[y];
					map[z,x,y] = square; 
				}
			}
		}*/
		/*
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
					int y = 0;
					for(int i = terrains.Length -1; i > 0; i --){
						if(map[x,i,z] != null){
							y = i;
							break;
						}
					}
					
					GameObject gameObj = Instantiate (enemyTypes [currentEnemyType], GetPiecePosition(enemyTypes[currentEnemyType].transform, new Position(x,map[x,z,y].position.Y, z)), new Quaternion ()) as GameObject;
					gameObj.renderer.material.color = team.teamColor;
					gameObj.GetComponent<Piece>().position = new Position(x, map[z,x].position.Y, z);
				}
			}
		}*/
	}

	public IEnumerator InstantiateBlocks(Position cameraPosition){
		if (cameraPosition.X > (currentBlockX + 1) * blockSize) {
						currentBlockX ++;
						yield return new WaitForSeconds(.1f);
						DeleteBlock (map [leftBlock]);
						yield return new WaitForSeconds(.1f);
						DeleteBlock (map [upBlock]);
						yield return new WaitForSeconds(.1f);
						DeleteBlock (map [downBlock]);

						int oldR = rightBlock;
						int oldC = centerBlock;
						rightBlock = leftBlock; 
						leftBlock = oldC;
						centerBlock = oldR;
						
			yield return new WaitForSeconds(.1f);
			LoadBlockIntoMap (map [rightBlock], currentBlockZ, currentBlockX + 1);
			yield return new WaitForSeconds(.1f);			
			LoadBlockIntoMap (map [upBlock], currentBlockZ + 1, currentBlockX); 
			yield return new WaitForSeconds(.1f);			
			LoadBlockIntoMap (map [downBlock], currentBlockZ - 1, currentBlockX);
				} else if (cameraPosition.X < (currentBlockX) * blockSize) {
						currentBlockX --;
			yield return new WaitForSeconds(.1f);			
			DeleteBlock (map [rightBlock]);
			yield return new WaitForSeconds(.1f);			
			DeleteBlock (map [upBlock]);
			yield return new WaitForSeconds(.1f);			
			DeleteBlock (map [downBlock]);

						int oldL = leftBlock;
						int oldC = centerBlock;
						leftBlock = rightBlock;
						rightBlock = oldC;
						centerBlock = oldL;
			yield return new WaitForSeconds(.1f);
						LoadBlockIntoMap (map [leftBlock], currentBlockZ, currentBlockX - 1);
			yield return new WaitForSeconds(.1f);			
			LoadBlockIntoMap (map [upBlock], currentBlockZ + 1, currentBlockX); 
			yield return new WaitForSeconds(.1f);			
			LoadBlockIntoMap (map [downBlock], currentBlockZ - 1, currentBlockX);
				} else if (cameraPosition.Z > (currentBlockZ + 1) * blockSize) {
						currentBlockZ ++;
			yield return new WaitForSeconds(.1f);			
			DeleteBlock (map [downBlock]);
			yield return new WaitForSeconds(.1f);			
			DeleteBlock (map [leftBlock]);
			yield return new WaitForSeconds(.1f);			
			DeleteBlock (map [rightBlock]);

						int oldU = upBlock;
						int oldC = centerBlock;
						upBlock = downBlock;
						downBlock = oldC;
						centerBlock = oldU;
			yield return new WaitForSeconds(.1f);
						LoadBlockIntoMap (map [leftBlock], currentBlockZ, currentBlockX - 1);
			yield return new WaitForSeconds(.1f);			
			LoadBlockIntoMap (map [upBlock], currentBlockZ + 1, currentBlockX); 
			yield return new WaitForSeconds(.1f);			
			LoadBlockIntoMap (map [rightBlock], currentBlockZ, currentBlockX + 1);
				} else if (cameraPosition.Z < currentBlockZ * blockSize) {
						currentBlockZ --;
			yield return new WaitForSeconds(.1f);			
			DeleteBlock (map [upBlock]);
			yield return new WaitForSeconds(.1f);			
			DeleteBlock (map [leftBlock]);
			yield return new WaitForSeconds(.1f);			
			DeleteBlock (map [rightBlock]);
						
						int oldD = downBlock;
						int oldC = centerBlock;
						downBlock = upBlock;
						upBlock = oldC;
						centerBlock = oldD;

			yield return new WaitForSeconds(.1f);
			LoadBlockIntoMap (map [leftBlock], currentBlockZ, currentBlockX - 1);
			yield return new WaitForSeconds(.1f);			
			LoadBlockIntoMap (map [downBlock], currentBlockZ - 1, currentBlockX); 
			yield return new WaitForSeconds(.1f);			
			LoadBlockIntoMap (map [rightBlock], currentBlockZ, currentBlockX + 1);	
		}
		yield return null;
	}
	
	private void DeleteBlock(Square[,,] block){
		foreach(Square square in block){
			if(square != null)
				Destroy(square.squareObject);
		}
	}

	private void LoadBlockIntoMap(Square[,,] block, int blockZ, int blockX){

		for (int z = blockZ * blockSize; z < (blockZ + 1) * blockSize; z ++) {
			for (int x = blockX * blockSize; x < (blockX + 1) * blockSize; x ++) {
				int value = (int)(Mathf.PerlinNoise(xRand + x / 10f, zRand + z / 10f) * terrains.Length);
				for(int y = 0; y < value + 1; y ++){
					if(y >= terrains.Length)
						break;
					GameObject squareObj = Instantiate(genericSquare, new Vector3(x,y,z) , new Quaternion ()) as GameObject;
					squareObj.renderer.materials = terrains[y].materials;
					Square square = new Square();
					square.position = new Position(x,y,z);
					square.squareObject = squareObj;
					square.terrainType = terrains[y];
					block[z - blockZ * blockSize,x - blockX * blockSize, y] = square; 
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
