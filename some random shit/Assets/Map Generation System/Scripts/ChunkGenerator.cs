using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChunkGenerator : MonoBehaviour
{
    public float noiseScale;

    public int chunkCount;

    public Vector3 offset;

    private Chunk chunk;

    private BuildingSystemOperator buildingSystemOperator;

    public GameObject chunksFolder;

    public ChunkSO data;

    public void Start()
    {
        CreateChunk();
    }

    public void Update()
    {

    }

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



        chunkCount++;
    }
}
