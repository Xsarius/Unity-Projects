using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChunkGenerator : MonoBehaviour
{
    ///////////////
    // Variables //
    ///////////////
    //
    // Summary:
    //     
    //
    // 
    //
    public float noiseScale;
    //
    // Summary:
    //     
    //
    // 
    //
    public int chunkCount;
    //
    // Summary:
    //     
    //
    // 
    //
    public Vector3 offset;
    //
    // Summary:
    //     
    //
    // 
    //
    private Chunk chunk;
    //
    // Summary:
    //     
    //
    // 
    //
    public BuildingSystemOperator buildingSystemOperator;
    //
    // Summary:
    //     
    //
    // 
    //
    public GameObject chunksFolder;
    //
    // Summary:
    //     
    //
    // 
    //
    public ChunkSO data;

    /////////////
    // Methods //
    /////////////
    //
    // Summary:
    //     
    //
    // Parameters:
    //   
    //
    public void Start()
    {
        CreateChunk();


    }
    //
    // Summary:
    //     
    //
    // Parameters:
    //   
    //   
    public void Update()
    {
        buildingSystemOperator.UpdateBuildingSystem();
    }
    //
    // Summary:
    //     
    //
    // Parameters:
    //   
    //   
    private void CreateChunk()
    {
        chunk = new Chunk();

        chunk.data = data;
        chunk.chunkOffset = offset;

        chunk.GenerateChunk(noiseScale);
        chunk.AdjustToOffset(offset);

        GameObject newChunk = new GameObject("Chunk" + chunkCount);

        newChunk.transform.parent = chunksFolder.transform;
        newChunk.transform.position += offset;

        newChunk.AddComponent<MeshFilter>();
        newChunk.AddComponent<MeshRenderer>();
        newChunk.AddComponent<MeshCollider>();

        newChunk.GetComponent<MeshFilter>().mesh = chunk.mesh;
        newChunk.GetComponent<MeshCollider>().sharedMesh = chunk.mesh;
        newChunk.GetComponent<MeshRenderer>().material = chunk.data.defaultMaterial;

        buildingSystemOperator.chunk = chunk;

        chunkCount++;
    }
}
