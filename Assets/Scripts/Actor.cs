using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Actor : MonoBehaviour {

    #region Info
    [Header("Parameters")]
    [Tooltip("Info/Parameters/Settings for this Actor")]
    public int totalHealth;
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

    public bool lootItem(Item item){
        if(inventory != null){
            item.bringToInventory(inventory.backpack);
            inventory.addNewItem(item);
            //item.gameObject.transform.parent = this.transform;
            return true;
        }

        return false;
    }
}
