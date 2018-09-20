using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UI_Player_Stat_Controller : MonoBehaviour {
	#region Components
	public Actor playerActor;
	public Text HealthText;
	public Text StaminaText;
	public Text HungerText;
	public Text ThirstText;
	#endregion
	#region State
	public float currentHealthValue;
	private float maxHealthValue;
	public float currentStaminaValue;
	private float maxStaminaValue;
	public float currentHungerValue;
	private float maxHungerValue;
	public float currentThirstValue;
	private float maxThirstValue;
	#endregion

	// Use this for initialization
	void Start () {
		if(playerActor == null){
			Debug.LogWarning("UI_Player_Stat_Controller doesn't have a reference to the player actor ");
		} else {
			initializeMaxValues();
		}
	}
	
	// Update is called once per frame
	void Update () {
		updateStatUI();
	}

	public void updateStatUI(){
		updateStatValues();

	}

	private void updateStatValues(){
		currentHealthValue = playerActor.currentHealth;
		currentStaminaValue = playerActor.currentStamina;
		currentHungerValue = playerActor.currentHunger;
		currentThirstValue = playerActor.currentThirst;
	}

	private void initializeMaxValues(){
		maxHealthValue = playerActor.totalHealth;
		maxStaminaValue = playerActor.maxStamina;
		maxHungerValue = playerActor.maxHunger;
		maxThirstValue = playerActor.maxThirst;
	}
}
