using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class InventoryController : MonoBehaviour {

	public Texture inventoryBgTexture;

	public Texture inventoryRedKeyTexture;
	public Texture inventoryGreenKeyTexture;
	public Texture inventoryBlueKeyTexture;

	private List<Artifact> artifactIventoryList;

	void Start() {
		artifactIventoryList = new List<Artifact>();
	}

	public void AddArtifact(Artifact artifact) {
		artifactIventoryList.Add(artifact);
	}

	public bool HasArtifactObject(ArtifactObject artifactObject) {
		foreach(Artifact obj in artifactIventoryList) {
			if(obj.GetArtifactObject() == artifactObject) {
				// HAS ARTIFACT
				return true;
			}
		}

		return false;
	}

	void OnGUI() {
		GUI.DrawTexture(new Rect(10, 10, 236, 90), inventoryBgTexture);

		Vector2 offsetInventory = new Vector2(22, 48);

		float xDist = 57.0f;
		float xPos = 0.0f;

		foreach(Artifact obj in artifactIventoryList) {
			if(obj.GetArtifactObject() == ArtifactObject.ArtifactRedKey) {
				GUI.DrawTexture(new Rect(xPos+offsetInventory.x, offsetInventory.y, 38, 38), inventoryRedKeyTexture);
			}
			if(obj.GetArtifactObject() == ArtifactObject.ArtifactGreenKey) {
				GUI.DrawTexture(new Rect(xPos+offsetInventory.x, offsetInventory.y, 38, 38), inventoryGreenKeyTexture);
			}
			if(obj.GetArtifactObject() == ArtifactObject.ArtifactBlueKey) {
				GUI.DrawTexture(new Rect(xPos+offsetInventory.x, offsetInventory.y, 38, 38), inventoryBlueKeyTexture);
			}

			xPos += xDist;
		}
	}
}
