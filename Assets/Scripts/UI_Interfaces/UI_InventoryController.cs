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
	public UI_InventoryPreviewController inventoryPreview;
	private List<UI_InventoryPanel> inventoryPanels = new List<UI_InventoryPanel>();
	[SerializeField]
	private GameObject InventoryPanelPrefab;
	[SerializeField]
	private GridLayoutGroup inventoryGridLayoutGroup;

	#endregion

	#region Events
    public delegate void UIInventoryEvent();
    public static event UIInventoryEvent OnInventoryOpened;
    public static event UIInventoryEvent OnInventoryClosed;
	#endregion

	 void OnEnable(){
    }

	void OnDisable(){
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

	public void selectItem(Item item){
		inventoryPreview.showItem(item);
	}
	
	private void createInventoryUIPanels(){
		List<Item> inventoryItems = playerInventory.inventoryList;
		foreach(Item item in inventoryItems){
			InventoryPanelPrefab.GetComponent<UI_InventoryPanel>().m_item = item;
			GameObject newPanel = Instantiate(InventoryPanelPrefab, inventoryGridLayoutGroup.transform);
			UI_InventoryPanel inventoryPanel = newPanel.GetComponent<UI_InventoryPanel>();
			if(inventoryPanel !=  null){
				inventoryPanel.m_itemPanelName = item.name;
				inventoryPanel.m_item = item;
				inventoryPanel.inventoryController = this;
			}
			inventoryPanels.Add(inventoryPanel);
		}
	}

    private void handleInventoryInput(){
        if(CrossPlatformInputManager.GetButtonDown("Inventory")){
            if(isDisplaying){
				hideInventory();
				inventoryPreview.hideItem();
			} else {
				showInventory();
			}
        }
    }

	private void showInventory(){
		if(OnInventoryOpened != null){
			OnInventoryOpened();
		}
		createInventoryUIPanels();
		isDisplaying = true;
		animator.SetBool("isDisplaying", true);
	}

	private void hideInventory(){
		if(OnInventoryClosed != null){
			OnInventoryClosed();
		}
		isDisplaying = false;
		animator.SetBool("isDisplaying", false);
	}
}
