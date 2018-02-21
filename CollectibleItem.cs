using UnityEngine;
using System.Collections;
public class CollectibleItem : MonoBehaviour
{
    [SerializeField] private string itemName;       // Item name can be set in Inspector

    void OnTriggerEnter(Collider other)
    {
        Debug.Log("Item collected: " + itemName);   // Prints to console, so you Know something happened
        Destroy(this.gameObject);                   // Removes game object
    }

    //1b
	/*
    void OnTriggerEnter(Collider other)
    {
        Managers.Inventory.AddItem(name);
        Destroy(this.gameObject);
    }*/
}
