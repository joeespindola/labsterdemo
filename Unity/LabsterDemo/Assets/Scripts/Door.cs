using UnityEngine;
using System.Collections;

public class Door : MonoBehaviour {

	public Artifact.ArtifactObject artifactNeeded;

	public void SetArtifactNeeded(Artifact.ArtifactObject artifact) {
		artifactNeeded = artifact;
	}

	public Artifact.ArtifactObject GetNeededArtifact() {
		return artifactNeeded;
	}

	public void OpenDoor() {
		DestroyObject(gameObject);
	}
}
