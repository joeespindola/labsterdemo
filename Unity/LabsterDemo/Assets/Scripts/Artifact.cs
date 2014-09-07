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

	void Start() {
		artifactAnimator = GetComponent<Animator>();
	}

	// SET ANIMATOR BOOL
	public void ArtifactCollected() {
		artifactAnimator.SetBool ("collected_artifact", true);
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

