using UnityEngine;
using System.Collections;
using System.Collections.Generic;

/*
* 	GAME CONTROLLER IS RESPONSIBLE FOR
* 
*	- UPDATING CONTROLLERS IN ORDER
* 	- SET PLAYER MOVEMENT BASED ON INPUT
* 	- HANDLE COLLISIONS
* 	- IMPLEMENT GAME LOGIC
*	- INTERFACE INTERACTION
*	- SET GLOBAL ENUMS
*/

// MAKE ENUMS ACCESSIBLE THROUGHOUT CODE
// ARTIFACT OBJECTS
public enum ArtifactObject {
	ArtifactNull,
	ArtifactRedKey,
	ArtifactBlueKey,
	ArtifactGreenKey,
	ArtifactWhiteKey
}

// TAGS
public enum TagObject {
	TagNull,
	TagPlayer,
	TagGround,
	TagWall,
	TagDoor,
	TagArtifact,
	TagBlock
}

// LEVEL DIFICULTY
public enum LevelDificulty {
	Null,
	Easy,
	Medium,
	Hard
}

public class GameController : MonoBehaviour {
	public PlayerController player;
	public InputController input;
	public LevelController level;
	public InventoryController inventory;

	public Camera camera;

	private TagObject lastSelectedObjectTag;
	private Vector3 lastSelectedWorldPos;

	private bool guiInteraction = false;
	
	void Start () {

		// LOAD THE LEVEL EASY
		level.LoadLevel(LevelDificulty.Easy);

	}
	
	void Update () {

		// UPDATE INPUT
		input.Tick();

		// CHECK IF PLAYER HAS INPUT AND CHECK ACTION
		CheckForActionInput();

		// UPDATE PLAYER
		player.Tick();

	}

	private void CheckForActionInput() {
		Vector3 lastInput = input.GetLastInputPosition();

		// HAS PLAYER INPUT EXECUTE ACTION
		if(lastInput != Vector3.zero) {

			// RAYCAST FORM CAMERA
			Ray ray = Camera.main.ScreenPointToRay(lastInput);
			RaycastHit hit;

			if(Input.GetMouseButtonDown (0) && GUIUtility.hotControl == 0) {
				// RAYCAST TO POSITION
				Physics.Raycast(Camera.main.transform.position, ray.direction, out hit, 2000);

				Vector3 hitPoint = hit.point;
				hitPoint.y = 0f;

				// SET PLAYER NEW TARGET TO MOVE
				player.SetTargetPosition(hitPoint);
			}

		}
	}



	public void PlayerHasCollisions(ControllerColliderHit collider) {
		string tag = collider.gameObject.tag;

		// IGNORE GROUND
		if(GetTagObjectFromString(tag) == TagObject.TagGround) {
			return;
		}

		TagObject tagObject = GetTagObjectFromString(tag);

		// STOP PLAYER
		player.SetTargetPosition(Vector3.zero);

		// PLAYER COLLIDED WITH ARTIFACT
		if(tagObject == TagObject.TagArtifact) {
			// GET ARTIFACT OBJECT
			Artifact artifact = collider.gameObject.GetComponent<Artifact>();

			// MAKE SURE ARTIFACT IS COLLECTED ONLY ONCE
			if(artifact.IsArtifactCollected() == false) {

				// SEND MESSAGE TO COLLECTED ARTIFACT.
				artifact.ArtifactCollected();

			}
		}

		// PLAYER COLLIDED WITH DOOR
		if(tagObject == TagObject.TagDoor) {
			// GET DOOR OBJECT
			Door door = collider.gameObject.GetComponent<Door>();

			// CHECK IF PLAYER HAS ARTIFACT ON OBJECT LIST
			if(inventory.HasArtifactObject(door.GetNeededArtifact())) {
				// PLAYER HAS DOOR ARTIFACT
				door.OpenDoor();
			}

		}
	}

	// ADD ARTIFACT DO PLAYER INVENTORY (CALLED AFTER ANIMATION ENDED)
	public void AddArtifact(object artifact) {
		Artifact artifactObject = (Artifact)artifact;

		inventory.AddArtifact(artifactObject);

		// WHITE KEYS CYCLES THE LEVELS
		if(artifactObject.GetArtifactObject() == ArtifactObject.ArtifactWhiteKey) {

			if(level.GetCurrentLevelDificulty() == LevelDificulty.Hard) {
				// LOAD EASY LEVEL
				level.LoadLevel(LevelDificulty.Easy);
			}
			else if(level.GetCurrentLevelDificulty() == LevelDificulty.Easy) {
				// LOAD MEDIUM LEVEL
				level.LoadLevel(LevelDificulty.Medium);
			}
			else if(level.GetCurrentLevelDificulty() == LevelDificulty.Medium) {
				// LOAD HARD LEVEL
				level.LoadLevel(LevelDificulty.Hard);
			}

		}
	}

	// PLAYER GUI INTERACTION
	public void OnGUI() {

		// NO SAVING AND LOADING FROM WEBPLAYER
		// CANNOT WRITE/READ FILES
		if( Application.platform != RuntimePlatform.OSXWebPlayer && Application.platform != RuntimePlatform.WindowsWebPlayer ) {

			if (GUI.Button(new Rect(10, 20, 110, 30), "Save Game")) {
				level.SaveGame();
			}

			else if (GUI.Button(new Rect(10, 55, 110, 30), "Load Game")) {
				level.StartCoroutine("LoadGame");
			}

		}

		if (GUI.Button(new Rect(10, 91, 110, 30), "Restart Level")) {
			level.RestartLevel();
		}

		GUI.Label (new Rect(20, 130, 110, 30), "Load level");

		if (GUI.Button(new Rect(10, 160, 50, 30), "Easy")) {
			level.LoadLevel(LevelDificulty.Easy);
		}

		else if (GUI.Button(new Rect(70, 160, 50, 30), "Medium")) {
			level.LoadLevel(LevelDificulty.Medium);
		}

		else if (GUI.Button(new Rect(130, 160, 50, 30), "Hard")) {
			level.LoadLevel(LevelDificulty.Hard);
		}

		// NO QUITTING APPLICATION ON WEBPLAYERS
		if( Application.platform != RuntimePlatform.OSXWebPlayer && Application.platform != RuntimePlatform.WindowsWebPlayer ) {

			if (GUI.Button(new Rect(10, 200, 120, 30), "Quit Apllication")) {
				Application.Quit();
			}

		}

	}

	// CONVERT STRING TO TAG OBJECT
	public static TagObject GetTagObjectFromString(string tag) {
		TagObject tagObject= TagObject.TagNull;
		
		switch(tag) {
		case "TagPlayer":
			tagObject = TagObject.TagPlayer;
			break;
		case "TagGround":
			tagObject = TagObject.TagGround;
			break;
		case "TagWall":
			tagObject = TagObject.TagWall;
			break;
		case "TagArtifact":
			tagObject = TagObject.TagArtifact;
			break;
		case "TagDoor":
			tagObject = TagObject.TagDoor;
			break;
		case "TagBlock":
			tagObject = TagObject.TagBlock;
			break;
		}
		
		return tagObject;
	}

	// RETURN STRONG FROM TAG OBJECT
	public static string GetTagStringFromObject(TagObject tag) {
		string tagObject = "TagNull";

		switch(tag) {
		case TagObject.TagPlayer:
			tagObject = "TagPlayer";
			break;
		case TagObject.TagWall:
			tagObject = "TagWall";
			break;
		case TagObject.TagGround:
			tagObject = "TagGround";
			break;
		case TagObject.TagDoor:
			tagObject = "TagDoor";
			break;
		case TagObject.TagArtifact:
			tagObject = "TagArtifact";
			break;
		case TagObject.TagBlock:
			tagObject = "TagBlock";
			break;
		}
		
		return tagObject;
	}

	// CONVERT STRING TO DIFICULTY
	public static LevelDificulty GetLevelDificultyFromString(string dificulty) {
		LevelDificulty levelDificulty = LevelDificulty.Null;
		
		switch(dificulty) {
		case "Easy":
			levelDificulty = LevelDificulty.Easy;
			break;
		case "Medium":
			levelDificulty = LevelDificulty.Medium;
			break;
		case "Hard":
			levelDificulty = LevelDificulty.Hard;
			break;
		}
		
		return levelDificulty;
	}
	
	// RETURN STRONG FROM TAG OBJECT
	public static string GetDificultyStringFromLevelDificulty(LevelDificulty dificulty) {
		string dificultyString = "Null";
		
		switch(dificulty) {
		case LevelDificulty.Easy:
			dificultyString = "Easy";
			break;
		case LevelDificulty.Medium:
			dificultyString = "Medium";
			break;
		case LevelDificulty.Hard:
			dificultyString = "Hard";
			break;
		}
		
		return dificultyString;
	}
}
