using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;


public class Unit : MonoBehaviour, ITurnDependant
{
    // Pobieramy informacje o zasiegu ruchu, danych jednostki i pliku audio
    private int currentMovementPoints;
    public UnityEvent FinishedMoving;
    private UnitData unitData;
    private AudioSource stepSound;
    public int CurrentMovementPoints { get => currentMovementPoints; }
    public event Action OnMove;

    [SerializeField]
    private LayerMask enemyDetectionLayer;

    private void Awake()
    {
        stepSound = GetComponent<AudioSource>();
        unitData = GetComponent<UnitData>();
    }

    // Na poczatku ustalany zasieg ruchu na podstawie danych w panelu Unity
    void Start()
    {
        ResetMovementPoints();
    }

    // Update danych
    private void ResetMovementPoints()
    {
        currentMovementPoints = unitData.Data.movementRange;
    }

    // Sprawdamy czy zostaly punkty ruchu
    public bool CanStillMove()
    {
        return currentMovementPoints > 0;
    }

    // Koniec tury resetuje punkty ruchu
    public void WaitTurn()
    {
        ResetMovementPoints();
    }

    // Jesli wykonywany jest ruch sprawdaamy czy jednostka ma wystarczajaco punktow ruchu
    public void HandleMovement(Vector3 cardinalDirection, int movementCost)
    {
        if(currentMovementPoints - movementCost < 0)
        {
            Debug.LogError($"Brak punktow ruchu {currentMovementPoints}/{movementCost}");
            return;
        }

        // Wykonujemy ruch jesli wszsytkie warunki sa spelnione
        // Odejmujemy wartosc wykonanego ruchu od punktow jednstki
        currentMovementPoints -= movementCost;
        GameObject enemyUnity = CheckIfEnemyUnitInDirection(cardinalDirection);
        if (enemyUnity == null)
        {
            transform.position += cardinalDirection;
            stepSound.Play();
            OnMove?.Invoke();
        }
        // Jesli ruch byl wykonany na pole z becnym wrogiem to nie wykonuejmy go tylko zadajemy obrazenia wrogowi
        else {
            PerformAttack(enemyUnity.GetComponent<Health>());
        }

      
        if (currentMovementPoints <= 0)
            FinishedMoving?.Invoke();

        
    }

    // Odejmyjemy punkty zycia wrogowi
    private void PerformAttack(Health health) {
        health.GetHit(unitData.Data.attackStrength);       
    }

    // Sprawdzamy czy na podanym polu znajduje sie jednostka wroga
    private  GameObject CheckIfEnemyUnitInDirection(Vector3 cardinalDirection) {
        RaycastHit2D hit = Physics2D.Raycast(transform.position, cardinalDirection, 1, enemyDetectionLayer);
        if (hit.collider != null) {
            return hit.collider.gameObject;
        }
        return null;
    
    }

    // Usuwanie jednostki
    public void DestroyUnit()
    {
        FinishedMoving?.Invoke();
        Destroy(gameObject);
    }

    // Koniec ruchu wylacza jednostke i jej animacje
    public void FinishMovement()
    {
        this.currentMovementPoints = 0;
        FinishedMoving?.Invoke();
    }
}
