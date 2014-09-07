using UnityEngine;
using System.Collections;

public class Artifact : MonoBehaviour
{
	public enum ArtifactObject {
		ArtifactNull,
		ArtifactRedKey,
		ArtifactBlueKey,
		ArtifactGreenKey
	}

	public ArtifactObject artifact;

	public Animator artifactAnimator;

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

	public ArtifactObject GetArtifactObject() {
		return artifact;
	}
}

