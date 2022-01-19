using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Town : MonoBehaviour, ITurnDependant
{
    public bool InProduction { get; private set; }
    private GameObject unitToCreate;

    // Zmieniamy status miasta na "in production"
    public void AddUnitToProduction(GameObject unitToCreate)
    {
        this.unitToCreate = unitToCreate;
        InProduction = true;
    }

    // Przycisk konczacy runde koncy rowniez produkcje
    public void WaitTurn()
    {
        CompleteProduction();
    }

    // Jesli mamy cos w kolejce produkcji to mozemy stworzyc ta jednostke 
    private void CompleteProduction()
    {
        if(InProduction == false)
        {
            return;
        }
        InProduction = false;
        if (unitToCreate == null)
            return;
        Instantiate(unitToCreate, transform.position, Quaternion.identity);
    }
}
