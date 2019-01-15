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
            grabObject(other.GameObject);
        }
    }
	
	// Update is called once per frame
	void Update () {
		//TODO: If grab key is pressed, set grabbing = true;
        grabbing = false;
	}

    void grabObject(GameObject objectToGrab){
        if(objectToGrab.GetComponent<Grabbable>()?.isGrabbable){
            objectToGrab.parent = this.transform;
        }
    }

    void dropObject(){
        if(grabbedObject != null){
            grabbedObject.parent = null;
            grabbedObject = null;
        }
    }

}
