using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Map/Data")]
public class TerrainSO : ScriptableObject
{
    public bool walkable = false;
    public int movementCost = 10;

}
