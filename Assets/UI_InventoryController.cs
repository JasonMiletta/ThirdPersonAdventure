using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityStandardAssets.CrossPlatformInput;

public class UI_InventoryController : MonoBehaviour {

	#region State
	[Header("State")]
	public bool isDisplaying = false;
	#endregion

	#region Components
	private Animator animator;
	public Inventory playerInventory;
	#endregion

	 void OnEnable()
    {
		Inventory.OnItemAdded += handleOnItemAddedEvent;
		Inventory.OnItemRemoved += handleOnItemRemovedEvent;
    }

	void OnDisable(){
		Inventory.OnItemAdded -= handleOnItemAddedEvent;
		Inventory.OnItemRemoved -= handleOnItemRemovedEvent;
	}

	// Use this for initialization
	void Start () {
		animator = GetComponent<Animator>();
		if(playerInventory == null){
			Debug.LogError("UI_InventoryController doesn't have a reference to the playerInventory");
		}
	}
	
	// Update is called once per frame
	void Update () {
		handleInventoryInput();
	}
	
    private void handleInventoryInput(){
        if(CrossPlatformInputManager.GetButtonDown("Inventory")){
            if(isDisplaying){
				hideInventory();
			} else {
				showInventory();
			}
        }
    }

	private void showInventory(){
		isDisplaying = true;
		animator.SetBool("isDisplaying", true);
	}

	private void hideInventory(){
		isDisplaying = false;
		animator.SetBool("isDisplaying", false);
	}

	private void handleOnItemAddedEvent(Item itemAdded, Inventory inventory){
		//TODO Create Item inventory node
	}

	private void handleOnItemRemovedEvent(Item itemRemoved, Inventory inventory){
		//TODO Remove item inventory node
	}
}
