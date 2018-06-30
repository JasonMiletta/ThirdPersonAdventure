using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Actor : MonoBehaviour, IEntity {

    #region Info
    [Header("Parameters")]
    [Tooltip("Info/Parameters/Settings for this Actor")]
    public int totalHealth;

    public string apiName{
        get;
        set;
    }
    public string displayName{
        get;
        set;
    }
    #endregion

    #region State
    private int currentHealth; 
    private Inventory inventory;
    #endregion


    #region IEntity Interface
    void IEntity.Initialize(){
    }
    #endregion

	// Use this for initialization
	void Start () {
        if(apiName == null || apiName.Equals("")){
            Debug.LogError("Actor does not have an apiName!");
        }
        if(displayName == null || displayName.Equals("")){
            Debug.LogError("Actor does not have a displayName!");
        }

        currentHealth = totalHealth;

        inventory = GetComponent<Inventory>();
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public bool lootItem(Item item){
        if(inventory != null){
            inventory.addNewItem(item);
            return true;
        }

        return false;
    }
}
