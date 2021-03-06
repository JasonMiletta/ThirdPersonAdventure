using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Item : MonoBehaviour, IEntity {

    #region Info
    [Header("Parameters")]
    [Tooltip("Info/Parameters/Settings for this Actor")]
    public string m_apiName;
    public string m_displayName;
    public bool isGrabbable = false;
    public bool isLootable = true;
    public int weight = 1;
    public string apiName{
        get {return m_apiName;}
    }
    public string displayName{
        get {return m_displayName;}
    }
    public bool isInteractable{
        get;set;
    }
    public bool isConsumable;
    public bool isEquipable;
    #endregion

    #region State
    [Header("State")]
    public bool isCurrentlyHeld = false;
    public bool isCurrentlyEquipped = false;
    #endregion

    #region Components
    UI_ToolTip tooltip;
    CapsuleCollider capsuleCollider;
    MeshCollider meshCollider;
    Item_Consumable consumable;
    Item_Equipment equipable;
    #endregion


    #region IEntity Interface
    void IEntity.Initialize(){
        if(apiName == null || apiName.Equals("")){
            Debug.LogError("Item does not have an apiName!");
        }
        if(displayName == null || displayName.Equals("")){
            Debug.LogError("Item does not have a displayName!");
        }
    }
    #endregion

	// Use this for initialization
	void Start () {
        tooltip = GetComponentInChildren<UI_ToolTip>();
        capsuleCollider = GetComponent<CapsuleCollider>();
        meshCollider = GetComponent<MeshCollider>();
        
        consumable = GetComponent<Item_Consumable>();
        isConsumable = consumable != null;
        equipable = GetComponent<Item_Equipment>();
        isEquipable = equipable != null;
        
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    /// <summary>
    /// Gets the Item_Equipment data attached to this item's object
    /// </summary>
    /// <returns>Item_Equipable or null if it doesn't exist</returns>
    public Item_Equipment getItemEquipment(){
        if(isEquipable && equipable != null){
            return equipable;
        }
        return null;
    }

    public float getHungerValue(){
        return isConsumable ? consumable.hungerValue : 0;
    }
    public float getThirstValue(){
        return isConsumable ? consumable.thirstValue : 0;
    }
    
    public float getStaminaValue(){
        return isConsumable ? consumable.staminaValue : 0;
    }

    public float getSleepValue(){
        return isConsumable ? consumable.sleepValue : 0;
    }

    public void bringToInventory(GameObject inventory){
        prepareForBeingTaken();

        StartCoroutine(Util_TransformManipulation.smoothMovement(this.gameObject, this.transform.position, inventory.transform.position, 0.1f));
        StartCoroutine(Util_TransformManipulation.lerpObjToScale(this.gameObject, new Vector3(0.01f, 0.01f, 0.01f), 1f));
    }
    
    public void bringToHand(Transform hand){
        prepareForBeingTaken();

        this.transform.up = hand.transform.forward;
        StartCoroutine(Util_TransformManipulation.smoothMovement(this.gameObject, this.transform.position, hand.transform.position, 0.1f));
        this.transform.parent = hand;
    }

    public void dropItem(){
        isCurrentlyHeld = false;
        enableColliders();
        GetComponent<Rigidbody>().isKinematic = false;
    }

    public void displayUIPrompt(){
        if(tooltip != null && !isCurrentlyHeld){
            tooltip.displayTooltip();
        }
    }

    public void hideUIPrompt(){
        if(tooltip != null){
            tooltip.hideTooltip();
        }
    }

    private void prepareForBeingTaken(){
        hideUIPrompt();
        isCurrentlyHeld = true;
        GetComponent<Rigidbody>().isKinematic = true;
        disableColliders();
    }

    private void enableColliders(){
        if(capsuleCollider != null){
            capsuleCollider.enabled = true;    
        }
        if(meshCollider != null){
            meshCollider.enabled = true;
        }
    }

    private void disableColliders(){
        if(capsuleCollider != null){
            capsuleCollider.enabled = false;    
        }
        if(meshCollider != null){
            meshCollider.enabled = false;
        }
    }
}