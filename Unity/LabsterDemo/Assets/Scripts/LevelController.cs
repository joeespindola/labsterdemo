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

	void Start () {

	}

	// LOAD LEVEL XML
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

}
