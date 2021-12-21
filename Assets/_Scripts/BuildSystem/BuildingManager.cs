using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuildingManager : MonoBehaviour, ITurnDependant
{
    [SerializeField]
    private UIBuildButtonHandler unitBuildUI;
    private Unit farmerUnit;

    [SerializeField]
    private AudioSource audioSource;

    [SerializeField]
    private Map map;

    private void Awake()
    {
        audioSource = GetComponent<AudioSource>();
    }

    public void HandleSelection(GameObject selectedObject)
    {
        ResetBuildingSystem();

        if (selectedObject == null)
            return;

        farmerUnit = selectedObject.GetComponent<Unit>();
        if (farmerUnit != null && farmerUnit.CanStillMove())
        {
            HandleUnitSelection();
        }
    }

    private void ResetBuildingSystem()
    {
        farmerUnit = null;
        unitBuildUI.ToggleVisibility(false);
    }

    private void HandleUnitSelection()
    {
        unitBuildUI.ToggleVisibility(true);
    }

    public void BuildStructure(GameObject structurePrefab)
    {
        if (map.IsPositionInvalid(this.farmerUnit.transform.position))
            return;

        Debug.Log("Placing at " + this.farmerUnit.transform.position);
        GameObject structure = Instantiate(structurePrefab, this.farmerUnit.transform.position, Quaternion.identity);
        map.AddStructure(this.farmerUnit.transform.position, structure);
        audioSource.Play();

        if (structurePrefab.name == "TownStructure")
            this.farmerUnit.DestroyUnit();
        
        else
            this.farmerUnit.FinishMovement();
        

        ResetBuildingSystem();

    }

    public void WaitTurn()
    {
        ResetBuildingSystem();
    }
}