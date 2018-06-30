using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Item : MonoBehaviour, IEntity, IInteractable {

    #region Info
    [Header("Parameters")]
    [Tooltip("Info/Parameters/Settings for this Actor")]
    public bool isGrabbable = false;
    public bool isLootable = false;
    public int weight = 1;
    public string apiName{
        get;
        set;
    }
    public string displayName{
        get;
        set;
    }
    public bool isInteractable{
        get;set;
    }
    #endregion


    #region IEntity Interface
    void IEntity.Initialize(){
        if(apiName == null || apiName.Equals("")){
            Debug.LogError("Item does not have an apiName!");
        }
        if(displayName == null || displayName.Equals("")){
            Debug.LogError("Item does not have a displayName!");
        }
    }
    #endregion

    #region IInteractable Interface
    void IInteractable.Interact(Actor interactor){
        if(isLootable){
            interactor.lootItem(this);
        } else if(isGrabbable){
            //TODO reparent to interactor and start holding
        }
    }
    #endregion

	// Use this for initialization
	void Start () {

		
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}