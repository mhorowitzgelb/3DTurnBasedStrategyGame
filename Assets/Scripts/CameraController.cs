using UnityEngine;
using System.Collections;

public class CameraController : MonoBehaviour {
	

	// Update is called once per frame
	void Update () {
		transform.Translate(new Vector3(Input.GetAxis ("Horizontal") * 20 * Time.deltaTime,
		                                0,
		                                Input.GetAxis ("Vertical") * 20 * Time.deltaTime), Space.World);
	}
}
