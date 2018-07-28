using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent (typeof(Item))]
public class Item_Equipment : MonoBehaviour {
	
	public enum Type {Weapon, OffHand, Armor};
	public enum Placement {MainHand, OffHand, Head, Torso, Arm, Leg};

	#region Info
	public float AttackValue;
	public float DefenseValue;
	public Type EquipmentType;
	public Placement EquipmentPlacement;

	#endregion
}
