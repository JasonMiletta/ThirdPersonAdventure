using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PhysicsAnimation_HumanRework : MonoBehaviour {

	public enum AnimationState {None, Standing, Walking, Running, Falling};


	[Header("Parameters")]
	#region Parameters
	public float standingForce = 1.0f;
	public float standingHeight = 1.0f;
	public float uprightTorque = 5f;
	public float forwardFacingTorque = 5f;
	public float attackStrength = 1.0f;
	#endregion

	[Header("Components")]
	#region Components
	public Rigidbody hips;
	public Rigidbody spineMid;
	public Rigidbody leftThigh;
	public Rigidbody leftKnee;
	public Rigidbody leftFoot;
	public Rigidbody leftArm;
	public Rigidbody leftForeArm;
	public Rigidbody leftHand;
	public Rigidbody rightThigh;
	public Rigidbody rightKnee;
	public Rigidbody rightFoot;
	public Rigidbody rightArm;
	public Rigidbody rightForeArm;
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
	private float runSpeed = 20.0f;
	[SerializeField]
	private float walkingStepPace;
	[SerializeField]
	private float runningStepPace;
	private float currentStepTime;
	private bool stepWithLeftLeg = false;
	[SerializeField]
	private float legStrength = 1.0f;
	[SerializeField]
	private float footStrength = 1.0f;
	[SerializeField]
	private float legBackSwingModifier = 0.5f;
	[SerializeField]
	private float armAnimStrength = 1.0f;
	[SerializeField]
	private float legExtensionTimingModifier = 0.7f;
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
			setForwardFacingTargetVector(hips.transform.forward);
		}
		currentAnimationState = AnimationState.Standing;
	}
	
	// Update is called once per frame
	void FixedUpdate () {

		if(currentAnimationState != AnimationState.None){
			Ray ray = new Ray (spineMid.transform.position, Vector3.down);
			raysToDraw.Add(ray);
			RaycastHit hit;

			float standingRaycastDistance = standingHeight * 2;
			bool didHit = Physics.Raycast(ray, out hit, standingRaycastDistance, (1 << LayerMask.NameToLayer("Terrain")));
			spheresToDraw.Add(hit.point);

			if(!didHit){
				currentAnimationState = AnimationState.Falling;
			}

			
			if(currentAnimationState == AnimationState.Standing){
				standStraightCorrectionUpdate();
			}
			else if(currentAnimationState == AnimationState.Walking || currentAnimationState == AnimationState.Running){
				walkingUpdate(didHit, hit, spineMid.transform.position);
			}
			
			if(currentAnimationState != AnimationState.Falling){
				forwardFacingRotationalCorrectionUpdate();
				standingUpdate(didHit, hit, spineMid.transform.position);
			}
		}
	}

	/// <summary>
	/// PlayerAction: Set the correct animationState. Add the necessary force to move the character based on player input. This doesn't 
	/// provide any additional force/animation beyond pushing the hips.
	///</summary>
	public void moveInDirection(Vector3 movementVector, bool isRunning){
		if(currentAnimationState != AnimationState.Falling){
			var modifiedMovementVector = movementVector;
			if(movementVector == Vector3.zero){
				currentAnimationState = AnimationState.Standing;
			} else if(isRunning){
				currentAnimationState = AnimationState.Running;
				modifiedMovementVector = movementVector * runSpeed;	
			} else {
				currentAnimationState = AnimationState.Walking;
				modifiedMovementVector = movementVector * walkSpeed;	
			}

			setForwardFacingTargetVector(modifiedMovementVector);
			hips.AddForce(modifiedMovementVector, ForceMode.Acceleration);
		}
	}

	public void stand(){
		if(currentAnimationState != AnimationState.Falling){
			currentAnimationState = AnimationState.Falling;
		} else {
			currentAnimationState = AnimationState.Standing;
		}
	}

	public void stopAllForces(){
		currentAnimationState = AnimationState.None;
	}

	/// <summary>
	/// This provides the necessary vertical force to keep the character standing at the correct height
	///</summary>
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
			var stepPace = walkingStepPace;
			if(currentAnimationState == AnimationState.Running){
				stepPace = runningStepPace;
			}
			currentStepTime += Time.deltaTime;
			bool extendLeg = false;
			if(currentStepTime > stepPace){
				currentStepTime = 0.0f;
				stepWithLeftLeg = !stepWithLeftLeg;
				extendLeg = false;
			} else if(currentStepTime > stepPace * legExtensionTimingModifier){
				extendLeg = true;
			}
			
			Vector3 upForwardVector = forwardFacingTargetVector * legStrength;
			Vector3 forwardFootVector = forwardFacingTargetVector * footStrength;
			Vector3 backVector =  -forwardFacingTargetVector * legStrength * 0.5f;
			Vector3 backFootVector = -forwardFacingTargetVector * footStrength *0.5f;


			Vector3 forwardRotationVector = Vector3.Cross(forwardFacingTargetVector, Vector3.up);

			if(stepWithLeftLeg){
				//Step forward With left leg
				leftThigh.AddRelativeTorque(Vector3.right * legStrength);
				
				//If on forward up swing of leg, bend the knee, otherwise we relax it to extend the leg and finish the forward step
				if(!extendLeg){
					leftKnee.AddRelativeTorque(-Vector3.right * legStrength * 0.5f);
				}
				//Contract foot inward
				leftFoot.AddRelativeTorque(Vector3.right * legStrength * 0.25f);


				//extend and swing leg back
				rightThigh.AddRelativeTorque(-Vector3.right * legStrength * legBackSwingModifier);
				//keep knee extended out
				rightKnee.AddRelativeTorque(Vector3.right * legStrength * legBackSwingModifier);
				//Extend foot out
				rightFoot.AddRelativeTorque(-Vector3.right * legStrength * legBackSwingModifier);

				//Swing opposite Arm forward 
				rightArm.AddRelativeTorque(-Vector3.forward * armAnimStrength);
				rightForeArm.AddRelativeTorque(-Vector3.forward * armAnimStrength * 2f);
				rightForeArm.AddForce(Vector3.up * armAnimStrength);
			} else {
				//Step With right leg
				rightThigh.AddRelativeTorque(Vector3.right * legStrength);
				
				//If on forward up swing of leg, bend the knee, otherwise we relax it to extend the leg and finish the forward step
				if(!extendLeg){
					rightKnee.AddRelativeTorque(-Vector3.right * legStrength * 0.5f);
				}
				//Contract foot inward
				rightFoot.AddRelativeTorque(Vector3.right * legStrength * 0.25f);


				//extend and swing leg back
				leftThigh.AddRelativeTorque(-Vector3.right * legStrength * legBackSwingModifier);
				//keep knee extended out
				leftKnee.AddRelativeTorque(Vector3.right * legStrength * legBackSwingModifier);
				//Extend foot out
				leftFoot.AddRelativeTorque(-Vector3.right * legStrength * legBackSwingModifier);

				//Swing opposite Arm forward
				leftArm.AddRelativeTorque(-Vector3.forward * armAnimStrength);
				leftForeArm.AddRelativeTorque(-Vector3.forward * armAnimStrength * 2f);
				leftForeArm.AddForce(Vector3.up * armAnimStrength);
			}
		}
	}

	/// <summary>
	///Grabs the current floor position (mid point between the current feet positions) and creates a horizontal force to keep 
	/// spineMid centered. Note: No upward force is created here, only x and z axis.
	///</summary>
	private void standStraightCorrectionUpdate(){
		Vector3 appliedStandingForce = Vector3.zero;

		float xOffset = getCurrentFloorPosition().x - spineMid.transform.position.x;
		float zOffset = getCurrentFloorPosition().z - spineMid.transform.position.z;
		appliedStandingForce.x = xOffset * uprightTorque;
		appliedStandingForce.z = zOffset * uprightTorque;
		spineMid.AddForce(appliedStandingForce);

		xOffset = getCurrentFloorPosition().x - hips.transform.position.x;
		zOffset = getCurrentFloorPosition().z - hips.transform.position.z;
		appliedStandingForce.x = xOffset * uprightTorque;
		appliedStandingForce.z = zOffset * uprightTorque;
		hips.AddForce(appliedStandingForce);
	}

	/// <summary>
	/// Rotates the spineMid and hips to attempt to face the forwardFacingTargetVector (The last movement direction of the character)
	///</summary>
	private void forwardFacingRotationalCorrectionUpdate(){
		var hipsRotation = Quaternion.FromToRotation(hips.transform.forward, forwardFacingTargetVector);
 		hips.AddTorque(new Vector3(hipsRotation.x, hipsRotation.y, hipsRotation.z)*forwardFacingTorque);
		 
		var spineMidRotation = Quaternion.FromToRotation(spineMid.transform.forward, forwardFacingTargetVector);
 		spineMid.AddTorque(new Vector3(spineMidRotation.x, spineMidRotation.y, spineMidRotation.z)*forwardFacingTorque);
	}

	/// <summary>
	/// Grabs the current midpoint between the left and right foot
	///</summary>
	private Vector3 getCurrentFloorPosition(){
		currentFloorPosition = (leftFoot.transform.position + rightFoot.transform.position) / 2;
		return currentFloorPosition;
	}

	private void setForwardFacingTargetVector(Vector3 forwardFacingVector){
		forwardFacingTargetVector = forwardFacingVector.normalized;
	}

    public IEnumerator smoothForceToPosition(Rigidbody rigidBody, Dictionary<Vector3, float> destinationToSpeedMap, float strength){
        foreach(Vector3 destination in destinationToSpeedMap.Keys){
            float duration;
            destinationToSpeedMap.TryGetValue(destination, out duration);
            yield return StartCoroutine(smoothForceToPositionCoroutine(rigidBody, rigidBody.transform.position, destination, strength, duration));
        }
    }
    public IEnumerator smoothForceToPositionCoroutine(Rigidbody rigidBody, Vector3 source, Vector3 destination, float strength, float duration){
        float startTime = Time.time;
        while(Time.time < startTime + duration){
            rigidBody.AddForce((destination - source) * strength, ForceMode.Force);
            yield return null;
        }

        yield return null;
    }

	public void addRayToDraw(Vector3 start, Vector3 end){
		Ray ray = new Ray (start, end);
		raysToDraw.Add(ray);
	}
}

