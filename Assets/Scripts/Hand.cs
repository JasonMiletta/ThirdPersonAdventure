using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hand : MonoBehaviour {

    #region State
    public bool grabbing = false;
    #endregion

    #region Components
    public GameObject grabbedObject;
    private FixedJoint joint;
    #endregion

	// Use this for initialization
	void Start () {
	}

    private void OnCollisionEnter(Collision other) {
        if(grabbing == true && grabbedObject == null){
            grabObject(other.gameObject);
        }
    }
	
	// Update is called once per frame
	void Update () {
		//TODO: If grab key is pressed, set grabbing = true;
        //grabbing = false;
	}

    public void beginGrabbing()
    {
        if (!grabbing)
        {
            GetComponent<Renderer>().material.color = Color.green;
            grabbing = true;
        }
    }

    public void stopGrabbing()
    {
        if (grabbing)
        {
            GetComponent<Renderer>().material.color = Color.white;
            dropObject();
        }
        grabbing = false;
    }

    void grabObject(GameObject objectToGrab){
        Grabbable grabbableComponent = objectToGrab.GetComponent<Grabbable>();
        Rigidbody grabbableRigidBody = objectToGrab.GetComponent<Rigidbody>();
        if (grabbableComponent != null && grabbableRigidBody != null)
        {
            if (grabbableComponent.isGrabbable)
            {
                createFixedJoint(grabbableRigidBody);
                //objectToGrab.transform.parent = this.transform;
                //objectToGrab.GetComponent<Rigidbody>().isKinematic = true;
                grabbedObject = objectToGrab;
            }
        }
    }

    void dropObject(){
        if(grabbedObject != null){
            destroyFixedJoint();
            //grabbedObject.transform.parent = null;
            //grabbedObject.GetComponent<Rigidbody>().isKinematic = false;
            grabbedObject = null;
        }
    }

    private void createFixedJoint(Rigidbody rigidBody)
    {
        joint = gameObject.AddComponent(typeof(FixedJoint)) as FixedJoint;
        joint.connectedBody = rigidBody;
    }

    private void destroyFixedJoint()
    {
        Destroy(joint);
    }
}
