using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Inventory : MonoBehaviour {

    #region Info
    [Header("Parameters")]
    [Tooltip("Info/Parameters/Settings for this Actor")]
	public int maxCapacity = 10;
	public List<GameObject> inventoryList;
	#endregion

	#region State
	public int currentCapacity = 0;
	#endregion

	#region Component
	public GameObject backpack;
	#endregion

	// Use this for initialization
	void Start () {
		inventoryList = new List<GameObject>();
	}
	
	// Update is called once per frame
	void Update () {
		
	}
	/// <summary>
	///	This adds the provided item to the current inventory and updates the current capacity of this inventory
	/// </summary>
	public bool addNewItem(Item newItem){
		if((currentCapacity + newItem.weight) < maxCapacity){
			inventoryList.Add(newItem.gameObject);
			currentCapacity += newItem.weight;
			return true;
		}

		return false;
	}

	public GameObject retrieveItem(int itemIndex){
		if(inventoryList.Count > itemIndex){
			return inventoryList[itemIndex];
		}

		return null;
	}

	public void removeItem(int itemIndex){
		if(inventoryList.Count > itemIndex){
			currentCapacity -= inventoryList[itemIndex].GetComponent<Item>().weight;
			inventoryList.RemoveAt(itemIndex);
		}
	}
}
