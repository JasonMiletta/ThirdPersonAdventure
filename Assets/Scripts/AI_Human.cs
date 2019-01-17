using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AI_Human : MonoBehaviour {

    #region Info
    [Header("Parameters")]
	public Actor ActorToFollow;
	public float minFollowDistance = 5.0f;
	public float maxFollowDistance = 10.0f;
    #endregion

    #region Components
	private PhysicsAnimation_Human human;
    #endregion

    #region State
    private int currentHealth; 
    private Inventory inventory;
    #endregion
	// Use this for initialization
	void Start () {
		human = GetComponent<PhysicsAnimation_Human>();
	}
	
	// Update is called once per frame
	void Update () {
		followActor();
	}

	private void followActor(){
		if(ActorToFollow != null){
			float distanceToActor = Vector3.Distance(ActorToFollow.transform.position, transform.position);
			if(distanceToActor > minFollowDistance && distanceToActor <= maxFollowDistance){
				Vector3 directionToActor = ActorToFollow.transform.position - transform.position;
				
				human.moveInDirection(directionToActor.normalized, false);
				return;
			}
		}
		human.moveInDirection(Vector3.zero, false);
	}
}
