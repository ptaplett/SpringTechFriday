using UnityEngine;
using System.Collections;
public class DoorKeyDevice : MonoBehaviour {
	[SerializeField] private Vector3 dPos;		// The position to offset to when door opens

	private bool _open;			// Keeps track if door is open(true) or closed(false)

	// Activate and Deactivate are same open and close methods but
	// for the motion controlled door logic
	public void Activate() {							
		if (!_open) {
			Vector3 pos = transform.position + dPos;
			transform.position = pos;
			_open = true;
		}
	}
	public void Deactivate() {
		if (_open) {
			Vector3 pos = transform.position - dPos;
			transform.position = pos;
			_open = false;
		}
	}
}