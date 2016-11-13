using UnityEngine;
using System.Collections;
using UnityStandardAssets.CrossPlatformInput;

public class CameraScript : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
		//Used so that whoevers turn it is can update the camera
		float x = CrossPlatformInputManager.GetAxis ("Horizontal" + Statics.playerTurn.ToString());
		float y = CrossPlatformInputManager.GetAxis ("Vertical" + Statics.playerTurn.ToString());
		if ((transform.position.x > 8.0f && x > 0.0f) || (transform.position.x < -24.0f && x < 0.0f)) {
			x = 0.0f; 
		}
		if ((transform.position.y > 5.5f && y > 0.0f) || (transform.position.y < -6.4f && y < 0.0f)) {
			y = 0.0f; 
		}
		x += transform.position.x; 
		y += transform.position.y; 
		transform.position = new Vector3 (x, y, transform.position.z);
	}
}
