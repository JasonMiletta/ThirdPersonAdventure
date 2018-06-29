using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInventory : MonoBehaviour {

    #region Info
    [Header("Parameters")]
    [Tooltip("Info/Parameters/Settings for this Actor")]
	public int maxCapacity = 10;
	public List<Item> inventoryList;
	#endregion

	// Use this for initialization
	void Start () {
		inventoryList = new List<Item>();
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	public bool addNewItem(Item newItem){
		if(inventoryList.Count < maxCapacity){
			inventoryList.Add(newItem);
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
			inventoryList.RemoveAt(itemIndex);
		}
	}
}
