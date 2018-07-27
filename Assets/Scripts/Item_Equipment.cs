using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Item_Equipment : MonoBehaviour {
	
	public enum Type {Weapon, OffHand, Helmet, TorsoArmor, ArmArmor, LegArmor};

	#region Info
	public float AttackValue;
	public float DefenseValue;
	public Type EquipmentType;

	#endregion
}
