using UnityEngine;
using System.Collections;

public class Artifact : MonoBehaviour
{
	// ARTIFACT OBJECTS
	public enum ArtifactObject {
		ArtifactNull,
		ArtifactRedKey,
		ArtifactBlueKey,
		ArtifactGreenKey
	}

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
	public void ArtifactCollected() {
		isArtifactColleted = true;
		artifactAnimator.SetBool ("artifact_collected", true);
	}

	// ANIMATION CALLBACK
	public void DestroyArtifact() {
		DestroyObject(gameObject);
	}

	public void SetArtifactObject(ArtifactObject artifact) {
		artifactObject = artifact;
	}

	public ArtifactObject GetArtifactObject() {
		return artifactObject;
	}
}

