using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using System.Xml;
using System.IO;

/*
* 	LEVEL CONTROLLER IS RESPONSIBLE FOR
* 
*	- LOADING LEVELS FROM XML
* 	- CREATING GAME OBJECTS LOADED LEVELS
* 	- SAVING AND LOADING GAME FILES
*/
public class LevelController : MonoBehaviour {
	public GameController gameController;

	public List<Door> gameLevelDoorList;
	public List<Artifact> gameLevelArtifactList;

	public GameObject levelRootObject;
	public TextAsset LevelAsset;

	void Start () {


	}

	// LOAD LEVEL XML
	public void LoadLevel() {

		XmlDocument xmlDoc = new XmlDocument(); 
		xmlDoc.LoadXml(LevelAsset.text);

		XmlNodeList levelsList = xmlDoc.GetElementsByTagName("level");

		// CREATE DOOR LIST
		gameLevelDoorList = new List<Door>();
		
		// CREATE ARTIFACT LIST
		gameLevelArtifactList = new List<Artifact>();

		// RESET IDS
		int artifactIdCount = 0;
		int doorIdCount = 0;

		foreach (XmlNode levelInfo in levelsList) {
			XmlNodeList levelNodes = levelInfo.ChildNodes;

			foreach (XmlNode levelNode in levelNodes) // levels itens nodes.
			{
				GameObject obj = (GameObject)Instantiate(Resources.Load(levelNode.Name));

				XmlNodeList transformcontent = levelNode.ChildNodes;

				// SET TAGS AND SPECIFIC GAME ATTRIBUTES
				if(levelNode.Name == "ground")
				{
					obj.tag = GameController.GetTagStringFromObject(TagObject.TagGround);
				}
				if(levelNode.Name == "wall")
				{
					obj.tag = GameController.GetTagStringFromObject(TagObject.TagWall);
				}
				if(levelNode.Name == "door")
				{
					// CREATE DOOR OBJECT
					obj.tag = GameController.GetTagStringFromObject(TagObject.TagDoor);
					Door doorObj = obj.GetComponent<Door>();
					doorObj.id = doorIdCount;
					doorObj.Create();

					gameLevelDoorList.Add(doorObj);

					doorIdCount++;	

					// SET DOOR TYPE
					foreach (XmlNode transformItens in transformcontent) {
						if(transformItens.Name == "type") {
							string typeValue = transformItens.InnerText;
							
							if(typeValue == "Red") {
								doorObj.SetArtifactNeeded(ArtifactObject.ArtifactRedKey);
								obj.transform.GetChild(0).gameObject.renderer.material.color = Color.red;
							}
							if(typeValue == "Green") {
								doorObj.SetArtifactNeeded(ArtifactObject.ArtifactGreenKey);
								obj.transform.GetChild(0).gameObject.renderer.material.color = Color.green;
							}
							if(typeValue == "Blue") {
								doorObj.SetArtifactNeeded(ArtifactObject.ArtifactBlueKey);
								obj.transform.GetChild(0).gameObject.renderer.material.color = Color.blue;
							}
						}
					}
				}
				if(levelNode.Name == "artifact")
				{
					// CREATE ARTIFACT OBJECT
					obj.tag = GameController.GetTagStringFromObject(TagObject.TagArtifact);
					Artifact artifactObj = obj.GetComponent<Artifact>();
					artifactObj.id = artifactIdCount;
					artifactObj.Create();

					gameLevelArtifactList.Add(artifactObj);

					artifactIdCount++;

					// SET ARTIFACT TYPE
					foreach (XmlNode transformItens in transformcontent) {
						if(transformItens.Name == "type") {
							string typeValue = transformItens.InnerText;

							if(typeValue == "Red") {
								artifactObj.SetArtifactObject(ArtifactObject.ArtifactRedKey);
								artifactObj.transform.GetChild(0).gameObject.renderer.material.color = Color.red;
							}
							if(typeValue == "Green") {
								artifactObj.SetArtifactObject(ArtifactObject.ArtifactGreenKey);
								artifactObj.transform.GetChild(0).gameObject.renderer.material.color = Color.green;
							}
							if(typeValue == "Blue") {
								artifactObj.SetArtifactObject(ArtifactObject.ArtifactBlueKey);
								artifactObj.transform.GetChild(0).gameObject.renderer.material.color = Color.blue;
							}
						}
					}

				}

				// SET OBJECT TRANSFORM VALUES FROM XML
				CreateTransformObject(transformcontent, obj);

			}

		}

	}

	// HELPER FUNCTION TO CONVERT STRING TO VECTORS
	private Vector3 DeserializeVectorString(string vectorString) {
		string[] vectors = vectorString	.Split(',');
		
		float x = float.Parse(vectors[0]);
		float y = float.Parse(vectors[1]);
		float z = float.Parse(vectors[2]);
		
		return new Vector3(x,y,z);
	}

	// SET TRANSFORMATIONS ON OBJECTS LOADED FROM XML
	private void CreateTransformObject(XmlNodeList transformcontent, GameObject obj) {
		Vector3 objScale = Vector3.zero;
		Vector3 objPosition = Vector3.zero;
		Vector3 objRotation = Vector3.zero;
		
		foreach (XmlNode transformItens in transformcontent) {
			if(transformItens.Name == "scale") {
				string vectorValue = transformItens.InnerText;
				objScale = DeserializeVectorString(vectorValue);
			}
			
			if(transformItens.Name == "position") {
				string vectorValue = transformItens.InnerText;
				objPosition = DeserializeVectorString(vectorValue);
			}
			
			if(transformItens.Name == "rotation") {
				string vectorValue = transformItens.InnerText;
				objRotation = DeserializeVectorString(vectorValue);
			}
		}

		// PARENT AND POSITION, SCALE, ROTATE OBJECT
		obj.transform.parent = levelRootObject.transform;
		obj.transform.position = objPosition;
		
		Quaternion groundRotationQuat = obj.transform.rotation;
		groundRotationQuat.eulerAngles = objRotation;
		obj.transform.rotation = groundRotationQuat;
		
		obj.transform.localScale = objScale;
	}

	private void ClearLevel() {
		// DESTROY ALL LEVEL CHILDRENS
		foreach (Transform child in levelRootObject.transform) {
			GameObject.Destroy(child.gameObject);
		}
	}

	// SAVE GAME
	public void SaveGame() {
		string filepath = Application.dataPath + @"/Data/GameSave.xml";
		XmlDocument xmlDoc = new XmlDocument();

		// CHECK IF FILE EXISTS
		if(File.Exists(filepath))
		{
			xmlDoc.Load(filepath);
			
			XmlElement elmRoot = xmlDoc.DocumentElement;
			
			elmRoot.RemoveAll();

			// PLAYER
			XmlElement playerElement = xmlDoc.CreateElement("player");

			// PLAYER POSITION
			XmlElement positionNode = xmlDoc.CreateElement("position");

			Vector3 playerPosition = gameController.player.transform.position;
			positionNode.InnerText = (System.Math.Round(playerPosition.x,3)+","+System.Math.Round(playerPosition.y,3)+","+System.Math.Round(playerPosition.z,3));
			
			playerElement.AppendChild(positionNode);

			// PLAYER ROTATION
			XmlElement rotationNode = xmlDoc.CreateElement("rotation");
			
			Vector3 rotationVector = gameController.player.transform.rotation.eulerAngles;
			rotationNode.InnerText = (System.Math.Round(rotationVector.x,3)+","+System.Math.Round(rotationVector.y,3)+","+System.Math.Round(rotationVector.z,3));
			
			playerElement.AppendChild(rotationNode);

			elmRoot.AppendChild(playerElement);

			// DOORS
			XmlElement doorsElement = xmlDoc.CreateElement("doors");

			foreach(Door door in gameLevelDoorList) {
				XmlElement doorElement = xmlDoc.CreateElement("door");
				doorElement.SetAttribute("id", door.id.ToString());

				doorElement.InnerText = (door.IsOpened().ToString());

				doorsElement.AppendChild(doorElement);
			}

			elmRoot.AppendChild(doorsElement);

			// ARTIFACTS
			XmlElement artifactsElement = xmlDoc.CreateElement("artifacts");
			
			foreach(Artifact artifact in gameLevelArtifactList) {
				XmlElement artifactElement = xmlDoc.CreateElement("artifact");
				artifactElement.SetAttribute("id", artifact.id.ToString());
				
				artifactElement.InnerText = (artifact.IsArtifactCollected().ToString());
				
				artifactsElement.AppendChild(artifactElement);
			}
			
			elmRoot.AppendChild(artifactsElement);

			// CAMERA
			XmlElement cameraElement = xmlDoc.CreateElement("camera");

			XmlElement cameraPositionElement = xmlDoc.CreateElement("position");
			Vector3 cameraPosition = gameController.camera.transform.position;

			cameraPositionElement.InnerText = (cameraPosition.x+","+cameraPosition.y+","+cameraPosition.z);

			XmlElement cameraRotationElement = xmlDoc.CreateElement("rotation");
			Vector3 cameraRotation = gameController.camera.transform.rotation.eulerAngles;

			cameraRotationElement.InnerText = (System.Math.Round(cameraRotation.x,3)+","+System.Math.Round(cameraRotation.y,3)+","+System.Math.Round(cameraRotation.z,3));

			cameraElement.AppendChild(cameraPositionElement);
			cameraElement.AppendChild(cameraRotationElement);
							
			elmRoot.AppendChild(cameraElement);

			xmlDoc.Save(filepath);
		}
	}

	// LOAD GAME
	public void LoadGame() {
		ClearLevel();

		string filepath = Application.dataPath + @"/Data/GameSave.xml";
		XmlDocument xmlDoc = new XmlDocument();

		
		// CHECK IF FILE EXISTS
		if(File.Exists(filepath))
		{
			xmlDoc.Load(filepath);

			// DESTROY LEVEL CHILDRENS
			ClearLevel();

			// LOAD LEVEL AGAIN
			LoadLevel();

			// CLEARS INVENTORY LIST
			gameController.inventory.ClearInventory();

			// SET PLAYER POSITION AND ROTATION
			XmlNode playerNode = xmlDoc.DocumentElement.GetElementsByTagName("player")[0];

			foreach(XmlNode playerInfoNode in playerNode.ChildNodes) {
				if(playerInfoNode.Name == "position") {
					string playerPositionString = playerInfoNode.InnerText;
					
					gameController.player.WarpTo( DeserializeVectorString(playerPositionString) );
				}
				
				if(playerInfoNode.Name == "rotation") {
					string playerRotationString = playerInfoNode.InnerText;
					
					Quaternion playerRotationQuat = gameController.player.transform.rotation;
					playerRotationQuat.eulerAngles = DeserializeVectorString(playerRotationString);
					
					gameController.player.transform.rotation = playerRotationQuat;
				}
			}

			// SET CAMERA POSITION AND ROTATION
			XmlNode cameraNode = xmlDoc.DocumentElement.GetElementsByTagName("camera")[0];

			foreach(XmlNode camInfoNode in cameraNode.ChildNodes) {
				if(camInfoNode.Name == "position") {
					string cameraPositionString = camInfoNode.InnerText;

					gameController.camera.transform.position = DeserializeVectorString(cameraPositionString);
				}

				if(camInfoNode.Name == "rotation") {
					string cameraRotationString = camInfoNode.InnerText;

					Quaternion cameraRotationQuat = gameController.camera.transform.rotation;
					cameraRotationQuat.eulerAngles = DeserializeVectorString(cameraRotationString);

					gameController.camera.transform.rotation = cameraRotationQuat;
				}
			}

			// SET DOOR STATES

			XmlNode doorsNode = xmlDoc.DocumentElement.GetElementsByTagName("doors")[0];
			
			foreach(XmlNode doorInfoNode in doorsNode.ChildNodes) {
				if(doorInfoNode.Name == "door") {
					string doorStateValue = doorInfoNode.InnerText;

					int id = int.Parse( doorInfoNode.Attributes["id"].Value );

					// CHECK IF DOOR ID IS THE SAME
					foreach(Door door in gameLevelDoorList) {
						if(door.id == id && doorStateValue == "True") {
							// WARP DOOR
							door.WarpDoor();
						}
					}
				}

			}


			// SET ARTIFACTS STATES
			XmlNode artifactsNode = xmlDoc.DocumentElement.GetElementsByTagName("artifacts")[0];
			
			foreach(XmlNode artifactInfoNode in artifactsNode.ChildNodes) {
				if(artifactInfoNode.Name == "artifact") {
					string artifactStateValue = artifactInfoNode.InnerText;
					
					int id = int.Parse( artifactInfoNode.Attributes["id"].Value );
				
					// CHECK IF DOOR ID IS THE SAME
					foreach(Artifact artifact in gameLevelArtifactList) {
						if(artifact.id == id && artifactStateValue == "True") {
							// WARP ARTIFACT
							artifact.WarpArtifact();
						}
					}
				}
				
			}


		}

	}

}
