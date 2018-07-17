using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PhysicsAnimation_Human : MonoBehaviour {

	public enum AnimationState {Standing, Walking, Running, Jumping, Falling};


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
	[SerializeField]
	private float footStrength = 1.0f;
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
		currentAnimationState = AnimationState.Standing;
	}
	
	// Update is called once per frame
	void FixedUpdate () {
		Ray ray = new Ray (spineMid.transform.position, Vector3.down);
		raysToDraw.Add(ray);
        RaycastHit hit;
		//If we're falling, wait till the person hits the ground for extra goof
		float raycastDistance = currentAnimationState == AnimationState.Falling ? 0.5f : standingHeight;
		bool didHit = Physics.Raycast(ray, out hit, raycastDistance, (1 << LayerMask.NameToLayer("Terrain")));
		spheresToDraw.Add(hit.point);

		if(didHit){
			if(currentAnimationState == AnimationState.Falling){
				currentAnimationState = AnimationState.Standing;
			}
		} else {
			currentAnimationState = AnimationState.Falling;
		}

		
		if(currentAnimationState == AnimationState.Standing){

		}
		else if(currentAnimationState == AnimationState.Walking){
			walkingUpdate(didHit, hit, spineMid.transform.position);
		}
		
		if(currentAnimationState != AnimationState.Falling){
			forwardFacingRotationalCorrectionUpdate();
			standingUpdate(didHit, hit, spineMid.transform.position);
			uprightRotationalCorrectionUpdate();
		}
	}

	public void moveInDirection(Vector3 movementVector){
		if(currentAnimationState != AnimationState.Falling){
			if(movementVector == Vector3.zero){
				currentAnimationState = AnimationState.Standing;
			} else {
				currentAnimationState = AnimationState.Walking;
			}

			forwardFacingTargetVector = movementVector;
			hips.AddForce(movementVector * walkSpeed, ForceMode.Acceleration);
		}
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
			Vector3 forwardFootVector = forwardFacingTargetVector * footStrength;
			Vector3 backVector =  -forwardFacingTargetVector * legStrength;
			Vector3 backFootVector = -forwardFacingTargetVector * footStrength;


			if(stepWithLeftLeg){
				//Step With left leg
				leftFoot.AddForce(forwardFootVector, ForceMode.Force);
				leftKnee.AddForce(upForwardVector + (Vector3.up * 3), ForceMode.Force);
				leftThigh.AddForce(upForwardVector * 2.0f, ForceMode.Force);
				leftHand.AddForce(backVector, ForceMode.Force);

				rightFoot.AddForce(backFootVector, ForceMode.Force);
				rightKnee.AddForce(backVector, ForceMode.Force);
				rightThigh.AddForce(backVector, ForceMode.Force);
				rightHand.AddForce(upForwardVector, ForceMode.Force);
				
				raysToDraw.Add(new Ray(leftKnee.transform.position, upForwardVector));
				raysToDraw.Add(new Ray(rightKnee.transform.position,backVector));
			} else {
				//Step With right leg
				rightFoot.AddForce(forwardFootVector, ForceMode.Force);
				rightKnee.AddForce(upForwardVector + (Vector3.up * 3), ForceMode.Force);
				rightThigh.AddForce(upForwardVector * 2.0f, ForceMode.Force);
				rightHand.AddForce(backVector, ForceMode.Force);

				leftFoot.AddForce(backFootVector, ForceMode.Force);
				leftKnee.AddForce(backVector, ForceMode.Force);
				leftThigh.AddForce(backVector, ForceMode.Force);
				leftHand.AddForce(upForwardVector, ForceMode.Force);

				raysToDraw.Add(new Ray(rightKnee.transform.position, upForwardVector));
				raysToDraw.Add(new Ray(leftKnee.transform.position,backVector));
			}
		}
	}

	private void uprightRotationalCorrectionUpdate(){
		Vector3 appliedStandingForce = Vector3.zero;

		if(currentAnimationState == AnimationState.Standing){
			float xOffset = getCurrentFloorPosition().x - spineMid.transform.position.x;
			float zOffset = getCurrentFloorPosition().z - spineMid.transform.position.z;
			appliedStandingForce.x = xOffset * uprightTorque;
			appliedStandingForce.z = zOffset * uprightTorque;
			spineMid.AddForce(appliedStandingForce, ForceMode.Force);
		} else if(currentAnimationState == AnimationState.Walking){
		}
	}

	private void forwardFacingRotationalCorrectionUpdate(){
		var hipsRotation = Quaternion.FromToRotation(hips.transform.forward, forwardFacingTargetVector);
 		hips.AddTorque(new Vector3(hipsRotation.x, hipsRotation.y, hipsRotation.z)*forwardFacingTorque);
		 
		var spineMidRotation = Quaternion.FromToRotation(spineMid.transform.forward, forwardFacingTargetVector);
 		spineMid.AddTorque(new Vector3(spineMidRotation.x, spineMidRotation.y, spineMidRotation.z)*forwardFacingTorque);
	}

	private Vector3 getCurrentFloorPosition(){
		currentFloorPosition = (leftFoot.transform.position + rightFoot.transform.position) / 2;
		return currentFloorPosition;
	}
}
