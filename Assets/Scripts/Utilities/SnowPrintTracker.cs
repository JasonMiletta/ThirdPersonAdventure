using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SnowPrintTracker : MonoBehaviour
{
    public Shader drawShader;
    public float tracksHeightDistance = 1;

    private int layerMask;
    RaycastHit terrainHit;

    private Material drawMaterial;
    private Material myMaterial;

    private RenderTexture splatMap;

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
        layerMask = LayerMask.GetMask("Terrain");
        drawMaterial = new Material(drawShader);
        myMaterial = GetComponent<MeshRenderer>().material;
        myMaterial.SetTexture("_Splat", splatMap = new RenderTexture(1024, 1024, 0, RenderTextureFormat.ARGBFloat));      
    }

    // Update is called once per frame
    void Update()
    {
    
    }

    public void drawTracksToMaterial(RaycastHit terrainHit, float brushSize, float brushStrength){
        drawMaterial.SetVector("_Coordinate", new Vector4(terrainHit.textureCoord.x, terrainHit.textureCoord.y, 0, 0));
        drawMaterial.SetFloat("_Strength", brushStrength);
        drawMaterial.SetFloat("_Size", brushSize);
        RenderTexture temp = RenderTexture.GetTemporary(splatMap.width, splatMap.height, 0, RenderTextureFormat.ARGBFloat);
        Graphics.Blit(splatMap, temp);
        Graphics.Blit(temp, splatMap, drawMaterial);
        RenderTexture.ReleaseTemporary(temp);
    }
}
