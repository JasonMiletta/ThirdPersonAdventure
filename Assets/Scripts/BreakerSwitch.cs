using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BreakerSwitch: MonoBehaviour{

    #region Parameters
    public bool isEnabled {
        get; set;
    }
    #endregion
    #region Components
    [SerializeField]
    private ISwitchable ParentSwitch;
    [SerializeField]
    private List<ISwitchable> switchList;
    #endregion
    private void Start() {
        isEnabled = true;
    }

    private void Update(){
        if(isEnabled){
            bool allOn = true;
            for(var i = 0; i < switchList.Count || allOn == false; ++i){
                if(switchList[i].isOn == false){
                    allOn = false;
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