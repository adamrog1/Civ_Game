using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnitCreationManager : MonoBehaviour, ITurnDependant
{
    [SerializeField]
    private UIBuildButtonHandler townUi;
    private Town selectedTown = null;
    [SerializeField] private ResourceManager resourcemanager;

    // Sprawdzamy czy klikniety obiekt to miasto
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

    // Jesli kliknieto miasto to wlaczamy UI
    private void HandleTown(Town selectedTown)
    {
        townUi.ToggleVisibility(true, resourcemanager);
    }

    // Sprawdzamy czy mamy niezbedne surowce to stworzenia jednoski i dodajemy ja do produkcji
    public void CreateUnit(BuildDataSO unitData)
    {
        // Jedno miasto moze produkowac tylko jedna jednostke na raz
        if (selectedTown.InProduction)
        {
            return;
        }
        if (resourcemanager.isThereEneoughResources(unitData.buildCost) == false) { return; }
        resourcemanager.SpendResource(unitData.buildCost);
        selectedTown.AddUnitToProduction(unitData.prefab);
        ResetTownBuildUI();
    }

    // Wylaczamy UI rekrutacji jednostek
    private void ResetTownBuildUI()
    {
        townUi.ToggleVisibility(false, resourcemanager);
        selectedTown = null;
    }

    // W przypadku konca tury wylaczamy UI
    public void WaitTurn()
    {
        ResetTownBuildUI();
    }
}
