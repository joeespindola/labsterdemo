using UnityEngine;
using System.Collections;

/*
* 	ARTIFACT IS RESPONSIBLE FOR
* 
*	- STORING ARTIFACT TYPE
* 	- ANIMATING ARTIFACT OBJECT
*/
public class Artifact : MonoBehaviour {
	public int id;

	private GameObject gameController;

	public ArtifactObject artifactObject;
	private Animator artifactAnimator;

	private bool isArtifactColleted = false;

	void Start() {
		artifactAnimator = GetComponent<Animator>();
	}

	public bool IsArtifactCollected() {
		return isArtifactColleted;
	}

	// SET ANIMATOR BOOL
	public void ArtifactCollected(GameObject controller) {
		// STORE GAME CONTROLLER TO SEND MESSAGE LATTER
		gameController = controller;

		artifactAnimator.SetBool ("artifact_collected", true);

		isArtifactColleted = true;

	}

	// ANIMATION CALLBACK
	public void ArtifactAnimationCallback() {
		// CALLBACK GAME CONTROLLER TO ADD ARTIFACT TO INVENTORY
		gameController.SendMessage("AddArtifact", this);

	}

	public void SetArtifactObject(ArtifactObject artifact) {
		artifactObject = artifact;
	}

	public ArtifactObject GetArtifactObject() {
		return artifactObject;
	}
}

