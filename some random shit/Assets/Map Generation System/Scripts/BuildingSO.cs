using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "BuildingType", menuName = "Building Types")]
public class BuildingSO : ScriptableObject
{
    //
    // Summary:
    //     Type of buildings, ex. commercial,
    //      residential, etc. 
    //
    public string type;
}
