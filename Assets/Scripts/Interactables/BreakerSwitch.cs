using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BreakerSwitch: MonoBehaviour{

    #region Parameters
    [SerializeField]
    public bool isEnabled {
        get; set;
    }
    #endregion
    #region Components
    [SerializeField]
    private Component parentSwitchable;
    private ISwitchable parentSwitchableInterface;
    [SerializeField]
    private List<Component> switchList;
    private Dictionary<Component, ISwitchable> switchInterfaceDictionary = new Dictionary<Component, ISwitchable>();
    #endregion
    private void Start() {
        isEnabled = true;
        parentSwitchableInterface = parentSwitchable.GetComponent<ISwitchable>();
        foreach(Component switchComponent in switchList)
        {
            switchInterfaceDictionary.Add(switchComponent, switchComponent.GetComponent<ISwitchable>());
        }
    }

    private void Update(){
        if(isEnabled){
            bool allOn = true;
            for(var i = 0; i < switchList.Count && allOn != false; ++i){
                ISwitchable switchable = switchList[i].GetComponent<ISwitchable>();
                if (switchable != null)
                {
                    if (switchable.isOn == false)
                    {
                        allOn = false;
                    }
                }
            }
            if(allOn){
                parentSwitchableInterface.switchOn();
            } else {
                parentSwitchableInterface.switchOff();
            }
        }
    }
}