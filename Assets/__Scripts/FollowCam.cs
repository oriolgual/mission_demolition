using UnityEngine;
using System.Collections;

public class FollowCam : MonoBehaviour {
	static public FollowCam Instance; // a FollowCam Singleton
	public float easing = 0.05f;
	public Vector2 minXY = new Vector2(0,0);

	// fields set dynamically
	public GameObject poi; // The point of interest
	float camZ; // The desired Z pos of the camera
	
	void Awake() {
		Instance = this;
		camZ = transform.position.z;
	}
	
	void FixedUpdate () {
		if (poi == null) return; // return if there is no poi
		
		// Get the position of the poi
		Vector3 destination = poi.transform.position;

		// Limit the X & Y to minimum values
		destination.x = Mathf.Max( minXY.x, destination.x );
		destination.y = Mathf.Max( minXY.y, destination.y );

		destination = Vector3.Lerp (transform.position, destination, easing);

		// Retain a destination.z of camZ
		destination.z = camZ;

		// Set the camera to the destination
		transform.position = destination;

		this.camera.orthographicSize = destination.y + 10;
	}
}