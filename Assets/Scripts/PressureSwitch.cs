using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PressureSwitch: MonoBehaviour, ISwitchable {

    #region Parameters
    public bool isOn {
        get; set;
    }
    #endregion

    #region Components
    public GameObject specificObject;
    [SerializeField]
    private ISwitchable parentSwitchable;
    #endregion
    private void Start() {
    }

    private void OnCollisionEnter(Collision other) {
        if(other.gameObject == specificObject){
            switchOn();
        }
    }

    private void OnCollisionExit(Collision other) {
        if(other.gameObject == specificObject){
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