using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InventoryManager : MonoBehaviour, IGameManager
{
	public ManagerStatus status {get; private set;}
	private Dictionary<string, int> _items;  // Initialize the empty list 

	public string equippedItem { get; private set; } // 2b This property will be checked by Device Trigger call

	public void Startup()
	{
		Debug.Log("Inventory manager starting...");

		_items = new Dictionary<string, int>();

		status = ManagerStatus.Started;
	}

	// Prints a message to the console of the current inventory
	private void DisplayItems() 
	{
		string itemDisplay = "Items: ";

		foreach (KeyValuePair<string, int> item in _items)
		{
			itemDisplay += item.Key + "(" + item.Value + ") ";
		}

		Debug.Log(itemDisplay);
	}

	// Ensures that other scripts can't manipulate the item list, but must call this method
	public void AddItem(string name)
	{

		if (_items.ContainsKey(name))   // Checks if dictionary already contains that item
		{
			_items[name] += 1;          // Then increment count
		}
		else                            // Else if its a new entry then create the entry
		{
			_items[name] = 1;
		}

		DisplayItems();
	}

	// Returns a list of items in inventory.  List is fine because we only need each item
	// to appear once. 
	public List<string> GetItemList()
	{
		List<string> list = new List<string>(_items.Keys);
		return list;
	}

	// Returns the number of items currently held of a specific item type
	public int GetItemCount(string name)
	{
		if (_items.ContainsKey(name))
		{
			return _items[name];
		}
		return 0;
	}


	// Equips item if isn't already equipped or unequips current item
	public bool EquipItem(string name) {
		if (_items.ContainsKey(name) && equippedItem != name) {
			equippedItem = name;
			Debug.Log("Equipped " + name);
			return true;
		}

		equippedItem = null;
		Debug.Log ("Unequipped");
		return false;
	}

	// Passes the string item name as argument.  If item is in inventory decrement 1 from
	// inventory.  Also if item count goes to 0 remove that item from inventory.  Also if item
	// isn't in inventory, display cannot consume "itemname"
	public bool ConsumeItem(string name) {
		if (_items.ContainsKey(name)) {
			_items[name]--;
			if (_items[name] == 0) {
				_items.Remove(name);
			}
		} else {
			Debug.Log("cannot consume " + name);
			return false;
		}
		DisplayItems();
		return true;
	}
}