using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Switch: MonoBehaviour, IInteractable, ISwitch {

    #region Parameters
    public Bool isOn {
        get; set;
    }
    #endregion

    #region Components
    public GameObject Light;
    [SerializeField]
    private Material lightOnMaterial;
    [SerializeField]
    private Material lightOffMaterial;
    #endregion
    private void Start() {
    }

    private void OnCollisionEnter(Collision other) {
        //TODO: Check if the other collider is from a person's hand that is reached out
        interact(other.GameObject);
    }

    public void interact(GameObject interactor){
        if(isOn){
            switchOff();
        }else {
            switchOn();
        }
    }

    public void switchOn(){
        turnLightOn();
    }

    public void switchOff(){
        turnLightOff();
    }

    public void turnLightOn(){
        //TODO: Set emissive material property for gameobject
        isOn = true;
    }

    public void turnLightOff(){
        //TODO: revert emissive material property for gameObject
        isOn = false;
    }
}