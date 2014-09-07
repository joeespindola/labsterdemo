using UnityEngine;
using System.Collections;

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
		TagArtifact
	}
	

	private TagObject lastSelectedObjectTag;
	private Vector3 lastSelectedWorldPos;

	void Start () {

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

			Debug.Log ("tag");

			// GET ARTIFACT TYPE
			Artifact.ArtifactObject artifactType = artifact.GetArtifactObject();

			// SET ARTIFACT TO INVENTORY


			// SEND MESSAGE TO COLLECTED ARTIFACT.
			artifact.ArtifactCollected();
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
		}
		
		return tagObject;
	}
}
