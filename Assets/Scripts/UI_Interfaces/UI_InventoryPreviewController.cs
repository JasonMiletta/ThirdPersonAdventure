using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UI_InventoryPreviewController : MonoBehaviour {

	#region Info
	[Header("Info")]
	public string m_itemPanelName;
	public Item m_item;
	public GameObject m_itemPreviewClone;
	#endregion

	#region State
	public bool isDisplaying = false;
	#endregion

	#region Components
	[Header("Components")]
	public Text ItemTitleComponent;
	public Image ItemModelComponent;
	public Transform ItemPreviewTransform;
	[SerializeField]
	private Button EquipButton;
	[SerializeField]
	private Button UnEquipButton;
	private Animator animator;
	private UI_InventoryController InventoryController;
	#endregion

	// Use this for initialization
	void Start () {
		InventoryController = GetComponentInParent<UI_InventoryController>();
		if(InventoryController == null){
			Debug.LogWarning("UI_InventoryPreviewController is missing UI_InventoryController in it's parent!");
		}
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
		m_item = item;
		m_itemPreviewClone = Instantiate(m_item.gameObject, ItemPreviewTransform);
		ItemTitleComponent.text = item.displayName;
		m_itemPreviewClone.transform.localPosition = Vector3.zero;
		m_itemPreviewClone.transform.localEulerAngles = Vector3.zero;
		m_itemPreviewClone.transform.localScale = Vector3.one;
		setEquipButtonState();

		showPreviewPanel();
	}

	public void hideItem(){
		hidePreviewPanel();
	}

	public void equipCurrentItem(){
		bool success = InventoryController.equipItem(m_item);
		if(success){
			
		}
		setEquipButtonState();
	}

	public void unequipCurrentItem(){
		InventoryController.unequipItem(m_item);
	}

	private void cleanupCurrentItem(){
		m_item.gameObject.transform.localPosition += Vector3.forward;
		m_itemPanelName = null;
		m_item = null;
		Destroy(m_itemPreviewClone);
	}

	private void showPreviewPanel(){
		isDisplaying = true;
		animator.SetBool("isDisplaying", true);
	}

	private void hidePreviewPanel(){
		isDisplaying = false;
		animator.SetBool("isDisplaying", false);
	}

	private void setEquipButtonState(){
		if(m_item != null){
			if(m_item.isEquipable){
				if(m_item.isCurrentlyEquipped){
					EquipButton.gameObject.SetActive(false);
					UnEquipButton.gameObject.SetActive(true);
				} else {
					EquipButton.gameObject.SetActive(true);
					UnEquipButton.gameObject.SetActive(false);
				}
			}else {
				EquipButton.gameObject.SetActive(false);
				UnEquipButton.gameObject.SetActive(false);
			}
		}
	}
}
