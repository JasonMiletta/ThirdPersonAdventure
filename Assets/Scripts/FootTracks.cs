using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FootTracks : MonoBehaviour
{
    public Transform[] foot;
    public float tracksHeightDistance = 1;

    [Range(1,500)]
    public float brushSize = 1;
    [Range(0,1)]
    public float brushStrength = 1;
    private int layerMask;
    RaycastHit terrainHit;

    #region GIZMOS
    List<Ray> raysToDraw = new List<Ray>();
    #endregion
    void OnDrawGizmos(){
        foreach(Ray r in raysToDraw){
            Gizmos.DrawRay(r);
        }
		raysToDraw = new List<Ray>();
    }

    // Start is called before the first frame update
    void Start()
    {    
    }

    // Update is called once per frame
    void Update()
    {
        foreach(Transform foot in foot){
			Ray ray = new Ray (foot.position, new Vector3(0, -tracksHeightDistance, 0));
            raysToDraw.Add(ray);
            if(Physics.Raycast(ray, out terrainHit, tracksHeightDistance, (1 << LayerMask.NameToLayer("Terrain")))){
                SnowPrintTracker snowTracker = terrainHit.collider.GetComponent<SnowPrintTracker>();
                if(snowTracker){
                    snowTracker.drawTracksToMaterial(terrainHit, brushSize, brushStrength);
                }
            }
        }
    }
}
