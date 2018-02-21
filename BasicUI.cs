using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BasicUI : MonoBehaviour
{
    void OnGUI()
	{
		// dimensions of UI box
		int posX = 10;
		int posY = 10;
		int width = 100;
		int height = 30;
		int buffer = 10; // Space between two icons

		List<string> itemList = Managers.Inventory.GetItemList ();
		if (itemList.Count == 0) {
			GUI.Box (new Rect (posX, posY, width, height), "No Items");   // Display this message if inventory is empty
		}
		foreach (string item in itemList) {
			int count = Managers.Inventory.GetItemCount (item);
			Texture2D image = Resources.Load<Texture2D> ("Icons/" + item); // Will search your Icons folder for icon files matching the same string name
			GUI.Box (new Rect (posX, posY, width, height),                  // Create box for item picked up
				new GUIContent ("(" + count + ")", image));
			posX += width + buffer;
		}

		//1b
		string equipped = Managers.Inventory.equippedItem;
		if (equipped != null) {									// Checks if anything is currently equipped, if not it creates a new image of which item is equipped
			posX = Screen.width - (width + buffer);
			Texture2D image = Resources.Load<Texture2D> ("Icons/" + equipped) as Texture2D; 
			GUI.Box (new Rect (posX, posY, width, height),                  
				new GUIContent ("Equipped", image));
		}

		posX = 10;
		posY = height + buffer;

		foreach (string item in itemList) {
			if (GUI.Button (new Rect (posX, posY, width, height), "Equip " + item)) {
				Managers.Inventory.EquipItem (item);
			}//1b
			// posX += width + Buffer // apart of 1b, moves down below 2b after implementation of 2b

			//2b
			if (item == "Health") {
				if (GUI.Button(new Rect(posX, posY + height+buffer, width,
					height), "Use Health")) {
					Managers.Inventory.ConsumeItem("Health");
					Managers.Player.ChangeHealth(25);
				}
			}//2b

			posX += width+buffer; 

		}
	}
}
