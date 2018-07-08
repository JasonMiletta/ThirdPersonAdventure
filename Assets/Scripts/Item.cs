using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Item : MonoBehaviour, IEntity {

    #region Info
    [Header("Parameters")]
    [Tooltip("Info/Parameters/Settings for this Actor")]
    public bool isGrabbable = false;
    public bool isLootable = true;
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

    #region Components
    UI_ToolTip tooltip;
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

	// Use this for initialization
	void Start () {
        tooltip = GetComponentInChildren<UI_ToolTip>();
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public void bringToInventory(GameObject inventory){
        GetComponent<Rigidbody>().isKinematic = true;
        GetComponent<CapsuleCollider>().enabled = false;
        StartCoroutine(Util_TransformManipulation.smoothMovement(this.gameObject, this.transform.position, inventory.transform.position, 0.1f));
        StartCoroutine(Util_TransformManipulation.lerpObjToScale(this.gameObject, new Vector3(0.01f, 0.01f, 0.01f), 1f));
    }

    public void displayUIPrompt(){
        if(tooltip != null){
            tooltip.displayTooltip();
        }
    }

    public void hideUIPrompt(){
        if(tooltip != null){
            tooltip.hideTooltip();
        }
    }
}