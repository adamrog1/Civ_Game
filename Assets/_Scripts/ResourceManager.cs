using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResourceManager : MonoBehaviour
{
    // Pobiramy dane o dostepnych typach surowcow i ich ilosci, oraz o panelu w ktorym sa wyswietlane 
    UIResourcesManager resourceUI;

    Dictionary<ResourceType, int> resourceDictionary = new Dictionary<ResourceType, int>();

    public List<ResourceValue> initialResources = new List<ResourceValue>();

    // Przy starcie zapisujemy te dane do zmiennych i wyswietlamy je w UI
    private void Start()
    {
        resourceUI = FindObjectOfType<UIResourcesManager>();
        PrepareResoureDictionary();
        SetInitialResourceValues();
        UpdateUI();
    }

    // Aktualizyjemy UI przy zmianie stanu surowcow
    private void UpdateUI()
    {
        foreach (ResourceType resourceType in resourceDictionary.Keys)
        {
            UpdateUI(resourceType);
        }
    }

    private void UpdateUI(ResourceType resourceType)
    {
        resourceUI.SetResource(resourceType, resourceDictionary[resourceType]);
    }

    // Pobieramy dane o wybranym surowcu
    private void SetInitialResourceValues()
    {
        foreach (ResourceValue initialResourceValue in initialResources)
        {
            if (initialResourceValue.resourceType == ResourceType.None)
                throw new ArgumentException("Resource can't be None!");
            resourceDictionary[initialResourceValue.resourceType] = initialResourceValue.resourceAmount;
        }
    }

    // Dodajemy surewiec do slownika
    public void AddResource(List<ResourceValue> producedResources)
    {
        foreach (ResourceValue resourseVal in producedResources)
        {
            AddResourse(resourseVal.resourceType, resourseVal.resourceAmount);
        }
    }

    // Odswiezamy UI
    public void AddResourse(ResourceType resourceType, int resourceAmount)
    {
        resourceDictionary[resourceType] += resourceAmount;
        VerifyResourceAmount(resourceType);
        UpdateUI(resourceType);
    }

    private void PrepareResoureDictionary()
    {
        foreach (ResourceType resourceType in Enum.GetValues(typeof(ResourceType)))
        {
            if (resourceType == ResourceType.None)
                continue;
            resourceDictionary[resourceType] = 0;
        }
    }

    // Sprawdzamy ilosc surowca na podstawie podanego typu 
    public bool CheckResourceAvailability(ResourceValue resourceRecquired)
    {
        return resourceDictionary[resourceRecquired.resourceType] >= resourceRecquired.resourceAmount;
    }

    // Usuwamy okreslona ilosc surowca se slownika
    public void SpendResource(List<ResourceValue> buildCost)
    {
        foreach (ResourceValue resourceValue in buildCost)
        {
            SpendResource(resourceValue.resourceType, resourceValue.resourceAmount);
        }
    }

    // Oswiezamy UI
    public void SpendResource(ResourceType resourceType, int resourceAmount)
    {
        resourceDictionary[resourceType] -= resourceAmount;
        VerifyResourceAmount(resourceType);
        UpdateUI(resourceType);
    }

    // Sprawdzamy czy podana wartosc nie przekroczy dostepnych surowcow 
    public bool isThereEneoughResources(List<ResourceValue> buildCost) {

        foreach (ResourceValue resourceValue in buildCost)
        {
            if (isThereEneoughResources(resourceValue.resourceType, resourceValue.resourceAmount) == false) { return false; }
        }
        return true;
    }
    public bool isThereEneoughResources(ResourceType resourceType, int resourceAmount) {
        if (resourceDictionary[resourceType] < resourceAmount)
        {
            return false;
        }
        else
            return true;
    
    }
    private void VerifyResourceAmount(ResourceType resourceType)
    {
        if (resourceDictionary[resourceType] < 0)
            throw new InvalidOperationException("Can't have resource less than 0 " + resourceType);
    }
}

[Serializable]
public struct ResourceValue
{
    public ResourceType resourceType;
    [Min(0)]
    public int resourceAmount;
}