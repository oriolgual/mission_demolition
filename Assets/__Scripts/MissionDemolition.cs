using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public enum GameMode {
	idle,
	playing,
	levelEnd
}

public class MissionDemolition : MonoBehaviour {
	static public MissionDemolition    Instance; // a Singleton
	
	// fields set in the Unity Inspector pane
	public GameObject[]       castles;   // An array of the castles
	public Text               levelText;   // The GT_Level GUIText
	public Text               scoreText;   // The GT_Score GUIText
	public Vector3            castlePos; // The place to put castles

	// fields set dynamically
	public int                level;     // The current level
	public int                levelMax;  // The number of levels
	public int                shotsTaken;
	public GameObject         castle;    // The current castle
	public GameMode           mode = GameMode.idle;
	public string             showing = "Slingshot"; // FollowCam mode
	
	void Start() {
		Instance = this; // Define the Singleton
		level = 0;
		levelMax = castles.Length;
		StartLevel();
	}
	
	void StartLevel() {
		// Get rid of the old castle if one exists
		if (castle != null) {
			Destroy( castle );
		}
		
		// Destroy old projectiles if they exist
		GameObject[] gos = GameObject.FindGameObjectsWithTag("Projectile");
		foreach (GameObject pTemp in gos) {
			Destroy( pTemp );
		}
		
		// Instantiate the new castle
		castle = Instantiate( castles[level] ) as GameObject;
		castle.transform.position = castlePos;
		shotsTaken = 0;
		
		// Reset the camera
		SwitchView("Both");
		ProjectileLine.Instance.Clear();
		
		// Reset the goal
		Goal.goalMet = false;
		
		ShowTexts();
		
		mode = GameMode.playing;
	}
	
	void ShowTexts() {
		// Show the data in the GUITexts
		levelText.text = "Level: "+(level+1)+" of "+levelMax;
		scoreText.text = "Shots Taken: "+shotsTaken;
	}

	void Update() {
		ShowTexts();
		
		// Check for level end
		if (mode == GameMode.playing && Goal.goalMet) {
			// Change mode to stop checking for level end
			mode = GameMode.levelEnd;
			// Zoom out
			SwitchView("Both");
			// Start the next level in 2 seconds
			Invoke("NextLevel", 2f);
		}
	}
	
	void NextLevel() {
		level++;
		if (level == levelMax) {
			level = 0;
		}
		StartLevel();
	}
	
	void OnGUI() {
		// Draw the GUI button for view switching at the top of the screen
		Rect buttonRect = new Rect( (Screen.width/2)-50, 10, 100, 24 );
		
		switch(showing) {
		case "Slingshot":
			if ( GUI.Button( buttonRect, "Show Castle" ) ) {
				SwitchView("Castle");
			}
			break;
			
		case "Castle":
			if ( GUI.Button( buttonRect, "Show Both" ) ) {
				SwitchView("Both");
			}
			break;
		case "Both":
			if ( GUI.Button( buttonRect, "Show Slingshot" ) ) {
				SwitchView( "Slingshot" );
			}
			break;
			
		}
	}
	
	// Static method that allows code anywhere to request a view change
	static public void SwitchView( string eView ) {
		Instance.showing = eView;
		switch (Instance.showing) {
		case "Slingshot":
			FollowCam.Instance.poi = null;
			break;
			
		case "Castle":
			FollowCam.Instance.poi = Instance.castle;
			break;
			
		case "Both":
			FollowCam.Instance.poi = GameObject.Find("ViewBoth");
			break;
			
		}
	}
	
	// Static method that allows code anywhere to increment shotsTaken
	public static void ShotFired() {
		Instance.shotsTaken++;
	}
	
}