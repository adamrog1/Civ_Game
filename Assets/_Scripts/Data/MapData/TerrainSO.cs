using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Map/Data")]
public class TerrainSO : ScriptableObject
{
    // Dane dotyczace kosztu ruchu na danym polu pobieramy z panelu Unity
    public bool walkable = false;
    public int movementCost = 10;

}
