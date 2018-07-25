using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityStandardAssets.CrossPlatformInput;

public class UI_InventoryController : MonoBehaviour {

	#region State
	[Header("State")]
	public bool isDisplaying = false;
	#endregion

	#region Components
	private Animator animator;
	public Inventory playerInventory;
	private List<UI_InventoryPanel> inventoryPanels = new List<UI_InventoryPanel>();
	[SerializeField]
	private GameObject InventoryPanelPrefab;
	[SerializeField]
	private GridLayoutGroup inventoryGridLayoutGroup;
	#endregion

	 void OnEnable()
    {
		//Inventory.OnItemAdded += handleOnItemAddedEvent;
		//Inventory.OnItemRemoved += handleOnItemRemovedEvent;
    }

	void OnDisable(){
		//Inventory.OnItemAdded -= handleOnItemAddedEvent;
		//Inventory.OnItemRemoved -= handleOnItemRemovedEvent;
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
	
	private void createInventoryUIPanels(){
		List<Item> inventoryItems = playerInventory.inventoryList;
		foreach(Item item in inventoryItems){
			GameObject newPanel = Instantiate(InventoryPanelPrefab, inventoryGridLayoutGroup.transform);
			UI_InventoryPanel inventoryPanel = newPanel.GetComponent<UI_InventoryPanel>();
			if(inventoryPanel !=  null){
				inventoryPanel.m_itemPanelName = item.name;
				inventoryPanel.m_item = item;
			}
			inventoryPanels.Add(inventoryPanel);
		}
	}

	private void cleanupInventoryUIPanels(){
		foreach(UI_InventoryPanel panel in inventoryPanels){
			Destroy(panel.gameObject);
			inventoryPanels.Remove(panel);
		}
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
		createInventoryUIPanels();
		isDisplaying = true;
		animator.SetBool("isDisplaying", true);
	}

	private void hideInventory(){
		isDisplaying = false;
		animator.SetBool("isDisplaying", false);
		cleanupInventoryUIPanels();
	}

	private void handleOnItemAddedEvent(Item itemAdded, Inventory inventory){
		//TODO Create Item inventory node
	}

	private void handleOnItemRemovedEvent(Item itemRemoved, Inventory inventory){
		//TODO Remove item inventory node
	}
}
