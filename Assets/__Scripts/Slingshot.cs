using UnityEngine;
using System.Collections;

public class Slingshot : MonoBehaviour {
	public GameObject prefabProjectile;
	public float velocityMult = 4f;
	static public Slingshot Instance;

	public GameObject launchPoint;
	Vector3 launchPos;
	GameObject projectile;
	bool aimingMode;
	
	void Awake() {
		Instance = this;
		Transform launchPointTrans = transform.Find("LaunchPoint");
		launchPoint = launchPointTrans.gameObject;
		launchPoint.SetActive( false );
		launchPos = launchPointTrans.position;
	}
	
	void OnMouseEnter() {
		//print("Slingshot:OnMouseEnter()");
		launchPoint.SetActive( true );
	}
	
	void OnMouseExit() {
		//print("Slingshot:OnMouseExit()");
		launchPoint.SetActive( false );
	}	

	void OnMouseDown() {
		// The player has pressed the mouse button while over Slingshot
		aimingMode = true;
		// Instantiate a Projectile
		projectile = Instantiate( prefabProjectile ) as GameObject;
		// Start it at the launchPoint
		projectile.transform.position = launchPos;
		// Set it to isKinematic for now
		projectile.rigidbody.isKinematic = true;
	}

	void Update() {
		if (!aimingMode) return;

		Vector3 mousePos2D = Input.mousePosition;
		mousePos2D.z = -Camera.main.transform.position.z;

		Vector3 mousePos3D = Camera.main.ScreenToWorldPoint(mousePos2D);
		Vector3 mouseDelta = mousePos3D - launchPos;
		float maxMagnitude = this.GetComponent<SphereCollider>().radius;

		if(mouseDelta.magnitude > maxMagnitude) {
			mouseDelta.Normalize();
			mouseDelta *= maxMagnitude;
		}

		Vector3 projPos = launchPos + mouseDelta;

		if (Input.GetMouseButtonUp(0)) {
			aimingMode = false;
			projectile.rigidbody.isKinematic = false;
			projectile.rigidbody.velocity = -mouseDelta * velocityMult;
			FollowCam.Instance.poi = projectile;
			projectile = null;
		}
	}
}