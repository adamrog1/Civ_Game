using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Units/UnitData")]
public class UnitDataSO : ScriptableObject
{
    // Dane do edycji parametrow jednostek z poziomu panelu Unity
    public int movementRange = 10;
    public int health = 1;
    public int attackStrength = 1;
}
