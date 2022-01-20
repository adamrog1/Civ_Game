using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class TurnBasedManager : MonoBehaviour
{
    // Pobieramy informacje o ruchach wrogich jednostek 
    Queue<EnemyTurnTaker> enemyQueue = new Queue<EnemyTurnTaker>();
    public UnityEvent OnBlockPlayerInput, OnUnblockPlayerInput;

    // Funckja blokuje interakcje gracza z interfejsem i wykonuje ruch przeciwnika
    public void NextTurn()
    {
        OnBlockPlayerInput?.Invoke();
        SystemTurn();
        EnemiesTurn();
    }
    // Przelaczanie miedzy turami
    private void SystemTurn()
    {
        foreach (SystemTurnTaker turnTaker in FindObjectsOfType<SystemTurnTaker>())
        {
            turnTaker.WaitTurn();

        }
    }

    // Wykonywanie ruchow przeciwnika
    private void EnemiesTurn()
    {
        enemyQueue = new Queue<EnemyTurnTaker>(FindObjectsOfType<EnemyTurnTaker>());
        StartCoroutine(EnemyTurnTaker(enemyQueue));
    }

    // Konczenie tury przeciwnika i wznawianie tury gracza
    private IEnumerator EnemyTurnTaker(Queue<EnemyTurnTaker> enemyQueue)
    {
        while (enemyQueue.Count > 0) {
            EnemyTurnTaker turnTaker = enemyQueue.Dequeue();
            turnTaker.TakeTurn();
            yield return new WaitUntil(turnTaker.IsFinished);
            turnTaker.Reset();
        }
        Debug.Log("PLAYER TAKES TURN");
        PlayerTurn();
    }

    // Rozpoczynamy ture gracza, oblokowujemy wszystkie elementy UI i interakcje z nimi
    private void PlayerTurn()
    {
        foreach (PlayerTurnTaker turnTaker in FindObjectsOfType<PlayerTurnTaker>())
        {
            turnTaker.WaitTurn();
            Debug.Log($"Unit {turnTaker.name} is waiting");
        }
        Debug.Log("New turn ready");
        OnUnblockPlayerInput?.Invoke();
    }
}

// Interfejs uzywany w innych klasach m.in. do blokowania elementow interfejsu
public interface ITurnDependant
{
    void WaitTurn();
}