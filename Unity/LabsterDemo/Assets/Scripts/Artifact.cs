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

	void Start() {

	}

	public ArtifactObject GetArtifactObject() {
		return artifact;
	}
}

