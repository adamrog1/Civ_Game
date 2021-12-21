using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerTurnTaker : MonoBehaviour
{
    public void WaitTurn()
    {
        foreach (ITurnDependant item in GetComponents<ITurnDependant>())
        {
            item.WaitTurn();
        }
    }
}
