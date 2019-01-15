using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PressureSwitch: MonoBehaviour, ISwitchable {

    #region Parameters
    public Bool isOn {
        get; set;
    }
    #endregion

    #region Components
    public GameObject specificObject;
    public ISwitchable parentSwitchable;
    #endregion
    private void Start() {
    }

    private void OnCollisionEnter(Collision other) {
        if(other.GameObject == specificObject){
            switchOn();
        }
    }

    private void OnCollisionExit(Collision other) {
        if(other.GameObject == specificObject){
            switchOff();
        }
    }

    public void interact(GameObject interactor){
        toggle();
    }

    public void switchOn(){
        parentSwitchable.switchOn();
    }

    public void switchOff(){
        parentSwitchable.switchOff();
    }

    public void toggle(){
        parentSwitchable.toggle();
    }
}