using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteractableSwitch: MonoBehaviour, IInteractable, ISwitchable {

    #region Parameters
    public bool isOn {
        get; set;
    }
    public bool isInteractable
    {
        get;set;
    }
    #endregion

    #region Components
    [SerializeField]
    private Component parentSwitchable;
    private ISwitchable switchableInterface;
    #endregion
    private void Start() {
        switchableInterface = parentSwitchable.GetComponent<ISwitchable>();
    }

    private void OnCollisionEnter(Collision other) {
        //Only do something if we've been hit by a character's hand
        if(other.gameObject.GetComponent<Hand>() != null){
            Interact(other.gameObject);
        }
    }

    public void Interact(GameObject interactor){
        toggle();
    }

    public void switchOn(){
        switchableInterface.switchOn();
    }

    public void switchOff(){
        switchableInterface.switchOff();
    }

    public void toggle(){
        switchableInterface.toggle();
    }
}