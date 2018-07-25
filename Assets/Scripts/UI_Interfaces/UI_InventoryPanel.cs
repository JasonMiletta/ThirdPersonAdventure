using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UI_InventoryPanel : MonoBehaviour {

	#region Info
	[Header("Info")]
	public string m_itemPanelName;
	public Item m_item;
	#endregion

	#region Components
	[Header("Components")]
	public Text PanelTitleComponent;
	public Image PanelIconComponent;
	public int PanelItemCount;
	public UI_InventoryController inventoryController;
	#endregion

	/// <summary>
	/// This function is called when the behaviour becomes disabled or inactive.
	/// </summary>
	void OnDisable()
	{
		Destroy(this.gameObject);
	}

	// Use this for initialization
	void Start () {
		PanelTitleComponent = GetComponentInChildren<Text>();
		PanelIconComponent = GetComponentInChildren<Image>();

		if(PanelTitleComponent != null){
			PanelTitleComponent.text = m_item.displayName;
		}
		if(inventoryController == null){
			inventoryController = GetComponentInParent<UI_InventoryController>();
		}
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	public void selectItem(){
		inventoryController.selectItem(m_item);
	}
}
