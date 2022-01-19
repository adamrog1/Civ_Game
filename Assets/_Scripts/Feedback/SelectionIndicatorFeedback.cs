using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SelectionIndicatorFeedback : MonoBehaviour, ITurnDependant
{
    // Dla kazego obiektu w ktory mozna kliknac potrzebujemy jego assetu, pobieramy go z panelu Unity
    int defaultSortingLayer;
    [SerializeField]
    private SpriteRenderer spriteRenderer;
    private int layerToUse;

    private void Start()
    {
        layerToUse = SortingLayer.NameToID("SelectedObject");
        defaultSortingLayer = spriteRenderer.sortingLayerID;
    }

    // Jesli obiekt jest klikniety do wrzucamy go do warstwy zawierajacej agent feedback
    private void ToggleSelection(bool val)
    {
        if (val)
        {
            spriteRenderer.sortingLayerID = layerToUse;
        }
        else
        {
            spriteRenderer.sortingLayerID = defaultSortingLayer;
        }
    }

    // Dla przypisania do akcji onclick, wlaczamy selection feedback
    public void Select()
    {
        ToggleSelection(true);
    }

    // Analogicznie przy zrzuceniu focusu z obiektu wylaczamy feednabk
    public void Deselect()
    {
        ToggleSelection(false);
    }

    // Przy koncu tury wylaczamy feedback
    public void WaitTurn()
    {
        Deselect();
    }
}
