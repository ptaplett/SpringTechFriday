using UnityEngine;
using System.Collections;
public class DoorOpenDevice : MonoBehaviour {
	[SerializeField] private Vector3 dPos;		// The position to offset to when door opens

	private bool _open;			// Keeps track if door is open(true) or closed(false)

	public void Operate() {
		if (_open) {
			Vector3 pos = transform.position - dPos;	// Move to offset to open
			transform.position = pos;
		} else {
			Vector3 pos = transform.position + dPos;	// Reset back to original position
			transform.position = pos;
		}
		_open = !_open;									// Set initial state to not open
	}
}