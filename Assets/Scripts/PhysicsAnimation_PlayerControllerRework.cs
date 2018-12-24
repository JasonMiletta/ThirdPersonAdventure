using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityStandardAssets.CrossPlatformInput;


[RequireComponent(typeof (PhysicsAnimation_HumanRework))]
public class PhysicsAnimation_PlayerControllerRework : MonoBehaviour {

    #region Components
	private PhysicsAnimation_HumanRework human;   //Human that we're controlling (This should be where all the physics go)
    private Actor m_actor;                  // Reference to the actor of the human we're controlling (This should be were all the state of the human go)
    private Transform m_Cam;                  // A reference to the main camera in the scenes transform
    private Vector3 m_CamForward;             // The current forward direction of the camera
    private Vector3 m_Move;
    private bool m_Jump;                      // the world-relative desired move direction, calculated from the camForward and user input.
    #endregion
    
	#region State
    [Header("State attributes")]
	List<Item> itemsInRange = new List<Item>();
    private bool isInventoryOpen = false;
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

	/// <summary>
	/// OnTriggerEnter is called when the Collider other enters the trigger.
	/// </summary>
	/// <param name="other">The other Collider involved in this collision.</param>
	void OnTriggerEnter(Collider other)
	{
		var Item = other.GetComponent<Item>();
		if(Item != null && !Item.isCurrentlyHeld){
			Item.displayUIPrompt();
			itemsInRange.Add(Item);
		}
	}

	/// <summary>
	/// OnTriggerExit is called when the Collider other has stopped touching the trigger.
	/// </summary>
	/// <param name="other">The other Collider involved in this collision.</param>
	void OnTriggerExit(Collider other)
	{
		var Item = other.GetComponent<Item>();
		if(Item != null){
			Item.hideUIPrompt();
			itemsInRange.Remove(Item);
		}
	}
    private void Start()
    {
        // get the transform of the main camera
        if (Camera.main != null)
        {
            m_Cam = Camera.main.transform;
        }
        else
        {
            Debug.LogWarning(
                "Warning: no main camera found. Third person character needs a Camera tagged \"MainCamera\", for camera-relative controls.", gameObject);
            // we use self-relative controls in this case, which probably isn't what the user wants, but hey, we warned them!
        }
		human = GetComponent<PhysicsAnimation_HumanRework>();
        m_actor = GetComponent<Actor>();
    }


    private void Update()
    {
        if(!isInventoryOpen){
            
            handleDropItem();
            handleInteract();
        }
    }


    // Fixed update is called in sync with physics
    private void FixedUpdate()
    {
        if(!isInventoryOpen){
            movement();
            handleAttackInput();
        }
    }

    private void movement(){
        // read inputs
        float h = CrossPlatformInputManager.GetAxis("Horizontal");
        float v = CrossPlatformInputManager.GetAxis("Vertical");
        bool crouch = Input.GetKey(KeyCode.C);

        // calculate move direction to pass to character
        if (m_Cam != null)
        {
            // calculate camera relative direction to move:
            m_CamForward = Vector3.Scale(m_Cam.forward, new Vector3(1, 0, 1)).normalized;
            m_Move = v*m_CamForward + h*m_Cam.right;
        }
        else
        {
            // we use world-relative directions in the case of no main camera
            m_Move = v*Vector3.forward + h*Vector3.right;
            m_Move = m_Move.normalized;
        }
        bool isRunning = false;
#if !MOBILE_INPUT
        // sprint speed multiplier
        if (Input.GetKey(KeyCode.LeftShift)) {
            isRunning = true;
        };
#endif


        if(Input.GetKey(KeyCode.Space)){
            human.stand();
        }
        // pass all parameters to the character control script
        human.moveInDirection(m_Move, isRunning);
        
        m_Jump = false;
    }


    private void handleInteract(){
        if(CrossPlatformInputManager.GetButtonDown("Grab") && this.hasItemInRange()){
            Item item = this.getClosestItemInRange();
            if(item.isLootable){
                if(m_actor.lootItem(item)){
                    itemsInRange.Remove(item);
                }
            }
        }
    }

    private void handleAttackInput(){
        if(CrossPlatformInputManager.GetButtonDown("Fire")){
            human.swingWithRightHand();
        }
        if(CrossPlatformInputManager.GetButtonDown("Fire2")){
            human.swingWithLeftHand();
        }
    }
    private void handleDropItem(){
        if(CrossPlatformInputManager.GetButtonDown("DropItem")){
            m_actor.dropItem();
        }
    }

	public bool hasItemInRange(){
		return itemsInRange.Count > 0;
	}

	public Item getClosestItemInRange(){
		Item closestItem = null;
		float shortestDistance = 1000f;
		foreach(Item item in itemsInRange){
			float distance = Vector3.Distance(transform.position, item.transform.position);
			if(distance < shortestDistance){
				closestItem = item;
				shortestDistance = distance; 
			}
		}
		return closestItem;
	}

    private void handleInventoryOpened(){
        isInventoryOpen = true;
    }
    
    private void handleInventoryClosed(){
        isInventoryOpen = false;
    }
}
