using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hand : MonoBehaviour {

    #region State
    public bool grabbing = false;
    #endregion

    #region Components
    public GameObject grabbedObject;
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
        grabbing = false;
	}

    void grabObject(GameObject objectToGrab){
        Grabbable grabbableComponent = objectToGrab.GetComponent<Grabbable>();
        if (grabbableComponent != null)
        {
            if (grabbableComponent.isGrabbable)
            {
                objectToGrab.transform.parent = this.transform;
            }
        }
    }

    void dropObject(){
        if(grabbedObject != null){
            grabbedObject.transform.parent = null;
            grabbedObject = null;
        }
    }

}
