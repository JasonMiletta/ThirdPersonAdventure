using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteractableSwitch: MonoBehaviour, IInteractable, ISwitchable {

    #region Parameters
    public Bool isOn {
        get; set;
    }
    #endregion

    #region Components
    public ISwitchable parentSwitchable;
    #endregion
    private void Start() {
    }

    private void OnCollisionEnter(Collision other) {
        //Only do something if we've been hit by a character's hand
        if(other.GameObject.getComponent<Hand>() != null){
            interact(other.GameObject);
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