using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InfoManager : MonoBehaviour, ITurnDependant
{
    // Pobieramy informacje, ktory element canvasu jest naszym panelem
    [SerializeField]
    private UIInfoPanel infoPanel;

    // Domyslnie nic nie jest klikniete wiec mozemy domyslnie wylaczyc panel
    private void Start()
    {
        HideInfoPanel();
    }

    // Wylaczamy UI element
    public void HideInfoPanel()
    {
        infoPanel.ToggleVisibility(false);
    }

    // Wlaczamy UI element
    private void ShowInfoPanel(InfoProvider infoProvider)
    {
        infoPanel.ToggleVisibility(true);
        infoPanel.SetData(infoProvider.Image, infoProvider.NameToDisplay);
    }

    // Wyswietlamy informacje o aktualnie kliknietym obiekcie
    public void HandleSelection(GameObject selectedObject)
    {
        HideInfoPanel();
        if(selectedObject == null)
        {
            return;
        }
        InfoProvider infoProvider = selectedObject.GetComponent<InfoProvider>();
        if(infoProvider == null)
        {
            return;
        }
        ShowInfoPanel(infoProvider);
    }

    // Przy koncu tury rowniez wylaczamy panel
    public void WaitTurn()
    {
        HideInfoPanel();
    }
}
