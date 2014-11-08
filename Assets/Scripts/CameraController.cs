using UnityEngine;
using System.Collections;

public class CameraController : MonoBehaviour {
	
	public LoadMap loadMap;
	// Update is called once per frame
	void Update () {
		transform.Translate(new Vector3(Input.GetAxis ("Horizontal") * 20 * Time.deltaTime,
		                                0,
		                                Input.GetAxis ("Vertical") * 20 * Time.deltaTime), Space.World);
		StartCoroutine(loadMap.InstantiateBlocks (new Position ((int) transform.position.x, (int) transform.position.y,(int) transform.position.z)));
	}
}
