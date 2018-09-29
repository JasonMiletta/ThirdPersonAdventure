using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Actor))]
public class Actor_Equipment : MonoBehaviour {

	#region Info
	[Header("Info")]
	public Item_Equipment Helmet;
	public Item_Equipment Torso;
	public Item_Equipment LeftArm;
	public Item_Equipment LeftHand;
	public Item_Equipment RightArm;
	public Item_Equipment RightHand;
	public Item_Equipment Legs;
	#endregion

	#region Components
	private Actor m_ParentActor;
	[Header("Body Placement Transforms")]
	[SerializeField]
	private Transform m_HeadTransform;
	[SerializeField]
	private Transform m_TorsoTransform;
	[SerializeField]
	private Transform m_LeftArmTransform;
	[SerializeField]
	private Transform m_LeftHandTransform;
	[SerializeField]
	private Transform m_RightArmTransform;
	[SerializeField]
	private Transform m_RightHandTransform;
	[SerializeField]
	private Transform m_LegsTransform;
	#endregion

	// Use this for initialization
	void Start () {
		m_ParentActor = GetComponentInParent<Actor>(); 
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	/// <summary>
	///Attempts to equip the provided item into the placement slot it belongs in. Return false if item is already equipped there
	///</summary>
	public bool equipItem(Item equipmentItem){
		Item_Equipment equipment = equipmentItem.getItemEquipment();
		Item_Equipment.Placement placement = equipment.EquipmentPlacement;
		if(placement == Item_Equipment.Placement.Head){
			if(Helmet != null){
				return false;
			} else {
				Helmet = equipment;
				equipment.transform.localScale = Vector3.one;
				equipment.gameObject.transform.parent = m_HeadTransform;
				equipment.transform.localPosition = Vector3.zero;
				return true;
			}
		} else if(placement == Item_Equipment.Placement.Torso){
			if(Torso != null){
				return false;
			} else {
				Torso = equipment;
				equipment.transform.localScale = Vector3.one;
				equipment.gameObject.transform.parent = m_HeadTransform;
				equipment.transform.localPosition = Vector3.zero;
				return true;
			}
			
		} else if(placement == Item_Equipment.Placement.Arm){
			if(RightArm == null){
				RightArm = equipment;
				equipment.transform.localScale = Vector3.one;
				equipment.gameObject.transform.parent = m_RightArmTransform;
				equipment.transform.localPosition = Vector3.zero;
			}else if(LeftArm == null){
				LeftArm = equipment;
				equipment.transform.localScale = Vector3.one;
				equipment.gameObject.transform.parent = m_LeftArmTransform;
				equipment.transform.localPosition = Vector3.zero;
			} else {
				return false;
			}
			return true;

		} else if(placement == Item_Equipment.Placement.Leg){
			if(Legs != null){
				return false;
			} else {
				Legs = equipment;
				equipment.transform.localScale = Vector3.one;
				equipment.gameObject.transform.parent = m_LegsTransform;
				equipment.transform.localPosition = Vector3.zero;
				return true;
			}

		} else if(placement == Item_Equipment.Placement.MainHand){
			if(RightHand != null){
				return false;
			} else {
				RightHand = equipment;
				equipment.transform.localScale = Vector3.one;
				equipment.gameObject.transform.parent = m_RightHandTransform;
				equipment.transform.localPosition = Vector3.zero;
				equipment.transform.localRotation = m_RightHandTransform.rotation;
				return true;
			}

		} else if(placement == Item_Equipment.Placement.OffHand){
			if(LeftHand != null){
				return false;
			} else {
				LeftHand = equipment;
				equipment.transform.localScale = Vector3.one;
				equipment.gameObject.transform.parent = m_LeftHandTransform;
				equipment.transform.localPosition = Vector3.zero;
				return true;
			}
		}
		
		Debug.LogWarning("This Item_Equipment doesn't have a specified placement: " + equipment);
		return false;
	}

	public bool unEquipItem(Item_Equipment.Placement placement){
		
		return false;
	}

	public bool unEquipItem(Item item){
		if(m_ParentActor.getInventoryTransform() != null){
			item.bringToInventory(m_ParentActor.getInventoryTransform().gameObject);
			return true;
		} else {
			item.transform.parent = null;
		}
		return false;
	}
	public bool unEquipHelmet(){
		if(Helmet != null){
			Item helmetItem = Helmet.GetComponent<Item>();
			Helmet = null;
			return true;
		}
		return false;
	}

	private void moveItemToInventory(Item item){
		Transform inventoryTransform = GetComponent<Actor>().getInventoryTransform();
		if(inventoryTransform != null){
			item.gameObject.transform.parent = inventoryTransform;
		} else {
			item.gameObject.transform.parent = null;
		}
	}
}
