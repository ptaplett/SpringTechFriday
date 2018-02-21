using UnityEngine;
using System.Collections;

//2 [RequireComponent(typeof(CharacterController))] Forces unity to make sure GameObject has a component of argument pased
public class RelativeMovement : MonoBehaviour {

	[SerializeField] private Transform target;  // Shows up in editor so we can link player 
	public float rotSpeed = 15.0f; // Speed that character will rotate
	//2 public float moveSpeed = 6.0f; // Movement speed of character

	//3 public float jumpSpeed = 15.0f; 		// Speed of vertical jump
	//3 public float gravity = -9.8f;			// Force of gravity acting down on player
	//3 public float terminalVelocity = -10.0f;	// Maximum speed character can fall
	//3 public float minFall = -1.5f;			// Minumum speed character can fall

    public float pushForce = 3.0f;          // 6b Force at which player will push something

	//2 private CharacterController _charController; 	
	//3 private float _vertSpeed;						
	//4 private ControllerColliderHit _contact;		 	// Will store collision data 
	//5 private Animator _animator;						


	void Start() {
		//2 _charController = GetComponent<CharacterController> (); // Returns all the other components attached to character
		//3 _vertSpeed = minFall;									// Set initial fall speed to minimum
		//5 _animator = GetComponent<Animator>();					
	}

	void Update() {
		Vector3 movement = Vector3.zero;	// Start with (0,0,0) vector and add components
											// important this is done at beginning since we
											// will add different components seperately yet
											// they all need to be apart of the same vector
		float horInput = Input.GetAxis("Horizontal");	// Check 'a' and 'd' keys
		float vertInput = Input.GetAxis("Vertical");	// Check 'w' and 's' keys
		if (horInput != 0 || vertInput != 0) {			// If keys were pressed
			movement.x = horInput;						// Update vector with input
			//2 movement.x = horInput * moveSpeed;  						
			movement.z = vertInput;
			//2 movement.z = vertInput * moveSpeed; 						
			//2 movement = Vector3.ClampMagnitude(movement, moveSpeed); 	// Limits diagonal movement so it doesn't double up speed

			// Next 4 lines are for calculating movement direction as a vector
			Quaternion tmp = target.rotation;								// store the targets rotation to restore later
			target.eulerAngles = new Vector3(0, target.eulerAngles.y, 0); 	// adjust rotation so its only around y-axis
			movement = target.TransformDirection(movement); 				// Transforms from local coordinates to global
			target.rotation = tmp;											// restore the targets rotation

			transform.rotation = Quaternion.LookRotation(movement); 		// Applies movement direction to the character
																			// by converting vector3 into quaternion
			//1 Quaternion direction = Quaternion.LookRotation(movement); 		// Converts vector 3 into quaternion
			//1 transform.rotation = Quaternion.Lerp (transform.rotation, direction, rotSpeed * Time.deltaTime);  // Interpolate (move a small amount) between current rotation and new rotation
		}

		//5 _animator.SetFloat ("Speed", movement.sqrMagnitude);				

		//4 all of this raycasting logic
		/*
		bool hitGround = false;
		RaycastHit hit;
		if (_vertSpeed < 0 && Physics.Raycast (transform.position, Vector3.down, out hit)) {
			float check = (_charController.height + _charController.radius) / 1.9f; // check distance is slightly beyond bottom of the capsule
			hitGround = hit.distance <= check; // Sets hitGround to true/false depending on how far hit.distance is
		}
			alternate if / else
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
			*/ 
			
			/*
			//5 next if statement
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
			*/ // end of 5
		//4} // Make sure to uncomment bracket for 4 changes
			
			
		//3 this whole if else statement
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
		// 3movement.y = _vertSpeed;  // Apply result to movements y vector

		//2 movement *= Time.deltaTime; 	// Always want multiply by Time.deltaTime to make frame-rate independent
		//2 _charController.Move(movement); // Move is a function belonging to the CharacterController class (handles all horizontal movement)	
	}

	//4 Store collision data in the callback when a collision is detected.
	/*
	void OnControllerColliderHit(ControllerColliderHit hit) {
		_contact = hit;

        Rigidbody body = hit.collider.attachedRigidbody;    //6b Checks if object player collides with has RigidBody
        if (body != null && !body.isKinematic) {            // if a body was hit and that body is kinematic
            body.velocity = hit.moveDirection * pushForce;      // apply velocity to the physics body
        }
	}
	*/
}
