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
					obj.tag = GameController.GetTagStringFromObject(TagObject.TagDoor);
					Door doorObj = obj.GetComponent<Door>();
					doorObj.id = doorIdCount;

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
					obj.tag = GameController.GetTagStringFromObject(TagObject.TagArtifact);
					Artifact artifactObj = obj.GetComponent<Artifact>();
					artifactObj.id = artifactIdCount;

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
				
				doorsElement.AppendChild(artifactElement);
			}
			
			elmRoot.AppendChild(doorsElement);

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
		string filepath = Application.dataPath + @"/Data/GameSave.xml";
		XmlDocument xmlDoc = new XmlDocument();
		
		// CHECK IF FILE EXISTS
		if(File.Exists(filepath))
		{
			xmlDoc.Load(filepath);
			
			XmlElement elmRoot = xmlDoc.DocumentElement;

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


		}

	}

}
