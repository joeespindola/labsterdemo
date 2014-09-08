using UnityEngine;
using System.Collections;

/*
* 	DOOR IS RESPONSIBLE FOR
* 
*	- STORING DOOR TYPE
* 	- ANIMATING DOOR OBJECT
*/
public class Door : MonoBehaviour {

	public ArtifactObject artifactKeyNeeded;
	private Animator doorAnimator;

	void Start() {
		doorAnimator = GetComponent<Animator>();
	}

	public void SetArtifactNeeded(ArtifactObject artifact) {
		artifactKeyNeeded = artifact;
	}

	public ArtifactObject GetNeededArtifact() {
		return artifactKeyNeeded;
	}

	// OPEN DOOR
	public void OpenDoor() {
		doorAnimator.SetBool ("door_opened", true);
	}
}
