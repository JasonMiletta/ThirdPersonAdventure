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
	#endregion

	// Use this for initialization
	void Start () {
		PanelTitleComponent = GetComponent<Text>();
		PanelIconComponent = GetComponent<Image>();

		if(PanelTitleComponent != null){
			PanelTitleComponent.text = m_itemPanelName;
		}
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
