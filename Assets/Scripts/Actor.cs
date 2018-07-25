using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Actor : MonoBehaviour {

    #region Info
    [Header("Parameters")]
    [Tooltip("Info/Parameters/Settings for this Actor")]
    public int totalHealth;
    public Item rightHandItem;
    public Item leftHandItem;
    #endregion

    #region Components
    [Header("Components")]
    [SerializeField]
    private Transform rightHand;
    [SerializeField]
    private Transform leftHand;
    #endregion

    #region State
    private int currentHealth; 
    private Inventory inventory;
    #endregion

	// Use this for initialization
	void Start () {
        currentHealth = totalHealth;

        inventory = GetComponent<Inventory>();
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public bool grabItem(Item item){
        if(rightHandItem == null && rightHand != null){
            rightHandItem = item;
            item.bringToHand(rightHand);
            return true;
        }

        return false;
    }

    public bool dropItem(){
        if(leftHandItem != null){
            leftHandItem.dropItem();
            leftHandItem.transform.parent = null;
            leftHandItem = null;
            return true;
        } else if(rightHandItem != null){
            rightHandItem.dropItem();
            rightHandItem.transform.parent = null;
            rightHandItem = null;
            return true;
        }

        return false;
    }

    public bool lootItem(Item item){
        if(inventory != null){
            item.bringToInventory(inventory.backpack);
            inventory.addNewItem(item);
            return true;
        }

        return false;
    }
}
