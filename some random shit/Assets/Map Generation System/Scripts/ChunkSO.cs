using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "ChunkData", menuName = "ChunkData", order = 1)]
public class ChunkSO : ScriptableObject
{
    public Vector3 chunkSize;

    public float cellSize;

    public Material defaultMaterial;
}
