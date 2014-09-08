using UnityEngine;
using System.Collections;

public class Door : MonoBehaviour {

	public Artifact.ArtifactObject artifactNeeded;
	private Animator doorAnimator;

	public void SetArtifactNeeded(Artifact.ArtifactObject artifact) {
		artifactNeeded = artifact;

		doorAnimator = GetComponent<Animator>();
	}

	public Artifact.ArtifactObject GetNeededArtifact() {
		return artifactNeeded;
	}

	public void OpenDoor() {
		doorAnimator.SetBool ("door_opened", true);
		Destroy(GetComponent<BoxCollider>());
	}
}
