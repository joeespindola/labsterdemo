using UnityEngine;
using System.Collections;

/*
* 	LEVEL CONTROLLER IS RESPONSIBLE FOR
* 
*	- LOADING LEVELS FROM XML
* 	- CREATING GAME OBJECTS LOADED LEVELS
* 	- SAVING AND LOADING GAME FILES
*/
public class LevelController : MonoBehaviour {
	public GameObject levelRootObject;

	void Start () {

	}

	public void LoadLevel() {
		LoadArtifacts();
	}

	private void LoadArtifacts() {
		CreateKeyArtifact(levelRootObject.transform, new Vector3(5f, 0.5f, 10f), Color.red, Artifact.ArtifactObject.ArtifactRedKey);
		CreateKeyArtifact(levelRootObject.transform, new Vector3(0f, 0.5f, 10f), Color.green, Artifact.ArtifactObject.ArtifactGreenKey);
		CreateKeyArtifact(levelRootObject.transform, new Vector3(-5f, 0.5f, 15f), Color.blue, Artifact.ArtifactObject.ArtifactBlueKey);
	}

	private void CreateKeyArtifact(Transform parent, Vector3 position, Color color, Artifact.ArtifactObject artifactObject) {
		GameObject keyArtifact = (GameObject)Instantiate(Resources.Load("ArtifactKey"));
		GameObject keyObject = keyArtifact.transform.GetChild(0).gameObject;

		// PARENT AND POSITION OBJECT
		keyArtifact.transform.parent = parent;
		keyArtifact.transform.position = position;

		// SET ARTIFACT OBJECT
		Artifact artifact = keyObject.GetComponent<Artifact>();
		artifact.SetArtifactObject(artifactObject);

		// SET MATERIAL
		keyObject.renderer.material.color = color;
	}

}
