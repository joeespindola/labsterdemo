using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class InventoryController : MonoBehaviour {

	public Texture inventoryBgTexture;

	private List<ArtifactObject> artifactIventoryList;

	void Start() {
		artifactIventoryList = new List<ArtifactObject>();
	}

	public void AddArtifact(ArtifactObject artifact) {
		artifactIventoryList.Add(artifact);
	}


	void OnGUI() {
		GUI.DrawTexture(new Rect(10, 10, 236, 90), inventoryBgTexture);
	}
}
