using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class CharacterMovement : MonoBehaviour
{
    // Pobieramy dane o wybranej jednostce, jej pozycji i zasiegu ruchu
    [SerializeField]
    private Map map;
    private Unit selectedUnit;
    private List<Vector2Int> movementRange;

    [SerializeField]
    private MovementRangeHighlight rangeHighlight;

    // Jesli wybrany obiekt to jednstka i ma ona jeszcze dostepne ruchy to mozemy wyswietlic highlight
    public void HandleSelection(GameObject detectedObject)
    {
        // Pomijamy naciesniecie budynku
        if (detectedObject == null)
        {
            ResetCharacterMovement();
            return;
        }
        if (detectedObject.CompareTag("Player"))
        {
            this.selectedUnit = detectedObject.GetComponent<Unit>();
        }
        else
            this.selectedUnit = null;
        this.selectedUnit = detectedObject.GetComponent<Unit>();
        if (this.selectedUnit == null)
            return;
        if (this.selectedUnit.CanStillMove())
            PrepareMovementRange();
        else
            rangeHighlight.ClearHighlight();
    }

    // Obliczamy zasieg ruchu danej jednostki i wyswietlamy go
    private void PrepareMovementRange()
    {
        movementRange = GetMovementRangeFor(this.selectedUnit).Keys.ToList();
        rangeHighlight.HighlightTiles(movementRange);
    }

    // Skrypt mapy oblicza zasieg ruchu na podstawie pozycji i otoczenia jednostki
    public Dictionary<Vector2Int, Vector2Int?> GetMovementRangeFor(Unit selectedUnit)
    {
        return map.GetMovementRange(selectedUnit.transform.position, selectedUnit.CurrentMovementPoints);
    }

    // Koniec ruchu wylacza highlight
    public void ResetCharacterMovement()
    {
        if(rangeHighlight !=null)
            rangeHighlight.ClearHighlight();
        this.selectedUnit = null;
    }

    // Wykonujemy ruch
    public void HandleMovement(Vector3 endPosition)
    {
        // Tylko jesli wybrany obiekt to jednostka i ma on jeszcze dostepny ruch
        if (this.selectedUnit == null)
            return;

        if (this.selectedUnit.CanStillMove() == false)
            return;
        Vector2 direction = CalculateMovementDirection(endPosition);
        Vector2Int unitTilePosition = Vector2Int.FloorToInt((Vector2)this.selectedUnit.transform.position + direction);

        // Jesli wybrana kalefka zawiera sie w tych dotsepnych dla danej jednostki to mozemy wykonac ruch
        if(movementRange.Contains(unitTilePosition))
        {
            int cost = map.GetMovementCost(unitTilePosition);
            this.selectedUnit.HandleMovement(direction, cost);
            if (this.selectedUnit.CanStillMove())
            {
                PrepareMovementRange();
            }
            else
            {
                rangeHighlight.ClearHighlight();
            }
        }
        else
        {
            Debug.Log("Cant move here");
        }
    }

    // Obliczamy kierynek ruchu na podstawie aktualnego poleozenia jednostki i miejsca kursora na ekranie
    private Vector2 CalculateMovementDirection(Vector3 endPosition)
    {
        Vector2 direction = (endPosition - this.selectedUnit.transform.position);

        if (Mathf.Abs(direction.x) > Mathf.Abs(direction.y))
        {
            float sign = Mathf.Sign(direction.x);
            direction = Vector2.right * sign;
        }
        else
        {
            float sign = Mathf.Sign(direction.y);
            direction = Vector2.up * sign;
        }
        return direction;
    }
}