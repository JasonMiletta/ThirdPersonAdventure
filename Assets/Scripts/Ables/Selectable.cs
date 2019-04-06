using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Selectable : MonoBehaviour
{

    public enum SelectedState {none, highlighted, selected};
    
    #region COMPONENTS
    GameObject self;
    private Material _defaultMaterial;
    private Renderer _renderer;
    public Material highlightedMaterial;
    public Material SelectedMaterial;
    #endregion

    #region STATE 
    private SelectedState currentSelectedState = SelectedState.none;
    #endregion

    // Start is called before the first frame update
    void Start()
    {
        _renderer = GetComponent<Renderer>();
        _defaultMaterial = _renderer.material;
        self = this.gameObject;
    }

    // Update is called once per frame
    void Update()
    {
        if(currentSelectedState == SelectedState.highlighted){
            unhighlight();
        }
    }

    public void select(){
        foreach(Renderer r in GetComponentsInChildren<Renderer>()){
            r.material = SelectedMaterial;
        }
        setSelectedStateSelected();
    }

    public void deselect(){
        foreach(Renderer r in GetComponentsInChildren<Renderer>()){
            r.material = _defaultMaterial;
        }
        setSelectedStateNone();
    }


    public void highlight(){
        if(currentSelectedState == SelectedState.none){
            foreach(Renderer r in GetComponentsInChildren<Renderer>()){
                r.material = highlightedMaterial;
            }
            setSelectedStateHighlighted();
        }
    }

    public void unhighlight(){
        foreach(Renderer r in GetComponentsInChildren<Renderer>()){
            r.material = _defaultMaterial;
        }
        setSelectedStateNone();
    }

    private void setSelectedStateNone(){
        currentSelectedState = SelectedState.none;
    }
    private void setSelectedStateHighlighted(){
        currentSelectedState = SelectedState.highlighted;
    }
    private void setSelectedStateSelected(){
        currentSelectedState = SelectedState.selected;
    }
}
