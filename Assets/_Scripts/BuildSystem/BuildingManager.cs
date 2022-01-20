using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuildingManager : MonoBehaviour, ITurnDependant
{
    // Potrzebujemy pobac dane dotyczace kilknietej jednostki
    // w celu obslugi systemu budowania, potrzebne tez beda informacje o pliku audio
    // wlaczanym przy budowaniu i danych do wyswietlenia w UI
    [SerializeField]
    private UIBuildButtonHandler unitBuildUI;
    private Unit workerUnit;

    [SerializeField]
    private AudioSource audioSource;

    [SerializeField]
    private Map map;

    [SerializeField]
    private InfoManager infoManager;

    [SerializeField]
    ResourceManager resourceManager;

    private void Awake()
    {
        audioSource = GetComponent<AudioSource>();
    }

    // Funkcja sprawdza czy klikniety obiekt to jednostka
    public void HandleSelection(GameObject selectedObject)
    {
        ResetBuildingSystem();

        if (selectedObject == null)
            return;
        Worker worker = selectedObject.GetComponent<Worker>();
        if(worker != null)
        {
            HandleUnitSelection(worker);
        }
    }

    // Zamykamy panel budowania i wylaczamy mozliwosc budowania jesli jednostka nie ma wiecej ruchu
    private void ResetBuildingSystem()
    {
        if (workerUnit != null)
            workerUnit.FinishedMoving.RemoveListener(ResetBuildingSystem);
        workerUnit = null;
        unitBuildUI.ToggleVisibility(false,resourceManager);
    }

    // Jesli zostala kliknieta jednostka to wlaczamy panel budowania
    private void HandleUnitSelection(Worker worker)
    {
        workerUnit = worker.GetComponent<Unit>();
        if (workerUnit != null && workerUnit.CanStillMove())
        {
            unitBuildUI.ToggleVisibility(true,resourceManager);
            workerUnit.FinishedMoving.AddListener(ResetBuildingSystem);
        }
    }

    // Sprawdamy pozycje jednostki na mapie i czy gracz ma niezbedne surowce
    public void BuildStructure(BuildDataSO buildData)
    {
        // Upewniamy sie ze budynek nie zostanie postawiony w zluym miejscu
        if (map.IsPositionInvalid(this.workerUnit.transform.position))
            return;

        // Pobieramy koszt budowy
        if (resourceManager.isThereEneoughResources(buildData.buildCost)==false) { return; }
        else
            resourceManager.SpendResource(buildData.buildCost);

        // Tworzymy budynek zgodnie z zaznaczonymi danymi
        GameObject structure = Instantiate(buildData.prefab, this.workerUnit.transform.position, Quaternion.identity);
        ResourceProducer resourceProducer = structure.GetComponent<ResourceProducer>();

        // Jesli mamy niezbedne surowce to mozemy postawic budynek i odjac surowce
        if (resourceProducer != null)
            resourceProducer.Initialize(buildData);

        map.AddStructure(this.workerUnit.transform.position, structure);
        audioSource.Play();

        // Jesli zbudowany miasto to usuwamy jednostke, w przeciwnym wypadku konczymy ruch
        if (buildData.prefab.name == "TownStructure")
        {
            this.workerUnit.DestroyUnit();
            infoManager.HideInfoPanel();
        }
        else
        {
            this.workerUnit.FinishMovement();
        }      

        // Po zakonczeniu budowy wylacamy element UI
        ResetBuildingSystem();
    }
    
    // Upewniamy sie ze przy wylaczaniu tury z ekranu zniknie UI budowania
    public void WaitTurn()
    {
        ResetBuildingSystem();
    }
}
