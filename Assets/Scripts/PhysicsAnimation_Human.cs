using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PhysicsAnimation_Human : MonoBehaviour {

	public enum AnimationState {Standing, Walking, Running, Jumping};


	[Header("Parameters")]
	#region Parameters
	public float standingForce = 1.0f;
	public float standingHeight = 1.0f;
	public float uprightTorque = 5f;
	public float forwardTorque = 5f;
	#endregion

	[Header("Components")]
	#region Components
	public Rigidbody hips;
	public Rigidbody spineMid;
	public Rigidbody leftKnee;
	public Rigidbody leftFoot;
	public Rigidbody rightKnee;
	public Rigidbody rightFoot;
	#endregion

	#region State
	public AnimationState currentAnimationState = AnimationState.Standing;
	public Vector3 currentFloorPosition;
	public Vector3 forwardFacingTargetVector;
	[SerializeField]
	private float stepSpeed;
	private float currentStepTime;
	private bool stepWithLeftLeg = false;
	[SerializeField]
	private float legStrength = 1.0f;
	#endregion
	
    #region GIZMOS
    List<Ray> raysToDraw = new List<Ray>();
    #endregion
    void OnDrawGizmos(){
        foreach(Ray r in raysToDraw){
            Gizmos.DrawRay(r);
        }
		raysToDraw = new List<Ray>();

		Gizmos.DrawWireSphere(getCurrentFloorPosition(), 0.1f);
		Gizmos.DrawRay(hips.transform.position, forwardFacingTargetVector);
    }

	// Use this for initialization
	void Start () {
		if(hips != null){
			forwardFacingTargetVector = hips.transform.forward;
		}
	}
	
	// Update is called once per frame
	void FixedUpdate () {
		Ray ray = new Ray (spineMid.transform.position, Vector3.down);
        RaycastHit hit;
		bool didHit = Physics.Raycast(ray, out hit);
		uprightRotationalCorrectionUpdate();
		forwardFacingRotationalCorrectionUpdate();
		standingUpdate(didHit, hit, spineMid.transform.position);
	
		if(currentAnimationState == AnimationState.Walking){
			walkingUpdate(didHit, hit, spineMid.transform.position);
		}
	}

	public void moveInDirection(Vector3 movementVector){
		if(movementVector == Vector3.zero){
			currentAnimationState = AnimationState.Standing;
		} else {
			currentAnimationState = AnimationState.Walking;
		}
	}
	
	private void standingUpdate(bool didHit, RaycastHit hit, Vector3 standingPosition){
		if(didHit){
            float proportionalHeight = (standingHeight - hit.distance) / standingHeight;
            Vector3 appliedStandingForce = Vector3.up * proportionalHeight * standingForce;
            spineMid.AddForceAtPosition(appliedStandingForce, standingPosition, ForceMode.Force);
            raysToDraw.Add(new Ray(standingPosition, appliedStandingForce));
		}
	}

	private void walkingUpdate(bool didHit, RaycastHit hit, Vector3 position){
		if(didHit){
			currentStepTime += Time.deltaTime;
			if(currentStepTime > stepSpeed){
				currentStepTime = 0.0f;
				stepWithLeftLeg = !stepWithLeftLeg;
			}
			Vector3 upForwardVector = new Vector3(0.0f, 1.0f, 1.0f * legStrength);
			Vector3 backVector =  -Vector3.forward * legStrength;
			if(stepWithLeftLeg){
				//Step With left leg
				leftKnee.AddForce(upForwardVector, ForceMode.Force);
				rightKnee.AddForce(backVector, ForceMode.Force);
				
				raysToDraw.Add(new Ray(leftKnee.transform.position, upForwardVector));
				raysToDraw.Add(new Ray(rightKnee.transform.position,backVector));
			} else {
				//Step With right leg
				rightKnee.AddForce(upForwardVector, ForceMode.Force);
				leftKnee.AddForce(backVector, ForceMode.Force);
				raysToDraw.Add(new Ray(rightKnee.transform.position, upForwardVector));
				raysToDraw.Add(new Ray(leftKnee.transform.position,backVector));
			}
		}
	}

	private void uprightRotationalCorrectionUpdate(){
		Vector3 floorNormal = Vector3.up;
		var rot = Quaternion.FromToRotation(spineMid.transform.up, floorNormal);
 		spineMid.AddTorque(new Vector3(rot.x, rot.y, rot.z)*uprightTorque);
	}

	private void forwardFacingRotationalCorrectionUpdate(){
		var hipsRotation = Quaternion.FromToRotation(hips.transform.forward, forwardFacingTargetVector);
 		hips.AddTorque(new Vector3(hipsRotation.x, hipsRotation.y, hipsRotation.z)*forwardTorque);
		 
		var spineMidRotation = Quaternion.FromToRotation(spineMid.transform.forward, forwardFacingTargetVector);
 		spineMid.AddTorque(new Vector3(spineMidRotation.x, spineMidRotation.y, spineMidRotation.z)*forwardTorque);
	}

	private Vector3 getCurrentFloorPosition(){
		return (leftFoot.transform.position + rightFoot.transform.position) / 2;
	}
}
