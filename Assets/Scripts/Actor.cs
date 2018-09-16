using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Actor : MonoBehaviour {

    #region Info
    [Header("Parameters")]
    [Tooltip("Info/Parameters/Settings for this Actor")]
    public float totalHealth;
    public float maxHunger;
    public float maxThirst;
    public float maxStamina;
    public float maxSleep;
    public Item rightHandItem;
    public Item leftHandItem;
    #endregion

    #region Components
    [Header("Components")]
    private PhysicsAnimation_Human m_physicsAnimation_Human;
    private Actor_Equipment m_actorEquipment;
    [SerializeField]
    private Transform rightHand;
    [SerializeField]
    private Transform leftHand;
    #endregion

    #region State
    public bool isAlive;
    public float currentHunger;
    public float currentThirst;
    public float currentStamina;
    public float currentSleep;
    public float currentHealth; 
    private Inventory inventory;
    #endregion

	// Use this for initialization
	void Start () {
        currentHealth = totalHealth;
        isAlive = true;

        inventory = GetComponent<Inventory>();
        m_actorEquipment = GetComponent<Actor_Equipment>();
        m_physicsAnimation_Human = GetComponent<PhysicsAnimation_Human>();
	}
	
	// Update is called once per frame
	void Update () {
        if(currentHealth <= 0){
            isAlive = false;
        }
        
        if(isAlive != true){
            killSelf();
        }
	}

    /// <summary>
    /// Attempt to physically grab the object (not place in inventory)
    /// </summary>
    /// <param name="item">Item that the actor is trying to grab.</param>
    /// <returns>Returns true on success, false on failure.</returns>
    public bool grabItem(Item item){
        if(rightHandItem == null && rightHand != null){
            rightHandItem = item;
            item.bringToHand(rightHand);
            return true;
        }

        return false;
    }

    /// <summary>
    /// Drops the item currently held in the actor's left hand, otherwise try to drop the item in the actor's right hand.
    /// </summary>
    /// <returns></returns>
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

    /// <summary>
    /// Pulls the provided item into this actor's inventory.
    /// </summary>
    /// <param name="item">Item that the actor is attempting to loot.</param>
    /// <returns>Returns true on success, false on failure.</returns>
    public bool lootItem(Item item){
        if(inventory != null){
            item.bringToInventory(inventory.backpack);
            inventory.addNewItem(item);
            return true;
        }

        return false;
    }

    /// <summary>
    /// Retrieves the position of this actor's inventory so we know where to float items to.
    /// </summary>
    /// <returns>Returns the Transform of the inventory object for this actor.</returns>
    public Transform getInventoryTransform(){
        if(inventory != null){
            return inventory.backpack.transform;
        }
        return null;
    }

    /// <summary>
    /// Retrieve this actor's Actor_Equipment. Used to see what items are currently equipped.
    /// </summary>
    /// <returns>Returns Actor_Equipment containing equipment setup</returns>
    public Actor_Equipment getCurrentEquipment(){
        return m_actorEquipment;
    }

    /// <summary>
    /// Attempts to equip the provided item.
    /// </summary>
    /// <param name="item">The item we're trying to equip for this actor</param>
    /// <returns>Returns true on success, false on failure if an item is already equipped</returns>
    public bool equipItem(Item item){
        if(m_actorEquipment != null){
            if(item.isEquipable){
                return m_actorEquipment.equipItem(item);
            }
        }
        return false;
    }

    /// <summary>
    /// Attempts to unequip the item in the provided slot
    /// </summary>
    /// <param name="placement">Placement of the item we want to remove (Helmet, Torso, legs, etc)</param>
    /// <returns>Returns true on success, false on failure, no item, or no Actor_Equipment</returns>
    public bool unEquipItem(Item_Equipment.Placement placement){
        if(m_actorEquipment != null){
            return m_actorEquipment.unEquipItem(placement);
        }
        return false;
    }


    /// <summary>
    /// Attempts to unequip the provided item
    /// </summary>
    /// <param name="item">Item that we want to unequip</param>
    /// <returns>Returns true on success, false on failure, no item, or no Actor_Equipment</returns>
    public bool unEquipItem(Item item){
        if(m_actorEquipment != null){
            return m_actorEquipment.unEquipItem(item);
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

    /// <summary>
    /// Kills this actor. Yeesh this got dark quick
    /// </summary>
    public void killSelf(){
        if(m_physicsAnimation_Human != null){
            m_physicsAnimation_Human.stopAllForces();
        }
        if(m_actorEquipment != null){
            //TODO Do we drop all equipment BOTW style or do we leave the body as a 'lootbox'
        }
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
