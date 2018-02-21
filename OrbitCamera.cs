using UnityEngine;
using System.Collections;
public class OrbitCamera : MonoBehaviour {

	[SerializeField] private Transform target; 	// Serialized allows the variable to show up in editor, 
												// so you can link player model to it

	public float rotSpeed = 1.5f;  	// Camera rotation speed
	private float _rotY;		  	// Float variable Y	
	private Vector3 _offset;		// Vector3 variable for offset Vector3 is (0,0,0) or (x,y,z)

	void Start() {
		_rotY = transform.eulerAngles.y;				// Translates rotation in degrees around y axis (complicated math)
		_offset = target.position - transform.position; // Store difference between players position and camera position
														// Important we did this at the Start() so position of camera can 
														// be maintained as script runs.  Meaning the distance between camera
														// and player wont change.
	}

	// LateUpdate is called last, we use LateUpdate to make sure player has moved before moving the camera.
	void LateUpdate() {
		float horInput = Input.GetAxis("Horizontal");  	// Checks to see if horizontal keys pressed (default are 'a' and 'd'	
		if (horInput != 0) {							// If 'a' or 'd' was pressed
			_rotY += horInput * rotSpeed;				// Rotate the camera slowly
		} else {
			_rotY += Input.GetAxis("Mouse X") * rotSpeed * 3;	// Else rotate camera quickly with mouse movement
		}
		Quaternion rotation = Quaternion.Euler(0, _rotY, 0); // Quaternions are used to represent rotations (complicated math)
		transform.position = target.position - (rotation * _offset); // Big deal line here.  (rotation * _offset) is multiplying a
																	 // Quaternion by a position vector which results in calculating
																	 // the rotated offset position then determines the new position
																	 // of camera by subtracting the new rotated offset position from the
																	 // target position.
		transform.LookAt(target);	// Points the camera towards the target.
	}
}
