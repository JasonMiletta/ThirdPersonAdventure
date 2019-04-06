using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RTS_PlayerController : MonoBehaviour
{

    #region COMPONENTS
    private Camera _camera;
    #endregion

    #region STATE
    public Selectable currentlyHighlightedSelectable;
    public List<Selectable> currentlySelectedSelectables; 
    #endregion

    #region GIZMOS
    List<Ray> raysToDraw = new List<Ray>();
    #endregion

    /// <summary>
    /// Callback to draw gizmos that are pickable and always drawn.
    /// </summary>
    void OnDrawGizmos()
    {
        foreach(Ray r in raysToDraw){
            Gizmos.DrawRay(r);
        }
        raysToDraw = new List<Ray>();
    }


    // Start is called before the first frame update
    void Start()
    {
        _camera = GetComponentInChildren<Camera>();
    }

    // Update is called once per frame
    void Update()
    {
        handleMouseHighlights();
        handleMouseSelect();   
    }

    void handleMouseHighlights(){
        Ray mouseRay = _camera.ScreenPointToRay(Input.mousePosition);
        raysToDraw.Add(mouseRay);
        RaycastHit hit;
        if (Physics.Raycast(mouseRay,out hit,100.0f)) {
            Selectable hitSelectable = hit.collider.GetComponent<Selectable>();
            if(hitSelectable){
                hitSelectable.highlight();
                currentlyHighlightedSelectable = hitSelectable;
            }
            //Check if we have a previously highlighted selectable but are no longer hovering over something
            else if(currentlyHighlightedSelectable){
                currentlyHighlightedSelectable = null;
            }
        }
    }

    void handleMouseSelect(){
        if(Input.GetAxis("Fire") > 0){
            if(currentlyHighlightedSelectable){
                selectSelectable(currentlyHighlightedSelectable);
            } else {
                foreach(Selectable s in currentlySelectedSelectables){
                    removeSelectable(s);
                }
                currentlySelectedSelectables.Clear();
            }
        }
    }

    public void selectSelectable(Selectable s){
        currentlyHighlightedSelectable.select();
        currentlySelectedSelectables.Add(currentlyHighlightedSelectable);
    }

    public void removeSelectable(Selectable s){
        s.deselect();
    }
}
