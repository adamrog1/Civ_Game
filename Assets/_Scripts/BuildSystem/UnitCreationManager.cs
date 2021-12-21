using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnitCreationManager : MonoBehaviour, ITurnDependant
{
    [SerializeField]
    private UIBuildButtonHandler townUi;
    private Town selectedTown = null;

    public void HandleSelection(GameObject selectedObject)
    {
        ResetTownBuildUI();

        if (selectedObject == null)
            return;

        selectedTown = selectedObject.GetComponent<Town>();
        if(selectedTown != null)
        {
            HandleTown(selectedTown);
        }
    }

    private void HandleTown(Town selectedTown)
    {
        townUi.ToggleVisibility(true);
    }

    public void CreateUnit(GameObject unitToCreate)
    {
        if (selectedTown.InProduction)
        {
            Debug.Log("Already done");
            return;
        }
        selectedTown.AddUnitToProduction(unitToCreate);
        ResetTownBuildUI();
    }

    private void ResetTownBuildUI()
    {
        townUi.ToggleVisibility(false);
        selectedTown = null;
    }

    public void WaitTurn()
    {
        ResetTownBuildUI();
    }
}
