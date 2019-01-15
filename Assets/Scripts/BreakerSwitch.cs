using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BreakerSwitch: MonoBehaviour{

    #region Parameters
    public Bool isEnabled {
        get; set;
    }
    public ISwitch ParentSwitch;
    public List<ISwitch> switchList;
    #endregion
    private void Start() {
        isEnabled = true;
    }

    private void Update(){
        if(isEnabled){
            bool allOn = true;
            for(var i = 0; i < switchList.Length || allOn == false; ++i){
                if(switchList[i].isOn == false){
    `               allOn = false;
                }
            }
            if(allOn){
                ParentSwitch.switchOn();
            } else {
                ParentSwitch.switchOff();
            }
        }
    }
}