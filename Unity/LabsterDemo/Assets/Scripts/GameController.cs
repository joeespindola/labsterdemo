using UnityEngine;
using System.Collections;

/*
* GAME CONTROLLER IS RESPONSIBLE FOR
*	- UPDATING CONTROLLERS
* 	- SETTING PLAYER MOVEMENT TARGETS BASED ON RAYCAST FROM INPUT
*	- INTERFACE INTERACTION
*/

public class GameController : MonoBehaviour {
	public PlayerController player;
	public InputController input;

	private TagObject lastSelectedObjectTag;
	private Vector3 lastSelectedWorldPos;

	public enum TagObject {
		TagNull,
		TagPlayer,
		TagGround,
		TagWall,
		TagObject
	}
	

	void Start () {

	}
	
	void Update () {

		// UPDATE INPUT
		input.Tick();

		// CHECK IF PLAYER HAS ACTION
		CheckForAction();

		// UPDATE PLAYER
		player.Tick();

	}

	private void CheckForAction() {
		Vector3 lastInput = input.GetLastInputPosition();

		if(lastInput != Vector3.zero) {

			Ray ray = Camera.main.ScreenPointToRay(lastInput);
			RaycastHit hit;

			Physics.Raycast(Camera.main.transform.position, ray.direction, out hit, 1000);

			string hitTag = hit.collider.gameObject.tag;
			lastSelectedObjectTag = GameController.ReturnTag(hitTag);

			// SET PLAYER POSITION OF CLICKED ON GROUND
			if(lastSelectedObjectTag == TagObject.TagGround) {
				player.SetTargetPosition(hit.point);
			}

		}
	}

	public static TagObject ReturnTag(string tag) {
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
		case "TagObject":
			tagObject = TagObject.TagPlayer;
			break;
		}
		
		return tagObject;
	}
}
