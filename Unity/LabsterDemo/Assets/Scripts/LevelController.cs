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
	public GameObject levelRootObject;
	public TextAsset LevelAsset;

	List<Dictionary<string,string>> levelObjects = new List<Dictionary<string,string>>();
	Dictionary<string,string> obj;

	void Start () {

	}

	public void LoadLevel() {

		XmlDocument xmlDoc = new XmlDocument(); 
		xmlDoc.LoadXml(LevelAsset.text);

		XmlNodeList levelsList = xmlDoc.GetElementsByTagName("level");

		foreach (XmlNode levelInfo in levelsList) {
			XmlNodeList levelNodes = levelInfo.ChildNodes;

			foreach (XmlNode levelNode in levelNodes) // levels itens nodes.
			{
				GameObject obj = (GameObject)Instantiate(Resources.Load(levelNode.Name));

				XmlNodeList transformcontent = levelNode.ChildNodes;

				// SET TAGS AND SPECIFIC GAME ATTRIBUTES
				if(levelNode.Name == "ground")
				{
					obj.tag = GameController.GetTagStringFromObject(GameController.TagObject.TagGround);
				}
				if(levelNode.Name == "wall")
				{
					obj.tag = GameController.GetTagStringFromObject(GameController.TagObject.TagWall);
				}
				if(levelNode.Name == "door")
				{
					obj.tag = GameController.GetTagStringFromObject(GameController.TagObject.TagDoor);
					Door doorObj = obj.GetComponent<Door>();

					// SET DOOR TYPE
					foreach (XmlNode transformItens in transformcontent) {
						if(transformItens.Name == "type") {
							string typeValue = transformItens.InnerText;
							
							if(typeValue == "Red") {
								doorObj.SetArtifactNeeded(Artifact.ArtifactObject.ArtifactRedKey);
							}
							if(typeValue == "Green") {
								doorObj.SetArtifactNeeded(Artifact.ArtifactObject.ArtifactGreenKey);
							}
							if(typeValue == "Blue") {
								doorObj.SetArtifactNeeded(Artifact.ArtifactObject.ArtifactBlueKey);
							}
						}
					}
				}
				if(levelNode.Name == "artifact")
				{
					obj.tag = GameController.GetTagStringFromObject(GameController.TagObject.TagArtifact);
					Artifact artifactObj = obj.GetComponent<Artifact>();

					// SET ARTIFACT TYPE
					foreach (XmlNode transformItens in transformcontent) {
						if(transformItens.Name == "type") {
							string typeValue = transformItens.InnerText;

							if(typeValue == "Red") {
								artifactObj.SetArtifactObject(Artifact.ArtifactObject.ArtifactRedKey);
							}
							if(typeValue == "Green") {
								artifactObj.SetArtifactObject(Artifact.ArtifactObject.ArtifactGreenKey);
							}
							if(typeValue == "Blue") {
								artifactObj.SetArtifactObject(Artifact.ArtifactObject.ArtifactBlueKey);
							}
						}
					}

				}

				// SET OBJECT TRANSFORM VALUES FROM XML
				CreateTransformObject(transformcontent, obj);
				
				// ADD LEVEL OBJECTS INTO THE LEVEL OBJECTS LIST BASED ON NAME
				//levelObjects.Add(obj);

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

	/*
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
*/

}
