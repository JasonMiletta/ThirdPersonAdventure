using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PhysicsAnimation_Human : MonoBehaviour {

	public enum AnimationState {None, Standing, Walking, Running, Falling, Jumping, Sitting};


	[Header("Value Parameters")]
	#region ValueParameters
	[SerializeField]
	private float walkSpeed = 10.0f;
	[SerializeField]
	private float runSpeed = 15.0f;
	[SerializeField]
	private float lookFocusRange = 2.5f;
	#endregion
	
	[Header("Physics Animation Forces")]
	#region AnimationForces
	[SerializeField]
	private float standingForce = 10000.0f;
    [SerializeField]
	private float defaultStandingHeight = 1.4f;
    [SerializeField]
	private float runningStandingHeight = 1.45f;
    
    [SerializeField]
	private float uprightTorque = 500f;
	
    [SerializeField]
	private float forwardFacingTorque = 500f;
	[SerializeField]
	private float lookTorqueInfluence = 1f;
	[SerializeField]
	private float reachForwardStrength = 5.0f;
	
    [SerializeField]
	private float attackStrength = 1.0f;
	
    [SerializeField]
	private float jumpStrength = 2.0f;
	[SerializeField]
	private float walkingStepPace = 0.55f;
	[SerializeField]
	private float walkingLegExtensionTimingModifier = 0.7f;
	[SerializeField]
	private float runningStepPace = 0.6f;
	[SerializeField]
	private float runningLegExtensionTimingModifier = 0.7f;
	[SerializeField]
	private float legStrength = 10000.0f;
	[SerializeField]
	private float footStrength = 1000.0f;
	[SerializeField]
	private float legBackSwingModifier = 0.5f;
	[SerializeField]
	private float armAnimStrength = 100.0f;
	[SerializeField]
	private float footStepUpwardForceStrength = 1.0f;
	#endregion

	[Header("Components")]
	#region Components
	[SerializeField]
	private Transform FrontActionTargetTransform;
	[SerializeField]
	private Transform RightActionTargetTransform;
	[SerializeField]
	private Transform LeftActionTargetTransform;
	public GameObject grabbedObject;
	#endregion

	[Header("Limb RigidBody Components")]
	#region RigidBodyComponents
	[SerializeField]
	private Rigidbody head;
    [SerializeField]
	private Rigidbody hips;
    [SerializeField]
	private Rigidbody spineMid;
    [SerializeField]
	private Rigidbody leftThigh;
    [SerializeField]
	private Rigidbody leftKnee;
    [SerializeField]
	private Rigidbody leftFoot;
    [SerializeField]
	private Rigidbody leftArm;
    [SerializeField]
	private Rigidbody leftForeArm;
    [SerializeField]
	private Rigidbody leftHand;
    [SerializeField]
	private Rigidbody rightThigh;
    [SerializeField]
	private Rigidbody rightKnee;
    [SerializeField]
	private Rigidbody rightFoot;
    [SerializeField]
	private Rigidbody rightArm;
    [SerializeField]
	private Rigidbody rightForeArm;
    [SerializeField]
	private Rigidbody rightHand;
	#endregion

	#region State
	[Header("State")]
	public AnimationState currentAnimationState = AnimationState.Standing;
	private Vector3 currentFloorPosition;
	public Vector3 forwardFacingTargetVector;
	private float currentStepTime;
	private bool stepWithLeftLeg = false;
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

        Vector3 floorPosition = getCurrentFloorPosition();
        if (floorPosition != null)
        {
            spheresToDraw.Add(floorPosition);
        }

		foreach(Vector3 v in spheresToDraw){
			Gizmos.DrawWireSphere(v, 0.1f);			
		}
		spheresToDraw = new List<Vector3>();
        if (hips != null)
        {
            Gizmos.DrawRay(hips.transform.position, forwardFacingTargetVector);
        }
    }

	/// <summary>
	/// OnGUI is called for rendering and handling GUI events.
	/// This function can be called multiple times per frame (one call per event).
	/// </summary>
	void OnGUI()
	{
		GUILayout.Label(leftFoot.velocity.ToString());
		GUILayout.Label(rightFoot.velocity.ToString());
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
			/* Raycast to the ground to see if we're falling or where our feet should be */
			Ray ray = new Ray (spineMid.transform.position, Vector3.down);
			raysToDraw.Add(ray);
			RaycastHit hit;

			float standingRaycastDistance = defaultStandingHeight * 2;
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
				GameObject objectToFocus = determineObjectToFocus();
				rotateHeadToFocusedObject(objectToFocus);
			}

		}
	}

	#region Actions
	

	public void stand(){
		if(currentAnimationState != AnimationState.Falling){
			currentAnimationState = AnimationState.Falling;
		} else {
			currentAnimationState = AnimationState.Standing;
		}
	}

	/// <summary>
	/// PlayerAction: Fire off the necessary force to make the character jump.
	/// </summary>
	public void jump(){
        //TODO: Rework this to function similar to standing update rather than wonky impulses
		currentAnimationState = AnimationState.Falling;
		spineMid.AddForce(Vector3.up * jumpStrength, ForceMode.Impulse);
		hips.AddForce(Vector3.up * jumpStrength, ForceMode.Impulse);

		if(stepWithLeftLeg){
			rightKnee.AddForce(forwardFacingTargetVector * jumpStrength, ForceMode.Impulse);
		} else {
			leftKnee.AddForce(forwardFacingTargetVector * jumpStrength, ForceMode.Impulse);
		}
	}

	/// <summary>
	/// PlayerAction: update currentAnimationState to sitting. Forces to simulate sitting handled by sittingUpdate
	/// </summary>
	public void sit(){
		if(currentAnimationState == AnimationState.Standing){
			currentAnimationState = AnimationState.Sitting;
		} else if(currentAnimationState == AnimationState.Sitting){
			currentAnimationState = AnimationState.Standing;
		}
	}

    public void beginGrabbing()
    {
        leftHand?.GetComponent<Hand>()?.beginGrabbing();
        rightHand?.GetComponent<Hand>()?.beginGrabbing();
    }

    public void stopGrabbing()
    {
        leftHand?.GetComponent<Hand>()?.stopGrabbing();
        rightHand?.GetComponent<Hand>()?.stopGrabbing();
    }

	public void reachWithLeftHand(Vector3 reachTargetVector){
        reachTargetVector = reachTargetVector * .75f;
        var armsRotation = Quaternion.FromToRotation(leftArm.transform.forward, reachTargetVector);

        leftArm.AddRelativeTorque(-Vector3.forward * reachForwardStrength,ForceMode.Force);
        leftHand.AddForce(reachTargetVector * reachForwardStrength);
    }

    public void reachWithRightHand(Vector3 reachTargetVector)
    {
        reachTargetVector = reachTargetVector * .75f;
        var armsRotation = Quaternion.FromToRotation(rightArm.transform.forward, reachTargetVector);

        rightArm.AddRelativeTorque(-Vector3.forward * reachForwardStrength,ForceMode.Force);
        rightHand.AddForce(reachTargetVector * reachForwardStrength);
    }

	

	/// <summary>
	/// PlayerAction send series of force inputs to swing right hand
	/// </summary>
	public void swingWithRightHand(){
		//TODO Enable dangerous collisions with right hand and start swinging
		Dictionary<Vector3, float> animationDestinationMap = new Dictionary<Vector3, float>();
		animationDestinationMap.Add(RightActionTargetTransform.position, 0.1f);
		animationDestinationMap.Add(FrontActionTargetTransform.position, 0.25f);
		StartCoroutine(smoothForceToPosition(rightHand, animationDestinationMap, attackStrength));
		//TODO: Disable dangerous collisions with right hand when done
	}


	/// <summary>
	/// PlayerAction send series of force inputs to swing left hand
	/// </summary>
	public void swingWithLeftHand(){
		//TODO Enable dangerous collisions with hand and start swinging
		Dictionary<Vector3, float> animationDestinationMap = new Dictionary<Vector3, float>();
		animationDestinationMap.Add(LeftActionTargetTransform.position, 0.1f);
		animationDestinationMap.Add(FrontActionTargetTransform.position, 0.25f);
		StartCoroutine(smoothForceToPosition(leftHand, animationDestinationMap, attackStrength));
		//TODO: Disable dangerous collisions with hand when done
	}
	#endregion

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
			spineMid.AddForce(modifiedMovementVector, ForceMode.Acceleration);
		}
	}

	#region updateFunctions
	public void stopAllForces(){
		currentAnimationState = AnimationState.None;
	}

	/// <summary>
	/// This provides the necessary vertical force to keep the character standing at the correct height
	///</summary>
	private void standingUpdate(bool didHit, RaycastHit hit, Vector3 standingPosition){
		if(didHit){
			spheresToDraw.Add(hit.point);

            float standingHeight = currentAnimationState == AnimationState.Running ? runningStandingHeight : defaultStandingHeight;
            float proportionalHeight = (standingHeight - hit.distance) / standingHeight;
            Vector3 appliedStandingForce = Vector3.up * proportionalHeight * standingForce;

            spineMid.AddForce(appliedStandingForce * .5f, ForceMode.Force);
            head.AddForce(appliedStandingForce * .5f, ForceMode.Force);
            raysToDraw.Add(new Ray(standingPosition, appliedStandingForce));
		}
	}

	private void walkingUpdate(bool didHit, RaycastHit hit, Vector3 position){
		if(didHit){
			var legExtensionTimingModifier = walkingLegExtensionTimingModifier;
			var stepPace = walkingStepPace;
			if(currentAnimationState == AnimationState.Running){
				stepPace = runningStepPace;
				legExtensionTimingModifier = runningLegExtensionTimingModifier;
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

			Debug.Log(leftFoot.velocity);
			Debug.Log(rightFoot.velocity);
			if(leftFoot.velocity == Vector3.zero || rightFoot.velocity == Vector3.zero){
				Debug.Log("Step UP!");
				hips.AddForce(Vector3.up * footStepUpwardForceStrength);
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
 		hips.AddTorque(new Vector3(hips.rotation.x, hipsRotation.y, hips.rotation.z)*forwardFacingTorque);
		 
		var spineMidRotation = Quaternion.FromToRotation(spineMid.transform.forward, forwardFacingTargetVector);
 		spineMid.AddTorque(new Vector3(spineMidRotation.x, spineMidRotation.y, spineMidRotation.z)*forwardFacingTorque);
	}

	private GameObject determineObjectToFocus(){
		//TODO: We'll want to have a priority heirarchy, including any nearby switches, grabbable objects, doors, breakable actors, or actors
		// get a normal for the surface that is being touched to move along it
            RaycastHit hitInfo;
			//TODO: Raycast in an area around us for any objects that we want to focus on. Find the closest one and return that object.
            Physics.SphereCast(transform.position, lookFocusRange, Vector3.up, out hitInfo,
                               1f, Physics.AllLayers);

		return null;
	}

	private void rotateHeadToFocusedObject(GameObject focusedObject){
		if(focusedObject != null){
			var headRotation = Quaternion.FromToRotation(head.transform.forward, focusedObject.transform.position);
			head.AddTorque(new Vector3(headRotation.x, headRotation.y, head.rotation.y) * lookTorqueInfluence);
		}
	}
	#endregion

	#region Utilities
	/// <summary>
	/// Grabs the current midpoint between the left and right foot
	///</summary>
	private Vector3 getCurrentFloorPosition(){
        currentFloorPosition = Vector3.zero;
        if (leftFoot != null && rightFoot != null) {
            currentFloorPosition = (leftFoot.transform.position + rightFoot.transform.position) / 2;
        }
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
	#endregion
}

