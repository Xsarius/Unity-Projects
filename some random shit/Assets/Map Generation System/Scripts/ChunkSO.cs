using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "ChunkData", menuName = "ChunkData", order = 1)]
public class ChunkSO : ScriptableObject
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
    public Vector3 chunkSize;
    //
    // Summary:
    //     
    //
    // 
    //
    [Range(1, 10)]
    public int cellSize;
    //
    // Summary:
    //     
    //
    // 
    //
    public Material defaultMaterial;
}
