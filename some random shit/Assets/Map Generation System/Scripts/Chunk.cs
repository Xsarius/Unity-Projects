using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Chunk
{
    ///////////////
    // Variables //
    ///////////////
    //
    // Summary:
    //     Offset of a chunk measured from the origin,
    //      for convinience placed in a separate variable,
    //      for future use.
    //  
    public Vector3 chunkOffset;
    //
    // Summary:
    //     Scriptable object containing common chunk data 
    //      such as: chunk size, cell size, etc.
    //
    public ChunkSO data;
    //
    // Summary:
    //     Mesh component of the chunk, used in rendering proccess.
    //
    public Mesh mesh;
    //
    // Summary:
    //     Instance of a custom class,
    //      contains ex. builgings (GameObject)
    //      array, etc.
    //
    public BuildingGrid buildingGrid;
    //
    // Summary:
    //      GameObject related to generated chunk in the 
    //          game hierarchy.
    public GameObject chunkObject;
    //
    // Summary:
    //     3-dim array containing all of the chunk's
    //      verticies.
    // 
    private Vector3[,,] verticies;
    //   
    // Summary:
    //     2-dim array containing all of the chunk's
    //      face triangles.
    //      The first coordinate is a triangle index,
    //      and the second in a triangle vertex, thus
    //      implementation should look like:
    //
    //      int[,] triangles = new int[n,3];
    //
    //      where n is number of triangles, or
    //      n = map width * map length * 2.   
    // 
    private int[,] triangles;
    //
    // Summary:
    //     Custom class prepared for the future 
    //      development.
    // 
    public class BuildingGrid
    {
        public GameObject[,,] grid;

        public BuildingGrid(int chunkWidth, int chunkHeight, int chunkLength, ChunkSO data)
        {
            chunkWidth /= data.cellSize;
            chunkHeight /= data.cellSize;
            chunkLength /= data.cellSize;
            grid = new GameObject[chunkWidth, chunkHeight, chunkLength];
        }
    }

    /////////////
    // Methods //
    /////////////
    //
    // Summary:
    //     Basic function used to generate the chunk.
    //
    // Parameters:
    //      noiseScale - factor used in a height map 
    //          generation proccess.
    public void GenerateChunk(float noiseScale, Vector3 offset)
    {

        GenerateMesh((int)data.chunkSize.x, (int)data.chunkSize.y, (int)data.chunkSize.z, noiseScale, offset);

        GenerateBuildingGrid((int)data.chunkSize.x, (int)data.chunkSize.y, (int)data.chunkSize.z);

    }
    //
    // Summary:
    //     Function used to generate
    //
    // Parameters:
    //      chunkWidth - width of a generated chunk.
    //      
    //      chunkHeight - height of a generated chunk.
    //   
    //      chunkLength - lenght of a generated chunk.
    //
    //      cellSize - size of a one cell in the chunk.
    //
    //      noiseScale - factor used in a height map 
    //          generation proccess.
    //
    //      offset - offset of a generated chunk from the 
    //          origin of the map.
    //
    private void GenerateMesh(int chunkWidth, int chunkHeight, int chunkLength, float noiseScale, Vector3 offset)
    {
        chunkWidth /= data.cellSize;
        chunkHeight /= data.cellSize;
        chunkLength /= data.cellSize;

        chunkOffset = offset;

        verticies = new Vector3[chunkWidth, chunkHeight, chunkLength];

        triangles = new int[verticies.Length * 6, 3];

        mesh = new Mesh();

        // Generating height map 
        float[] heightMap = Noise.GenerateHeightMap(chunkWidth, chunkLength, noiseScale);

        for (int y = 0; y < chunkHeight; y++)
        {
            for (int z = 0, currentQuad = 0; z < chunkLength; z += 2)
            {
                for (int x = 0; x < chunkWidth; currentQuad++, x += 2)
                {
                    verticies[x, y, z] = new Vector3(x * data.cellSize - chunkOffset.x, heightMap[currentQuad] + chunkOffset.y, z * data.cellSize - chunkOffset.z);
                    verticies[x + 1, y, z] = new Vector3((x + 1) * data.cellSize - chunkOffset.x, heightMap[currentQuad] + chunkOffset.y, z * data.cellSize - chunkOffset.z);
                    verticies[x, y, z + 1] = new Vector3(x * data.cellSize - chunkOffset.x, heightMap[currentQuad] + chunkOffset.y, (z + 1) * data.cellSize - chunkOffset.z);
                    verticies[x + 1, y, z + 1] = new Vector3((x + 1) * data.cellSize - chunkOffset.x, heightMap[currentQuad] + chunkOffset.y, (z + 1) * data.cellSize - chunkOffset.z);
                }
                currentQuad++;
            }
        }

        for (int y = 0; y < chunkHeight; y++)
        {
            for (int z = 0, vert = 0, tris = 0; z < chunkLength - 1; z++, vert++)
            {

                for (int x = 0; x < chunkWidth - 1; x++, vert++, tris += 2)
                {
                    triangles[tris, 0] = vert;
                    triangles[tris, 1] = vert + chunkWidth;
                    triangles[tris, 2] = vert + 1;
                    triangles[tris + 1, 0] = vert + 1;
                    triangles[tris + 1, 1] = vert + chunkWidth;
                    triangles[tris + 1, 2] = vert + chunkWidth + 1;
                }
            }
        }


        UpdateMesh();
    }
    //
    // Summary:
    //     Function chaning verticies custom made 3-dim coordinates system
    //      into 1-dim array, needed for built-in mesh system.
    //
    public Vector3[] Verticies()
    {
        Vector3[] verticiesOneDim = new Vector3[verticies.Length];

        for (int y = 0, i = 0; y < verticies.GetLength(1); y++)
        {
            for (int z = 0; z < verticies.GetLength(2); z++)
            {
                for (int x = 0; x < verticies.GetLength(0); x++, i++)
                {
                    verticiesOneDim[i] = verticies[x, y, z];
                }
            }
        }

        return verticiesOneDim;
    }
    //
    // Summary:
    //     Function chaning triangles custom made 2-dim coordinates system
    //      into 1-dim array, needed for built-in mesh system.
    //
    public int[] Triangles()
    {
        int[] trianglesOneDim = new int[triangles.Length];

        for (int tris = 0, i = 0; tris < triangles.Length / 3; tris++)
        {
            for (int vert = 0; vert < 3; vert++, i++)
            {
                trianglesOneDim[i] = triangles[tris, vert];
            }
        }

        return trianglesOneDim;
    }
    //
    // Summary:
    //     Function generating building grid, prepared for the furture use.
    //
    // Parameters:
    //      chunkWidth - width of a generated chunk.
    //      
    //      chunkHeight - height of a generated chunk.
    //   
    //      chunkLength - lenght of a generated chunk.
    //
    private void GenerateBuildingGrid(int chunkWidth, int chunkHeight, int chunkLength)
    {
        buildingGrid = new BuildingGrid(chunkWidth, chunkHeight, chunkLength, data);
    }
    //
    // Summary:
    //     Function adding generated verticies and triangle faces 
    //          to the mesh component of a chunk. 
    //          Used for the rendering.
    //
    public void UpdateMesh()
    {
        mesh.Clear();

        mesh.vertices = Verticies();
        mesh.triangles = Triangles();

        mesh.RecalculateNormals();
    }


}
