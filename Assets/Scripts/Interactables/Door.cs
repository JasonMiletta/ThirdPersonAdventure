using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Door: MonoBehaviour, ISwitchable {

    #region Parameters
    public bool isOn {
        get; set;
    }
    #endregion

    #region Components
    public GameObject door;
    #endregion
    private void Start() {
    }

    public void switchOn(){
        openDoor();
    }

    public void switchOff(){
        closeDoor();
    }

    public void toggle(){
        if(isOn){
            switchOn();
        } else {
            switchOff();
        }
    }

    public void openDoor(){
        //TODO: Set emissive material property for gameobject
        isOn = true;
    }

    public void closeDoor(){
        //TODO: revert emissive material property for gameObject
        isOn = false;
    }
}