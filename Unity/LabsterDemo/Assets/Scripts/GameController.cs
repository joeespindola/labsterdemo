﻿using UnityEngine;
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
	ArtifactGreenKey
}

// TAGS
public enum TagObject {
	TagNull,
	TagPlayer,
	TagGround,
	TagWall,
	TagDoor,
	TagArtifact
}

public class GameController : MonoBehaviour {
	public PlayerController player;
	public InputController input;
	public LevelController level;
	public InventoryController inventory;

	private TagObject lastSelectedObjectTag;
	private Vector3 lastSelectedWorldPos;

	private List<ArtifactObject> playerArtifactObjectList;

	void Start () {

		playerArtifactObjectList = new List<ArtifactObject>();

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
				Vector3 hitPoint = hit.point;
				hitPoint.y = 0f;

				player.SetTargetPosition(hitPoint);
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
				ArtifactObject artifactType = artifact.GetArtifactObject();

				// SET ARTIFACT TO INVENTORY
				playerArtifactObjectList.Add(artifactType);

				// SEND MESSAGE TO COLLECTED ARTIFACT.
				artifact.ArtifactCollected(gameObject);
			}
		}

		// PLAYER COLLIDED WITH DOOR
		if(tagObject == TagObject.TagDoor) {
			// GET DOOR OBJECT
			Door door = collider.gameObject.GetComponent<Door>();

			// CHECK IF PLAYER HAS ARTIFACT ON OBJECT LIST
			foreach(ArtifactObject artifactObject in playerArtifactObjectList) {

				if(door.GetNeededArtifact() == artifactObject) {
					// PLAYER HAS DOOR ARTIFACT

					door.OpenDoor();
				}

			}
		}
	}

	// ADD ARTIFACT DO PLAYER INVENTORY (CALLED AFTER ANIMATION ENDED)
	public void AddArtifact(object artifact) {
		ArtifactObject artifactObject = (ArtifactObject)artifact;

		inventory.AddArtifact(artifactObject);
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
