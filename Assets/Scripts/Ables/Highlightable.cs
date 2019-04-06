using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Highlightable : MonoBehaviour
{

    #region COMPONENTS
    GameObject self;
    private Material _defaultMaterial;
    private Renderer _renderer;
    public Material highlightedMaterial;
    #endregion

    #region 
    private bool isHighlighted = false;
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
        if(isHighlighted){
            stopHighlight();
        }
    }

    public void startHighlight(){
        foreach(Renderer r in GetComponentsInChildren<Renderer>()){
            r.material = highlightedMaterial;
        }
        isHighlighted = true;
    }

    public void stopHighlight(){
        foreach(Renderer r in GetComponentsInChildren<Renderer>()){
            r.material = _defaultMaterial;
        }
        isHighlighted = false;
    }
}
