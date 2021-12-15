using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Unit : MonoBehaviour
{
    [SerializeField]
    private int maxMovementPoints = 30;
    private int currentMovementPoints;

    public UnityEvent FinishedMoving;

    

    void Start()
    {
        currentMovementPoints = maxMovementPoints;
    }

    public bool CanStillMove()
    {
        return currentMovementPoints > 0;
    }

    public void HandleMovement(Vector3 cardinalDirection, int movementCost)
    {
        if(currentMovementPoints - movementCost < 0)
        {
            Debug.LogError($"Brak punktow ruchu {currentMovementPoints}/{movementCost}");
            return;
        }

        currentMovementPoints -= movementCost;

        if (currentMovementPoints <= 0)
            FinishedMoving?.Invoke();

        transform.position += cardinalDirection;
    }
}
