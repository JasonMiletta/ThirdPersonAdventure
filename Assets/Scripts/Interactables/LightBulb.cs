using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LightBulb: MonoBehaviour, ISwitchable {

    #region Parameters
    [SerializeField]
    public bool isOn {
        get; set;
    }
    #endregion

    #region Components
    [SerializeField]
    private Light lightComponent;
    [SerializeField]
    private Material lightOnMaterial;
    [SerializeField]
    private Material lightOffMaterial;
    private Renderer rendererComponent;

    #endregion
    private void Start() {
        lightComponent = GetComponent<Light>();
        rendererComponent = GetComponent<Renderer>();
    }

    public void switchOn(){
        turnLightOn();
    }

    public void switchOff(){
        turnLightOff();
    }

    public void toggle(){
        if(isOn)
        {
            switchOff();
        } else
        {
            switchOn();
        }
    }

    public void turnLightOn(){
        //TODO: Set emissive material property for gameobject
        isOn = true;
        rendererComponent.material = lightOnMaterial;
        lightComponent.enabled = true;
    }

    public void turnLightOff(){
        //TODO: revert emissive material property for gameObject
        isOn = false;
        rendererComponent.material = lightOffMaterial;
        lightComponent.enabled = false;
    }
}