using UnityEngine;
using System.Collections;

/*
* 	INPUT CONTROLLER IS RESPONSIBLE FOR
* 
*	- GETTING INPUT FROM EITHER MOUSE OR TOUCH EVENTS
*/
public class InputController : MonoBehaviour {

	private Vector3 lastInputPosition;

	void Start () {

	}
	
	public void Tick() {

		// GET MOUSE INPUT ON PC AND MAC
		GetMouseInput();

		// GET TOUCH INPUT ON MOBILE DEVICES
		GetTouchInput();

	}

	// RETURN LAST INPUT POSITION
	public Vector3 GetLastInputPosition() {
		return lastInputPosition;
	}

	// MOUSE EVENTS
	private void GetMouseInput() {
		if (Input.GetMouseButtonDown(0)) {
			lastInputPosition = Input.mousePosition;
		}
		else {
			lastInputPosition = Vector3.zero;
		}
	}

	// TOUCH EVENTS
	private void GetTouchInput() {

	}

}
