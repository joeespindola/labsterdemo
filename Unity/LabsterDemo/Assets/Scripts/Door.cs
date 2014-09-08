using UnityEngine;
using System.Collections;

/*
* 	DOOR IS RESPONSIBLE FOR
* 
*	- STORING DOOR TYPE
* 	- ANIMATING DOOR OBJECT
*/
public class Door : MonoBehaviour {
	public int id;

	public ArtifactObject artifactKeyNeeded;
	private Animator doorAnimator;

	private bool isOpened = false;
	private bool wrapDoor = false;

	void Start() {
		doorAnimator = GetComponent<Animator>();
	}

	public void SetArtifactNeeded(ArtifactObject artifact) {
		artifactKeyNeeded = artifact;
	}

	public ArtifactObject GetNeededArtifact() {
		return artifactKeyNeeded;
	}

	// RETURN IF DOOR IS OPENED
	public bool IsOpened() {
		return isOpened;
	}

	// OPEN DOOR
	public void OpenDoor() {
		isOpened = true;
		doorAnimator.SetBool("door_opened", isOpened);
		doorAnimator.SetBool("wrap_door", wrapDoor);
	}

	// WARP DOOR
	public void WarpDoor() {
		wrapDoor = true;
	}
}
