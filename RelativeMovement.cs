using UnityEngine;
using System.Collections;

[RequireComponent(typeof(CharacterController))] // Forces unity to make sure GameObject has a component of argument pased
public class RelativeMovement : MonoBehaviour {

	[SerializeField] private Transform target;  // Shows up in editor so we can link player 
	public float rotSpeed = 15.0f; // Speed that character will rotate

	public float moveSpeed = 6.0f; // Movement speed of character
	public float jumpSpeed = 15.0f; 		// Speed of vertical jump
	public float gravity = -9.8f;			// Force of gravity acting down on player
	public float terminalVelocity = -10.0f;	// Maximum speed character can fall
	public float minFall = -1.5f;			// Minumum speed character can fall
	//6 public float pushForce = 3.0f;          // Force at which player will push something

	private CharacterController _charController; 	
	private float _vertSpeed;						
	private ControllerColliderHit _contact;		 	// Will store collision data 
	//5 private Animator _animator;						


	void Start() {
		_charController = GetComponent<CharacterController> (); // Returns all the other components attached to character
		_vertSpeed = minFall;									// Set initial fall speed to minimum
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
			movement.x = horInput * moveSpeed;  		// Update vector with input					
			movement.z = vertInput * moveSpeed; 						
			movement = Vector3.ClampMagnitude(movement, moveSpeed); 	// Limits diagonal movement so it doesn't double up speed

			// Next 4 lines are for calculating movement direction as a vector
			Quaternion tmp = target.rotation;								// store the targets rotation to restore later
			target.eulerAngles = new Vector3(0, target.eulerAngles.y, 0); 	// adjust rotation so its only around y-axis
			movement = target.TransformDirection(movement); 				// Transforms from local coordinates to global
			target.rotation = tmp;											// restore the targets rotation

			//transform.rotation = Quaternion.LookRotation(movement); 		// Applies movement direction to the character
			// by converting vector3 into quaternion
			Quaternion direction = Quaternion.LookRotation(movement); 		// Converts vector 3 into quaternion
			transform.rotation = Quaternion.Lerp (transform.rotation, direction, rotSpeed * Time.deltaTime);  // Interpolate (move a small amount) between current rotation and new rotation
		}

		//5 _animator.SetFloat ("Speed", movement.sqrMagnitude);				


		bool hitGround = false;
		RaycastHit hit;
		if (_vertSpeed < 0 && Physics.Raycast (transform.position, Vector3.down, out hit)) {
			float check = (_charController.height + _charController.radius) / 1.9f; // check distance is slightly beyond bottom of the capsule
			hitGround = hit.distance <= check; // Sets hitGround to true/false depending on how far hit.distance is
		}

		if (hitGround) {		// if character is standing on the ground
			if (Input.GetButtonDown ("Jump")) { 	// If jump button is pressed 
				_vertSpeed = jumpSpeed;				// Set vertical speed to jump speed
			} else {
				_vertSpeed = minFall;	// Else reset vertical speed back to minimum 
				//5 _vertSpeed = -0.1f; 	
				//5 _animator.SetBool("Jumping", false);	
			}
		} else {
			_vertSpeed += gravity * 5 * Time.deltaTime; // If not not on ground apply gravity until terminal velocity reached
			if (_vertSpeed < terminalVelocity) {
				_vertSpeed = terminalVelocity;
			}
			 
			//5 if (_contact != null) {
			//5	_animator.SetBool("Jumping", true);
			//5 }
			
				
			if (_charController.isGrounded) {						// Check if capsule collider is hitting an edge while not standing on ground
				if (Vector3.Dot (movement, _contact.normal) < 0) {	// If facing towards edge when falling then set new movement direction
					movement = _contact.normal * moveSpeed;
				} else {
					movement += _contact.normal * moveSpeed;		// Else if facing away from edge then add to existing movement vector
				}
			}
		}


		movement.y = _vertSpeed;  // Apply result to movements y vector
		movement *= Time.deltaTime; 	// Always want multiply by Time.deltaTime to make frame-rate independent
		_charController.Move(movement); // Move is a function belonging to the CharacterController class (handles all horizontal movement)	
	}

	// Store collision data in the callback when a collision is detected.
	void OnControllerColliderHit(ControllerColliderHit hit) {
		_contact = hit;
		
		//6 Rigidbody body = hit.collider.attachedRigidbody;    // Checks if object player collides with has RigidBody
        //6 if (body != null && !body.isKinematic) {            // if a body was hit and that body is kinematic
            //6 body.velocity = hit.moveDirection * pushForce;      // apply velocity to the physics body
        //6}
	}
}