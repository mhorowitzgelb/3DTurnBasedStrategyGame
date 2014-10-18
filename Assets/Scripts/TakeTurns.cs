using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class TakeTurns : MonoBehaviour {

	public Material selectMaterial;

	LoadMap loadMap;
	// Use this for initialization
	void Start () {
		loadMap = GetComponent<LoadMap> ();
	}

	private bool makingMove = false;
	private Dictionary<int,Move> d;
	private Piece currentPiece;
	// Update is called once per frame
	void Update () {
		if (Input.GetMouseButtonDown (0)) { // if left button pressed...
				Ray ray = Camera.main.ScreenPointToRay (Input.mousePosition);
				RaycastHit hit;
				if (Physics.Raycast (ray, out hit)) {
					if(makingMove){
						Square square = hit.transform.GetComponent<Square>();
						if(square != null){
							Move move;
							d.TryGetValue(Move.GetHash(square.position.X, square.position.Z), out move);
							if(move != null){
							currentPiece.transform.position = LoadMap.GetPiecePosition(currentPiece.transform , new Position(move.Position.X,loadMap.map[move.Position.Z,move.Position.X].position.Y, move.Position.Z));
							currentPiece.position = move.Position;
							makingMove = false;
							foreach(Move moveLeftOver in d.Values){
								loadMap.map[moveLeftOver.Position.Z, moveLeftOver.Position.X].renderer.materials = loadMap.map[moveLeftOver.Position.Z, moveLeftOver.Position.X].origMaterials;	
							}

							}
						}

					}
					else{
						Piece piece = hit.transform.GetComponent<Piece> ();
						if (piece != null) {
						makingMove = true;
								d = new Dictionary<int,Move> ();
								GenerateMoveTable (d, new Move (piece.position, piece.moveRange, null));
								foreach (Move move in d.Values) {
										loadMap.map [move.Position.Z, move.Position.X].squareObject.renderer.materials = new Material[]{selectMaterial};	
								}
						currentPiece = piece;
					}
				}
			}
		}
	}

	void GenerateMoveTable(Dictionary<int,Move> moves, Move move){
		if (move.MovesLeft < 0)
			return;
		Move similarMove;

		moves.TryGetValue (move.GetHash (), out similarMove);

		if (similarMove == null) {
			moves.Add (move.GetHash (), move);
		} else if (move.MovesLeft > similarMove.MovesLeft) {
			moves.Remove(move.GetHash());
			moves.Add (move.GetHash (), move);
		}else
				return;



		
		int x = move.Position.X;
		int z = move.Position.Z;

		//Debug.Log (loadMap.map [z, x].squareObject.transform.position);
		//loadMap.map [z, x].squareObject.renderer.materials = new Material[]{selectMaterial};


		if (z < loadMap.worldHeight - 1) {
			GenerateMoveTable (moves, new Move(new Position(x, loadMap.map[z+1,x].position.Y, z + 1),move.MovesLeft - 1 - (int)Mathf.Abs(loadMap.map[z+1,x].position.Y - loadMap.map[z,x].position.Y),move));
		}
		if(z > 0)
			GenerateMoveTable (moves, new Move(new Position(x, loadMap.map[z-1,x].position.Y, z - 1),move.MovesLeft - 1 - (int)Mathf.Abs(loadMap.map[z-1,x].position.Y - loadMap.map[z,x].position.Y),move));
		if(x < loadMap.worldWidth -1)
			GenerateMoveTable (moves, new Move(new Position(x + 1, loadMap.map[z,x + 1].position.Y, z),move.MovesLeft - 1 - (int)Mathf.Abs(loadMap.map[z,x+1].position.Y - loadMap.map[z,x].position.Y),move));
		if(x > 0)
			GenerateMoveTable (moves, new Move(new Position(x - 1, loadMap.map[z,x - 1].position.Y, z), move.MovesLeft - 1 - (int)Mathf.Abs(loadMap.map[z,x - 1].position.Y - loadMap.map[z,x].position.Y),move));
	}

}
