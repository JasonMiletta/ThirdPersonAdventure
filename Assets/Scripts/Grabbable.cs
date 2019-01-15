using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Grabbable: MonoBehaviour {

    #region Parameters
    public Bool isGrabbable;
    #endregion

    #region Components
    #endregion
    
    private void Start() {
        isGrabbable = true;
    }
}