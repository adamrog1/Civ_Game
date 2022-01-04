using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class SystemTurnTaker : MonoBehaviour
{
    public void WaitTurn()
    {
        foreach (ITurnDependant item in GetComponents<ITurnDependant>())
        {
            item.WaitTurn();
        }
    }
}
