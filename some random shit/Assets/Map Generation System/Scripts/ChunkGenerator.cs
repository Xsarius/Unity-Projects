using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class ChunkGenerator : MonoBehaviour
{
    ///////////////
    // Variables //
    ///////////////
    //
    // Summary:
    //     Factor used in height map generation.
    //      Can be chosen arbitrarly.
    //
    public float noiseScale;
    //
    // Summary:
    //     Number of a chunks to be generated.
    //
    public int chunkCount;
    //
    // Summary:
    //     Custom chunk class element containing 
    //      the data related to a single chunk.
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
    //     Scriptable object containing a common chunk data.
    //
    public ChunkSO data;
    //
    // Summary: 
    //      List containing all generated chunks.
    //
    public List<Chunk> chunks;

    /////////////
    // Methods //
    /////////////
    //
    // Summary:
    //     Unity built-in start function;
    //
    public void Start()
    {
        for (int chunkID = 0; chunkID < chunkCount; chunkID++)
        {
            CreateChunk(chunkID);
        }
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
    //     Function creating single chunk of 
    //  
    private void CreateChunk(int ID)
    {
        Vector3 offset = new Vector3(0, 0, 0);

        chunk = new Chunk();

        chunk.data = data;
        chunk.chunkOffset = offset;

        chunk.GenerateChunk(noiseScale, offset);

        // Creating GameObject related to the generated chunk.
        GameObject newChunk = new GameObject("Chunk" + ID);

        newChunk.transform.parent = chunksFolder.transform;
        //?newChunk.transform.position += offset;

        newChunk.AddComponent<MeshFilter>();
        newChunk.AddComponent<MeshRenderer>();
        newChunk.AddComponent<MeshCollider>();

        newChunk.GetComponent<MeshFilter>().mesh = chunk.mesh;
        newChunk.GetComponent<MeshCollider>().sharedMesh = chunk.mesh;
        newChunk.GetComponent<MeshRenderer>().material = chunk.data.defaultMaterial;

        buildingSystemOperator.chunk = chunk;

        // Connecting GameObject with the chunk.
        chunk.chunkObject = newChunk;

        // Updating chunk list.
        //chunks.Add(chunk);
    }
}
