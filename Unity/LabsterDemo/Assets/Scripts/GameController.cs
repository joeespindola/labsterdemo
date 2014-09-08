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
*/
public class GameController : MonoBehaviour {
	public PlayerController player;
	public InputController input;
	public LevelController level;

	public enum TagObject {
		TagNull,
		TagPlayer,
		TagGround,
		TagWall,
		TagDoor,
		TagArtifact
	}

	private TagObject lastSelectedObjectTag;
	private Vector3 lastSelectedWorldPos;

	private List<Artifact.ArtifactObject> playerArtifactObjectList;

	void Start () {

		playerArtifactObjectList = new List<Artifact.ArtifactObject>();

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

			// RAYCAST TO POSITION
			Physics.Raycast(Camera.main.transform.position, ray.direction, out hit, 1000);

			// GET TAG
			string hitTag = hit.collider.gameObject.tag;
			lastSelectedObjectTag = GameController.GetTagObjectFromString(hitTag);

			// SET PLAYER POSITION UNLESS CLICKED ON A WALL
			if(lastSelectedObjectTag != TagObject.TagWall) {
				player.SetTargetPosition(hit.point);
			}

		}
	}

	public void PlayerHasCollisions(Collider collider) {
		string tag = collider.gameObject.tag;

		TagObject tagObject = GetTagObjectFromString(tag);

		// STOP PLAYER
		player.SetTargetPosition(Vector3.zero);

		// PLAYER COLLIDED WITH ARTIFACT
		if(tagObject == TagObject.TagArtifact) {
			// GET ARTIFACT OBJECT
			Artifact artifact = collider.gameObject.GetComponent<Artifact>();

			// MAKE SURE ARTIFACT IS COLLECTED ONLY ONCE
			if(artifact.IsArtifactCollected() == false) {
				// GET ARTIFACT TYPE
				Artifact.ArtifactObject artifactType = artifact.GetArtifactObject();

				// SET ARTIFACT TO INVENTORY
				playerArtifactObjectList.Add(artifactType);

				// SEND MESSAGE TO COLLECTED ARTIFACT.
				artifact.ArtifactCollected();
			}
		}

		// PLAYER COLLIDED WITH DOOR
		if(tagObject == TagObject.TagDoor) {
			// GET DOOR OBJECT
			Door door = collider.gameObject.GetComponent<Door>();

			Debug.Log ("Door collide "+door);

			// CHECK IF PLAYER HAS ARTIFACT ON OBJECT LIST
			foreach(Artifact.ArtifactObject artifactObject in playerArtifactObjectList) {

				if(door.GetNeededArtifact() == artifactObject) {
					// PLAYER HAS DOOR ARTIFACT

					door.OpenDoor();
					//Debug.Log ("OPEN DOOR");

				}

			}

			// GET DOOR TYPE
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
		}
		
		return tagObject;
	}
}
