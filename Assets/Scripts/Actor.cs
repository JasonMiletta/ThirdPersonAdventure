using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Actor : MonoBehaviour {

    #region Info
    [Header("Parameters")]
    [Tooltip("Info/Parameters/Settings for this Actor")]
    public int totalHealth;
    public float maxHunger;
    public float maxThirst;
    public float maxStamina;
    public float maxSleep;
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
    public float currentHunger;
    public float currentThirst;
    public float currentStamina;
    public float currentSleep;
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

    #region Meters
    public bool consumeItem(Item item){
        if(item.isConsumable){
            modifyHunger(item.getHungerValue());
            modifyThirst(item.getThirstValue());
            modifyStamina(item.getStaminaValue());
            modifySleep(item.getSleepValue());
            return true;
        }
        return false;
    }

    private void modifyHunger(float hungerAmount){
        currentHunger += hungerAmount;
        if(currentHunger > maxHunger){
            currentHunger = maxHunger;
        } else if(currentHunger < 0){
            currentHunger = 0;
        }
    }

    private void modifyThirst(float thirstAmount){
        currentThirst += thirstAmount;
        if(currentThirst > maxThirst){
            currentThirst = maxHunger;
        } else if(currentThirst < 0){
            currentThirst = 0;
        }
    }

    private void modifyStamina(float staminaAmount){
        currentStamina += staminaAmount;
        if(currentStamina > maxStamina){
            currentStamina = maxStamina;
        } else if(currentStamina < 0){
            currentStamina = 0;
        }
    }
    
    private void modifySleep(float sleepAmount){
        currentSleep += sleepAmount;
        if(currentSleep > maxSleep){
            currentSleep = maxSleep;
        } else if(currentSleep < 0){
            currentSleep = 0;
        }
    }
    #endregion


}
