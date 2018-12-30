using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FootTracks : MonoBehaviour
{
    public Shader drawShader;
    public GameObject terrain;
    public Transform[] foot;
    public float tracksHeightDistance = 1;

    [Range(1,500)]
    public float brushSize = 1;
    [Range(0,1)]
    public float brushStrength = 1;
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
        myMaterial = terrain.GetComponent<MeshRenderer>().material;
        myMaterial.SetTexture("_Splat", splatMap = new RenderTexture(1024, 1024, 0, RenderTextureFormat.ARGBFloat));      
    }

    // Update is called once per frame
    void Update()
    {
        foreach(Transform foot in foot){
			Ray ray = new Ray (foot.position, new Vector3(0, -tracksHeightDistance, 0));
            raysToDraw.Add(ray);
            if(Physics.Raycast(ray, out terrainHit, tracksHeightDistance, (1 << LayerMask.NameToLayer("Terrain")))){
                drawMaterial.SetVector("_Coordinate", new Vector4(terrainHit.textureCoord.x, terrainHit.textureCoord.y, 0, 0));
                drawMaterial.SetFloat("_Strength", brushStrength);
                drawMaterial.SetFloat("_Size", brushSize);
                RenderTexture temp = RenderTexture.GetTemporary(splatMap.width, splatMap.height, 0, RenderTextureFormat.ARGBFloat);
                Graphics.Blit(splatMap, temp);
                Graphics.Blit(temp, splatMap, drawMaterial);
                RenderTexture.ReleaseTemporary(temp);
            }
        }
    }
}
