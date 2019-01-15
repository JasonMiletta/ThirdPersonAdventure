using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Light: MonoBehaviour, ISwitchable {

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

    public void switchOn(){
        turnLightOn();
    }

    public void switchOff(){
        turnLightOff();
    }

    public void toggle(){
        if(isOn){
            switchOn();
        } else {
            switchOff();
        }
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