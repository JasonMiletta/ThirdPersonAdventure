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
	public float forwardFacingTorque = 5f;
	#endregion

	[Header("Components")]
	#region Components
	public Rigidbody hips;
	public Rigidbody spineMid;
	public Rigidbody leftThigh;
	public Rigidbody leftKnee;
	public Rigidbody leftFoot;
	public Rigidbody leftHand;
	public Rigidbody rightThigh;
	public Rigidbody rightKnee;
	public Rigidbody rightFoot;
	public Rigidbody rightHand;
	#endregion

	#region State
	[Header("State")]
	public AnimationState currentAnimationState = AnimationState.Standing;
	public Vector3 currentFloorPosition;
	public Vector3 forwardFacingTargetVector;
	[SerializeField]
	private float walkSpeed = 1.0f;
	[SerializeField]
	private float stepPace;
	private float currentStepTime;
	private bool stepWithLeftLeg = false;
	[SerializeField]
	private float legStrength = 1.0f;
	#endregion
	
    #region GIZMOS
    List<Ray> raysToDraw = new List<Ray>();
	List<Vector3> spheresToDraw = new List<Vector3>();
    #endregion
    void OnDrawGizmos(){
        foreach(Ray r in raysToDraw){
            Gizmos.DrawRay(r);
        }
		raysToDraw = new List<Ray>();

		spheresToDraw.Add(getCurrentFloorPosition());
		foreach(Vector3 v in spheresToDraw){
			Gizmos.DrawWireSphere(v, 0.1f);			
		}
		spheresToDraw = new List<Vector3>();

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
		raysToDraw.Add(ray);
        RaycastHit hit;
		bool didHit = Physics.Raycast(ray, out hit, standingHeight, (1 << LayerMask.NameToLayer("Terrain")));
		spheresToDraw.Add(hit.point);
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

		forwardFacingTargetVector = movementVector;
		hips.AddForce(movementVector * walkSpeed, ForceMode.Acceleration);
	}
	
	private void standingUpdate(bool didHit, RaycastHit hit, Vector3 standingPosition){
		if(didHit){
			spheresToDraw.Add(hit.point);
            float proportionalHeight = (standingHeight - hit.distance) / standingHeight;
            Vector3 appliedStandingForce = Vector3.up * proportionalHeight * standingForce;
            spineMid.AddForce(appliedStandingForce, ForceMode.Force);
            raysToDraw.Add(new Ray(standingPosition, appliedStandingForce));
		}
	}

	private void walkingUpdate(bool didHit, RaycastHit hit, Vector3 position){
		if(didHit){
			currentStepTime += Time.deltaTime;
			if(currentStepTime > stepPace){
				currentStepTime = 0.0f;
				stepWithLeftLeg = !stepWithLeftLeg;
			}
			
			Vector3 upForwardVector = forwardFacingTargetVector * legStrength;
			Vector3 backVector =  -forwardFacingTargetVector * legStrength;
			if(stepWithLeftLeg){
				//Step With left leg
				leftKnee.AddForce(upForwardVector, ForceMode.Force);
				leftThigh.AddForce(upForwardVector, ForceMode.Force);
				leftHand.AddForce(backVector * 0.5f, ForceMode.Force);

				rightKnee.AddForce(backVector, ForceMode.Force);
				rightThigh.AddForce(backVector, ForceMode.Force);
				rightHand.AddForce(upForwardVector * 0.5f, ForceMode.Force);
				
				raysToDraw.Add(new Ray(leftKnee.transform.position, upForwardVector));
				raysToDraw.Add(new Ray(rightKnee.transform.position,backVector));
			} else {
				//Step With right leg
				rightKnee.AddForce(upForwardVector, ForceMode.Force);
				rightThigh.AddForce(upForwardVector, ForceMode.Force);
				rightHand.AddForce(backVector * 0.5f, ForceMode.Force);

				leftKnee.AddForce(backVector, ForceMode.Force);
				leftThigh.AddForce(backVector, ForceMode.Force);
				leftHand.AddForce(upForwardVector * 0.5f, ForceMode.Force);

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
 		hips.AddTorque(new Vector3(hipsRotation.x, hipsRotation.y, hipsRotation.z)*forwardFacingTorque);
		 
		var spineMidRotation = Quaternion.FromToRotation(spineMid.transform.forward, forwardFacingTargetVector);
 		spineMid.AddTorque(new Vector3(spineMidRotation.x, spineMidRotation.y, spineMidRotation.z)*forwardFacingTorque);
	}

	private Vector3 getCurrentFloorPosition(){
		return (leftFoot.transform.position + rightFoot.transform.position) / 2;
	}
}
