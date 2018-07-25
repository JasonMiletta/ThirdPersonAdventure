using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Inventory : MonoBehaviour {

    #region Info
    [Header("Parameters")]
    [Tooltip("Info/Parameters/Settings for this Actor")]
	public int maxCapacity = 10;
	public List<Item> inventoryList;
	#endregion

	#region State
	public int currentCapacity = 0;
	#endregion

	#region Component
	public GameObject backpack;
	#endregion

	#region Events
    public delegate void ItemInventoryEvent(Item item, Inventory inventory);
    public static event ItemInventoryEvent OnItemAdded;
    public static event ItemInventoryEvent OnItemRemoved;
	#endregion

	// Use this for initialization
	void Start () {
		inventoryList = new List<Item>();
	}
	
	// Update is called once per frame
	void Update () {
		
	}
	/// <summary>
	///	This adds the provided item to the current inventory and updates the current capacity of this inventory
	/// </summary>
	public bool addNewItem(Item newItem){
		if((currentCapacity + newItem.weight) < maxCapacity){
			inventoryList.Add(newItem);
			currentCapacity += newItem.weight;
			
            if(OnItemAdded != null){OnItemAdded(newItem, this);}
			return true;
		}

		return false;
	}

	public Item retrieveItem(int itemIndex){
		if(inventoryList.Count > itemIndex){
			return inventoryList[itemIndex];
		}

		return null;
	}

	public void removeItem(int itemIndex){
		if(inventoryList.Count > itemIndex){
			Item itemToRemove = inventoryList[itemIndex].GetComponent<Item>();
			currentCapacity -= itemToRemove.weight;
			inventoryList.RemoveAt(itemIndex);
			
            if(OnItemRemoved != null){OnItemRemoved(itemToRemove, this);}
		}
	}
}
