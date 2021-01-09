/*
Code wrote basing on existing solution 
Author: Sebastian Lague
Source: https://github.com/SebLague/Procedural-Landmass-Generation/blob/master/Proc%20Gen%20E03/Assets/Scripts/Noise.cs
*/

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class Noise
{

    public static float[] GenerateHeightMap(int mapWidth, int mapLength, float noiseScale)
    {

        float[] heightMap = new float[(mapWidth + 1) * mapLength];

        if (noiseScale == 0)
        {
            noiseScale = 1;
        }

        for (int x = 0, i = 0; x < mapWidth; x++)
        {
            for (int z = 0; z < mapLength; z++, i++)
            {


                float sampleX = x / noiseScale;
                float sampleZ = z / noiseScale;
                heightMap[i] = Mathf.PerlinNoise(x / noiseScale, z / noiseScale) * 2 - 1;

            }

        }

        return heightMap;
    }
}

