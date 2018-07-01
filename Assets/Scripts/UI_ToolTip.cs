﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof (Animator))]
public class UI_ToolTip : MonoBehaviour {

	[Header("Parameters")]
	#region Parameters
	public float distanceToDisplayTooltip = 2.0f;
	public Vector3 displayPositionOffset = new Vector3(0.0f, 1.0f, 0.0f);
	#endregion

	#region State
	private bool isDisplaying = false;
	#endregion

	#region Components
	private Transform parentObjectTransform;
	private Animator animator;
	private Transform playerPosition;
	#endregion

	// Use this for initialization
	void Start () {
		parentObjectTransform = transform.parent;
		animator = GetComponent<Animator>();
		playerPosition = GameObject.FindWithTag("Player").transform;
	}
	
	// Update is called once per frame
	void Update () {
		transform.position = parentObjectTransform.position + displayPositionOffset;
		if(playerPosition != null){
			
			#if UNITY_EDITOR
			// helper to visualise the ground check ray in the scene view
			Debug.DrawLine(parentObjectTransform.position, playerPosition.position, Color.grey);
			#endif

			//if the player is close enough to display the tool tip
			if(Mathf.Abs(Vector3.Distance(playerPosition.position, parentObjectTransform.position)) <= distanceToDisplayTooltip){
				if(!isDisplaying){
					displayTooltip();
				}
			} else {
				if(isDisplaying){
					hideTooltip();
				}
			}
		}
	}

	public void displayTooltip(){
		isDisplaying = true;
		animator.SetBool("isDisplaying", true);
	}

	public void hideTooltip(){
		isDisplaying = false;
		animator.SetBool("isDisplaying", false);
	}
}
