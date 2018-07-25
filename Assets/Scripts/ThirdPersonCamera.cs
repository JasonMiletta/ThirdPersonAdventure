using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ThirdPersonCamera : MonoBehaviour {

	#region Parameters
	public bool lockCursor;
	public float mouseSensitivity = 10;
	public Transform target;
	public float dstFromTarget = 2;
	public Vector2 pitchMinMax = new Vector2 (-40, 85);

	public float rotationSmoothTime = .12f;
	#endregion
	
	#region State
	Vector3 rotationSmoothVelocity;
	Vector3 currentRotation;

	float yaw;
	float pitch;
	bool isInventoryOpen = false;
	#endregion

    /// <summary>
    /// This function is called when the object becomes enabled and active.
    /// </summary>
    void OnEnable()
    {
        UI_InventoryController.OnInventoryOpened += handleInventoryOpened;
        UI_InventoryController.OnInventoryClosed += handleInventoryClosed;
    }

    /// <summary>
    /// This function is called when the behaviour becomes disabled or inactive.
    /// </summary>
    void OnDisable()
    {
        UI_InventoryController.OnInventoryOpened -= handleInventoryOpened;
        UI_InventoryController.OnInventoryClosed -= handleInventoryClosed;
    }

	void Start() {
		if (lockCursor) {
			Cursor.lockState = CursorLockMode.Locked;
			Cursor.visible = false;
		}
	}

	void LateUpdate () {
		if(!isInventoryOpen){
			handleCameraInput();
		}

		pitch = Mathf.Clamp (pitch, pitchMinMax.x, pitchMinMax.y);

		currentRotation = Vector3.SmoothDamp (currentRotation, new Vector3 (pitch, yaw), ref rotationSmoothVelocity, rotationSmoothTime);
		transform.eulerAngles = currentRotation;

		transform.position = target.position - transform.forward * dstFromTarget;
	}

	private void handleCameraInput(){
		yaw += Input.GetAxis ("Mouse X") * mouseSensitivity;
		pitch -= Input.GetAxis ("Mouse Y") * mouseSensitivity;
	}

    private void handleInventoryOpened(){
        isInventoryOpen = true;
        Cursor.visible = true;
		Cursor.lockState = CursorLockMode.None;
    }
    
    private void handleInventoryClosed(){
        isInventoryOpen = false;
        Cursor.visible = false;
		Cursor.lockState = CursorLockMode.Locked;
    }
}
