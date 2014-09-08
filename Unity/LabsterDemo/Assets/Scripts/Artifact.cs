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

	public GameController gameController;

	public ArtifactObject artifactObject;
	private Animator artifactAnimator;

	private bool isArtifactColleted = false;

	public void Create() {
		artifactAnimator = GetComponent<Animator>();
		gameController = GameObject.FindGameObjectWithTag("TagGameController").GetComponent<GameController>();
	}

	public bool IsArtifactCollected() {
		return isArtifactColleted;
	}

	// SET ANIMATOR BOOL
	public void ArtifactCollected() {
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

	public void WarpArtifact() {
		if(artifactAnimator == null) {
			artifactAnimator = GetComponent<Animator>();
			gameController = GameObject.FindGameObjectWithTag("TagGameController").GetComponent<GameController>();

		}

		isArtifactColleted = true;

		artifactAnimator.SetBool ("warp_collected", true);

		// SEND MESSAGE TO GAME CONTROLLER TO COLLECT ARTIFACT
		ArtifactAnimationCallback();
	}
}

