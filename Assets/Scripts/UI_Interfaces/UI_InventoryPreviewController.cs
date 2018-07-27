using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UI_InventoryPreviewController : MonoBehaviour {

	#region Info
	[Header("Info")]
	public string m_itemPanelName;
	public Item m_item;
	#endregion

	#region State
	public bool isDisplaying = false;
	#endregion

	#region Components
	[Header("Components")]
	public Text ItemTitleComponent;
	public Image ItemModelComponent;
	public Transform ItemPreviewTransform;
	private Animator animator;
	#endregion

	// Use this for initialization
	void Start () {
		animator = GetComponent<Animator>();
		ItemTitleComponent = GetComponentInChildren<Text>();
		ItemModelComponent = GetComponentInChildren<Image>();

		if(ItemTitleComponent != null && m_item != null){
			ItemTitleComponent.text = m_item.displayName;
		}
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	public void showItem(Item item){
		if(m_item != null){
			hidePreviewPanel();
			cleanupCurrentItem();
		}
		ItemTitleComponent.text = item.displayName;
		item.gameObject.transform.parent = ItemPreviewTransform;
		item.gameObject.transform.localPosition = Vector3.zero;
		item.gameObject.transform.localEulerAngles = Vector3.zero;
		item.gameObject.transform.localScale = Vector3.one;
		m_item = item;

		showPreviewPanel();
	}

	public void hideItem(){
		hidePreviewPanel();
	}

	private void cleanupCurrentItem(){
		m_item.gameObject.transform.localPosition += Vector3.forward;
		m_itemPanelName = null;
		m_item = null;
	}

	private void showPreviewPanel(){
		isDisplaying = true;
		animator.SetBool("isDisplaying", true);
	}

	private void hidePreviewPanel(){
		isDisplaying = false;
		animator.SetBool("isDisplaying", false);
	}
}
