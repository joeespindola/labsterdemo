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

		// LOAD THE LEVEL
		level.LoadLevel();

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

		// WHITE KEYS RESETS THE GAME
		if(artifactObject.GetArtifactObject() == ArtifactObject.ArtifactWhiteKey) {
			level.RestartLevel();
		}
	}

	// PLAYER GUI INTERACTION
	public void OnGUI() {
		if (GUI.Button(new Rect(10, 20, 110, 30), "Save Game")) {
			level.SaveGame();
		}

		else if (GUI.Button(new Rect(10, 55, 110, 30), "Load Game")) {
			level.LoadGame();
		}

		else if (GUI.Button(new Rect(10, 91, 110, 30), "Restart Game")) {
			level.RestartLevel();
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
}
