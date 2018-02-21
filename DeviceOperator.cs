using UnityEngine;
using System.Collections;
public class DeviceOperator : MonoBehaviour {
	public float radius = 1.5f;		// How far away player is to activate device

	void Update() {
		if (Input.GetButtonDown("Fire3")) {
			Collider[] hitColliders =
				Physics.OverlapSphere(transform.position, radius);		// OverlapSphere returns a list of all the objects it is touching

			// For each collider object currently inside the hit collider
			foreach (Collider hitCollider in hitColliders) {
				Vector3 direction = hitCollider.transform.position - transform.position;	// Ensures the player has to be facing the object
				if (Vector3.Dot(transform.forward, direction) > .5f) {
					hitCollider.SendMessage("Operate",						// SendMessage trys to call the named Function ("Operate"), doesn't need to know targets type
						SendMessageOptions.DontRequireReceiver);			// This command will ignore the error message if nothing in object is recieved since we know most
																			// objects wont have the Operate function attached to it.
				}
			}
		}
	}
}
