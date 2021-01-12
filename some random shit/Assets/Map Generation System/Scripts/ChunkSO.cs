using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "ChunkData", menuName = "Chunk Data", order = 1)]
public class ChunkSO : ScriptableObject
{
    ///////////////
    // Variables //
    ///////////////
    //
    // Summary:
    //     Size of every chunk generated in the
    //      game. Counted as number of quads,
    //      number of verticies / 2 * cellSize. 
    //
    public Vector3 chunkSize;
    //
    // Summary:
    //     Factor of scale, matter only in 
    //      setting just the right perspective
    //      shouldn't be reachable by user.
    //
    public int cellSize = 1;
    //
    // Summary:
    //     
    //
    // 
    //
    public Material defaultMaterial;
}
