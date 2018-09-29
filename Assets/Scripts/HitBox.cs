using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HitBox : MonoBehaviour {

	#region Component
	public Item_Equipment parentEquipment;
	#endregion
	#region info
	private float attackStrength;
	#endregion

	// Use this for initialization
	void Start () {
		if(parentEquipment == null){
			parentEquipment = GetComponent<Item_Equipment>();
			if(parentEquipment == null){
				parentEquipment = GetComponentInParent<Item_Equipment>();
			}
		}

		if(parentEquipment != null){
			attackStrength = parentEquipment.AttackValue;
		} else {
			attackStrength = 0;
		}
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	/// <summary>
	/// OnCollisionEnter is called when this collider/rigidbody has begun
	/// touching another rigidbody/collider.
	/// </summary>
	/// <param name="other">The Collision data associated with this collision.</param>
	void OnCollisionEnter(Collision other)
	{
		Debug.Log("HitBox collision: " + other);
		handleActorCollision(other);
	}	

	private void handleActorCollision(Collision other){
		Actor opposingActor = other.gameObject.GetComponent<Actor>();
		if(opposingActor != null){
			opposingActor.applyIncomingDamage(attackStrength);
		}
	}
}
