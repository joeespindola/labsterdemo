using UnityEngine;
using System.Collections;

// PLAYER CLASS SIMPLY MOVES GAMEOBJECT TRANSFORM AROUND BASED ON TARGET
public class PlayerController : MonoBehaviour
{
	public float moveSpeed = 3.0f;
	public float gravity = 20.0f;
	public float targetHitDistance = 2.0f;

	private CharacterController playerCharacterController;
	private Rigidbody playerRigidBody;

	private Vector3 targetPosition;
	private Vector3 playerDirection;

	void Start ()
	{
		playerCharacterController = gameObject.GetComponent<CharacterController>();
		playerRigidBody = gameObject.GetComponent<Rigidbody>();

		playerRigidBody.freezeRotation = true;

		playerDirection = Vector3.zero;
		targetPosition = Vector3.zero;
	}

	public void Init() {

	}

	public void SetTargetPosition(Vector3 position) {
		targetPosition = position;
	}

	public Vector3 GetTargetPosition() {
		return targetPosition;
	}
	
	public void Tick() {

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

		playerCharacterController.Move(playerDirection);



	}

}

