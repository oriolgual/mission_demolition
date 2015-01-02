using UnityEngine;
using System.Collections;

public class FollowCam : MonoBehaviour {
	static public FollowCam Instance; // a FollowCam Singleton
	public float easing = 0.05f;
	public Vector2 minXY = new Vector2(0,0);

	// fields set dynamically
	public GameObject poi; // The point of interest
	float camZ; // The desired Z pos of the camera
	Vector3 launchPos;
	
	void Awake() {
		Instance = this;
		camZ = transform.position.z;
		launchPos = Slingshot.Instance.launchPoint.transform.position;
	}
	
	void FixedUpdate () {
		// Get the position of the poi
		Vector3 destination;

		if (poi == null) {
			destination = Vector3.zero;
		} else {
			// Get the position of the poi
			destination = poi.transform.position;

			// If poi is a Projectile, check to see if it's at rest
			if (poi.tag == "Projectile") {
				// if it is sleeping (that is, not moving)
				if ( poi.rigidbody.IsSleeping() && destination.x > launchPos.x ) {
					// return to default view
					poi = null;
					// in the next update
					return;
				}
			}
		}

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