using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class InventoryController : MonoBehaviour {

	public Texture inventoryBgTexture;

	public Texture inventoryRedKeyTexture;
	public Texture inventoryGreenKeyTexture;
	public Texture inventoryBlueKeyTexture;
	public Texture inventoryWhiteKeyTexture;

	private List<Artifact> artifactIventoryList = new List<Artifact>();

	void Start() {

	}

	public void AddArtifact(Artifact artifact) {
		artifactIventoryList.Add(artifact);
	}

	public bool HasArtifactObject(ArtifactObject artifactObject) {
		foreach(Artifact obj in artifactIventoryList) {

			// HAS ARTIFACT
			if(obj.GetArtifactObject() == artifactObject) {
				return true;
			}

		}

		return false;
	}

	public void ClearInventory() {
		artifactIventoryList.Clear();
	}

	void OnGUI() {
		Vector2 inventoryOffsetPosition =  new Vector2(Screen.width-(250), Screen.height-(105));

		GUI.DrawTexture(new Rect(10+inventoryOffsetPosition.x, 10+inventoryOffsetPosition.y, 236, 90), inventoryBgTexture);

		Vector2 offsetInventory = new Vector2(22+inventoryOffsetPosition.x, 48+inventoryOffsetPosition.y);

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
			if(obj.GetArtifactObject() == ArtifactObject.ArtifactWhiteKey) {
				GUI.DrawTexture(new Rect(xPos+offsetInventory.x, offsetInventory.y, 38, 38), inventoryWhiteKeyTexture);
			}

			xPos += xDist;
		}
	}
}
