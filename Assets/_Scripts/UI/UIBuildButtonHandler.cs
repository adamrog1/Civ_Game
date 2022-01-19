using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class UIBuildButtonHandler : MonoBehaviour
{
    // Potrzebujemy danych o przycisku budowania, mozliwych budynkach i przycisku potwierdzajacym budowe
    [SerializeField]
    private Button buildBtn;

    private BuildDataSO buildData;

    [SerializeField]
    private Transform UiElementsParent;

    private List<UIBuildSelectionHandler> buildOptions;

    [SerializeField]
    private UnityEvent<BuildDataSO> OnBuildButtonClick;

    // Zapisujemy jakie sa mozliwosci budowy na podstaie tego co widnieje w panelu
    private void Start()
    {
        gameObject.SetActive(false);
        buildOptions = new List<UIBuildSelectionHandler>();
        foreach (Transform selectionHandler in UiElementsParent)
        {
            buildOptions.Add(selectionHandler.GetComponent<UIBuildSelectionHandler>());
        }
    }

    // Pobieramy dane dotyczace tego co ma byc zbudowane
    public void PrepareBuildButton(BuildDataSO buildData)
    {

        ResetUiElements();
        this.buildData = buildData;
        this.buildBtn.gameObject.SetActive(true);
    }

    // Usuwamy dane dot. kolejki budowy
    public void ResetBuildButton()
    {
        this.buildData = null;
        this.buildBtn.gameObject.SetActive(false);
    }

    // Zlecamy kolejke budowy
    public void HandleButtonClick()
    {
        OnBuildButtonClick?.Invoke(this.buildData);
        ResetUiElements();
    }

    // Wylaczamy kazdy z elementow UI
    private void ResetUiElements()
    {
        foreach (UIBuildSelectionHandler selectionHandler in buildOptions)
        {
            selectionHandler.Reset();
        }
    }

    // Wylaczanie UI budowania
    public void ToggleVisibility(bool val, ResourceManager resourceManager)
    {
        gameObject.SetActive(val);
        if (val == true)
        {
            PrepareBuildOptions(resourceManager);
            ResetBuildButton();
            ResetUiElements();
        }
    }

 
    // Podstwielamy tylko te elementy w UI budowy na ktore aktualnie pozwalaja nam srodki 
    private void PrepareBuildOptions(ResourceManager resourceManager)
    {
        foreach (UIBuildSelectionHandler buildItem in buildOptions)
        {
            if (buildItem.BuildData == null)
            {
                buildItem.ToggleActive(false);
                continue;
            }
            buildItem.ToggleActive(true);
            foreach (ResourceValue item in buildItem.BuildData.buildCost)
            {
                if (resourceManager.CheckResourceAvailability(item) == false)
                {
                    buildItem.ToggleActive(false);
                    break;
                }

            }
        }
    }
}
