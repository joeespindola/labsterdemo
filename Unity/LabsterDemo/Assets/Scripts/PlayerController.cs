using UnityEngine;
using System.Collections;

/*
* 	PLAYER CONTROLLER IS RESPONSIBLE FOR
* 
*	- MOVING THE GAME OBJECT TRANSFORM BASED ON A TARGET
* 	- SENDING COLLISIONS TRIGGERS TO GAME CONTROLLER
*/
public class PlayerController : MonoBehaviour
{
	public float moveSpeed = 3.0f;
	public float gravity = 20.0f;
	public float targetHitDistance = 2.0f;

	public GameController gameController;
	
	private CharacterController playerCharacterController;
	private Rigidbody playerRigidBody;
	
	private Vector3 targetPosition;
	private Vector3 playerDirection;
	private Vector3 lastPlayerDirection;

	public Transform playerLookAt;
	
	void Start ()
	{
		playerCharacterController = gameObject.GetComponent<CharacterController>();
		playerRigidBody = gameObject.GetComponent<Rigidbody>();
		
		playerRigidBody.freezeRotation = true;
		
		playerDirection = Vector3.zero;
		targetPosition = Vector3.zero;
	}

	// SET TARGET POSITION
	public void SetTargetPosition(Vector3 position) {
		// IF VECTOR3 ZERO STOP PLAYER
		if(position == Vector3.zero) {
			targetPosition = transform.position;
			lastPlayerDirection = targetPosition;
		}
		else {
			targetPosition = position;
			lastPlayerDirection = targetPosition;
		}
	}

	// GET TARGET POSITION
	public Vector3 GetTargetPosition() {
		return targetPosition;
	}
	
	public void Tick() {

		// MOVE PLAYER
		Move();

	}

	private void Move() {

		if (targetPosition.magnitude > 0f) {
			// MOVE TORWARDS TARGET
			playerDirection = targetPosition - transform.position;
			
			// CHECK IF ARRIVED AT TARGET
			Vector3 x = targetPosition;
			// RESET Y TO GIVE A 1D VALUE ON MAGNITUDE
			x.y = transform.position.y;
			
			if( (transform.position - x).magnitude < targetHitDistance ){
				// PLAYER ARRIVED, RESET VALUES
				targetPosition = Vector3.zero;
				playerDirection = Vector3.zero;
			}
			
		} 
		
		playerDirection.y = 0;
		
		playerDirection = playerDirection.normalized;
		playerDirection *= moveSpeed * Time.deltaTime;
		
		playerDirection.y = -gravity * Time.deltaTime;
		
		if(playerDirection.magnitude > 0) playerLookAt.LookAt(transform.position + (lastPlayerDirection*10));
		
		playerCharacterController.Move(playerDirection);

	}

	// COLLISION TRIGGER FROM PLAYER DELEGATES COLLISION TO GAME CONTROLLER
	void OnTriggerEnter(Collider other) {
		gameController.PlayerHasCollisions(other);
	}

}

