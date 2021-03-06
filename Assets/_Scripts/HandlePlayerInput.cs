using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;

public class HandlePlayerInput : MonoBehaviour
{
    // Potrzebujemy danych o polozeniu kursora i warstwy z gameobjectami
    // Ustalamy treshold dla ktorego bedzie wykonywany ruch 
    // Potrzebujemy tez eventow zwiaanych z obsluga myszy
    public Camera currentCamera;
    public LayerMask layerMask;
    private Collider2D[] selectedObjects = new Collider2D[0];
    private int selectedIndex = 0;
    private bool firstSelection = false;
    private Vector3 startPosition;
    public float threshold = 0.5f;
    public UnityEvent<GameObject> OnHandleMouseClick;
    public UnityEvent<Vector3> OnHandleMouseFinishDragging;

    // Funkcja wywolywana z poziomu panelu Unity przy kazdej akcji myszka
    void Update()
    {
        if (Input.GetMouseButtonDown(0) && EventSystem.current.IsPointerOverGameObject() == false)
        {
            HandleMouseClick();
        }

        if (Input.GetMouseButtonUp(0) && EventSystem.current.IsPointerOverGameObject() == false)
        {
            HandleMouseUp();
        }
    }

    private void HandleMouseClick()
    {
        startPosition = GetMousePosition();
        firstSelection = selectedObjects.Length == 0;
        if (firstSelection)
        {
            PerformSelection();
        }
    }

    // Wybieramy gameobject z tych dostepnych na danym polu
    private void PerformSelection()
    {
        Collider2D collider = HandleMultipleObjectSelection(startPosition);
        GameObject selectedGameObject = collider == null ? null : collider.gameObject;
        OnHandleMouseClick?.Invoke(selectedGameObject);
        if (selectedGameObject != null)
            Debug.Log($"Selected {selectedGameObject.name}");
    }

    // Jesli sa dwa obiekty to przelaczamy sie miedy nimi
    private Collider2D HandleMultipleObjectSelection(Vector3 clickPosition)
    {
        Collider2D[] tempSelectedObjects = Physics2D.OverlapPointAll(clickPosition, layerMask);
        Collider2D selectedCollider = null;
        if(tempSelectedObjects.Length == 0)
        {
            selectedObjects = new Collider2D[0];
        }
        else
        {
            if (CheckTheSameSelection(tempSelectedObjects))
            {
                selectedIndex++;
                selectedIndex = selectedIndex >= selectedObjects.Length ? 0 : selectedIndex;
            }
            else
            {
                selectedObjects = tempSelectedObjects;
                selectedIndex = 0;
            }
            return selectedObjects[selectedIndex];
        }
        return selectedCollider;
    }

    // Przelaczanie polega na wybraniu innego obiektu niz aktualny z danego pola
    private bool CheckTheSameSelection(Collider2D[] tempSelectedObjects)
    {
        if (selectedObjects == null || selectedObjects.Length == 0) return false;
        return (tempSelectedObjects.Length == selectedObjects.Length) && tempSelectedObjects.Intersect(selectedObjects).Count() == selectedObjects.Length;
    }

    // Puszczenie przycisku triggeruje akcje wybory obiektu
    private void HandleMouseUp()
    {
        Vector3 endPosition = GetMousePosition();
        // Tylko jesli kursor ruszyl sie o wiecej niz pol pola
        if(Vector2.Distance(startPosition, endPosition) > threshold)
        {
            OnHandleMouseFinishDragging?.Invoke(endPosition);
            return;
        }
        if(firstSelection == false)
        {
            PerformSelection();
            return;
        }
    }

    // Obliczamy pozycje kursora na mapie
    private Vector3 GetMousePosition()
    {
        Vector3 mouseInput = currentCamera.ScreenToWorldPoint(Input.mousePosition);
        mouseInput.z = 0f;
        return mouseInput;
    }
}
