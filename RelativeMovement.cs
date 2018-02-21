using UnityEngine;
using System.Collections;

[RequireComponent(typeof(CharacterController))]	// 2b Forces unity to make sure GameObject has a component of argument pased
public class RelativeMovement : MonoBehaviour {

	[SerializeField] private Transform target;  // Shows up in editor so we can link player 
	public float rotSpeed = 15.0f; // Speed that character will rotate
	public float moveSpeed = 6.0f; // 2b Movement speed of character

	public float jumpSpeed = 15.0f; 		// 3b Speed of vertical jump
	public float gravity = -9.8f;			// 3b Force of gravity acting down on player
	public float terminalVelocity = -10.0f;	// 3b Maximum speed character can fall
	public float minFall = -1.5f;			// 3b Minumum speed character can fall

    public float pushForce = 3.0f;          // 6b Force at which player will push something

	private CharacterController _charController; 	// 2b
	private float _vertSpeed;						// 3b
	private ControllerColliderHit _contact;		 	// 4b Will store collision data 
	private Animator _animator;						// 5b


	void Start() {
		_charController = GetComponent<CharacterController> (); // 2b Returns all the other components attached to character
		_vertSpeed = minFall;									// 3b Set initial fall speed to minimum
		_animator = GetComponent<Animator>();					// 5b
	}

	void Update() {
		Vector3 movement = Vector3.zero;	// Start with (0,0,0) vector and add components
											// important this is done at beginning since we
											// will add different components seperately yet
											// they all need to be apart of the same vector
		float horInput = Input.GetAxis("Horizontal");	// Check 'a' and 'd' keys
		float vertInput = Input.GetAxis("Vertical");	// Check 'w' and 's' keys
		if (horInput != 0 || vertInput != 0) {			// If keys were pressed
			// 2a movement.x = horInput;						// Update vector with input
			movement.x = horInput * moveSpeed;  						// 2b 
			// 2amovement.z = vertInput;
			movement.z = vertInput * moveSpeed; 						// 2b
			movement = Vector3.ClampMagnitude(movement, moveSpeed); 	// 2b Limits diagonal movement so it doesn't double up speed

			// Next 4 lines are for calculating movement direction as a vector
			Quaternion tmp = target.rotation;								// store the targets rotation to restore later
			target.eulerAngles = new Vector3(0, target.eulerAngles.y, 0); 	// adjust rotation so its only around y-axis
			movement = target.TransformDirection(movement); 				// Transforms from local coordinates to global
			target.rotation = tmp;											// restore the targets rotation

			// 1a transform.rotation = Quaternion.LookRotation(movement); 	// Applies movement direction to the character
																			// by converting vector3 into quaternion
			Quaternion direction = Quaternion.LookRotation(movement); 		// 1b  Converts vector 3 into quaternion
			transform.rotation = Quaternion.Lerp (transform.rotation, direction, rotSpeed * Time.deltaTime);  // 1b  Interpolate (move a small amount) between current rotation and new rotation
		}

		_animator.SetFloat ("Speed", movement.sqrMagnitude);				// 5b

		//4b all of this raycasting logic
		bool hitGround = false;
		RaycastHit hit;
		if (_vertSpeed < 0 && Physics.Raycast (transform.position, Vector3.down, out hit)) {
			float check = (_charController.height + _charController.radius) / 1.9f; // check distance is slightly beyond bottom of the capsule
			hitGround = hit.distance <= check; // Sets hitGround to true/false depending on how far hit.distance is
		}
		//4b alternate if / else
		if (hitGround) {		// if character is standing on the ground
			if (Input.GetButtonDown ("Jump")) { 	// If jump button is pressed 
				_vertSpeed = jumpSpeed;				// Set vertical speed to jump speed
			} else {
				//5a(apart of 4b changed with 5b)_vertSpeed = minFall;				// Else reset vertical speed back to minimum 
				_vertSpeed = -0.1f;		//5b
				_animator.SetBool("Jumping", false);	//5b
			}
		} else {
			_vertSpeed += gravity * 5 * Time.deltaTime; // If not not on ground apply gravity until terminal velocity reached
			if (_vertSpeed < terminalVelocity) {
				_vertSpeed = terminalVelocity;
			}
			//5b next if statement
			if (_contact != null) {
				_animator.SetBool("Jumping", true);
			}
				
			if (_charController.isGrounded) {						// Check if capsule collider is hitting an edge while not standing on ground
				if (Vector3.Dot (movement, _contact.normal) < 0) {	// If facing towards edge when falling then set new movement direction
					movement = _contact.normal * moveSpeed;
				} else {
					movement += _contact.normal * moveSpeed;		// Else if facing away from edge then add to existing movement vector
				}
			}
		}
			
			
		//3b this whole if else statement
		/*if (_charController.isGrounded) {		// isGrounded is a method of CharacterController to check if character is on ground or not
			if (Input.GetButtonDown ("Jump")) { 	// If jump button is pressed
				_vertSpeed = jumpSpeed;				// Set vertical speed to jump speed
			} else {
				_vertSpeed = minFall;				// Else reset vertical speed back to minimum 
			}
		} else {
			_vertSpeed += gravity * 5 * Time.deltaTime; // If not not on ground apply gravity until terminal velocity reached
			if (_vertSpeed < terminalVelocity) {
				_vertSpeed = terminalVelocity;
			}
		}*/
		movement.y = _vertSpeed;  // 3b Apply result to movements y vector

		movement *= Time.deltaTime; 	// 2b Always want multiply by Time.deltaTime to make frame-rate independent
		_charController.Move(movement); // 2b Move is a function belonging to the CharacterController class (handles all horizontal movement)	
	}

	//4b Store collision data in the callback when a collision is detected.
	void OnControllerColliderHit(ControllerColliderHit hit) {
		_contact = hit;

        Rigidbody body = hit.collider.attachedRigidbody;    //6b Checks if object player collides with has RigidBody
        if (body != null && !body.isKinematic) {            // if a body was hit and that body is kinematic
            body.velocity = hit.moveDirection * pushForce;      // apply velocity to the physics body
        }
	}
}
