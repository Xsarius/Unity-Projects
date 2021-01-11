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
    public float noiseScale;
    //
    // Summary:
    //     Number of a chunks to be generated.
    //
    public int chunkCount;
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
    public BuildingSystemOperator buildingSystemOperator;
    //
    // Summary:
    //     GameObject used as a folder; just to clean up the mess.
    //
    public GameObject chunksFolder;
    //
    // Summary:
    //     Scriptable 
    public ChunkSO data;

    /////////////
    // Methods //
    /////////////
    //
    // Summary:
    //     Unity built-in start function;
    //
    public void Start()
    {
        CreateChunk();
    }
    //
    // Summary:
    //       Unity built-in update function.
    //
    public void Update()
    {
        buildingSystemOperator.UpdateBuildingSystem();
    }
    //
    // Summary:
    //     
    //  
    private void CreateChunk()
    {
        chunk = new Chunk();

        chunk.data = data;

        Vector3 offset = new Vector3();
        chunk.chunkOffset = offset;

        chunk.GenerateChunk(noiseScale, offset);

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
