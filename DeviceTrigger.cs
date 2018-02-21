using UnityEngine;
using System.Collections;
public class DeviceTrigger : MonoBehaviour {
	[SerializeField] private GameObject[] targets; // Drag and drop target doors to open in editor

	public bool requireKey; // 1b Public variable that you can select or deselect in editor

	// If an object enters the collider then send a message to all
	// linked targets to Activate (call the Activate function)
	void OnTriggerEnter(Collider other) {
		// 1b If key required checked in editor and character does not possess a key, return and don't open
		if (requireKey && Managers.Inventory.equippedItem != "Key") {
			return;
		}
		foreach (GameObject target in targets) {
			target.SendMessage("Activate");
		}
	}

	// If an object leaves the collider then send a message to all
	// linked targets to Deactivate (call the Deactivate function)
	void OnTriggerExit(Collider other) {
		foreach (GameObject target in targets) {
			target.SendMessage("Deactivate");
		}
	}
}