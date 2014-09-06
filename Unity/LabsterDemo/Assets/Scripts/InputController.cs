using UnityEngine;
using System.Collections;

// INPUT CLASS GETS THE CURRENT MOUSE OR TOUCH EVENTS AND RETURNS A VECTOR3 POSITION
public class InputController : MonoBehaviour {

	private Vector3 lastInputPosition;

	void Start () {

	}
	
	public void Tick() {
		GetMouseInput();
	}

	public Vector3 GetLastInputPosition() {
		return lastInputPosition;
	}

	private void GetMouseInput() {
		if (Input.GetMouseButtonDown(0)) {
			lastInputPosition = Input.mousePosition;
		}
		else {
			lastInputPosition = Vector3.zero;
		}
	}

	private void GetTouchInput() {

	}

}
