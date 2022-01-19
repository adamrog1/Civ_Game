using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class TurnBasedManager : MonoBehaviour
{
    Queue<EnemyTurnTaker> enemyQueue = new Queue<EnemyTurnTaker>();
    public UnityEvent OnBlockPlayerInput, OnUnblockPlayerInput;

    public void NextTurn()
    {
        OnBlockPlayerInput?.Invoke();
        SystemTurn();
        EnemiesTurn();
    }

    private void SystemTurn()
    {
        foreach (SystemTurnTaker turnTaker in FindObjectsOfType<SystemTurnTaker>())
        {
            turnTaker.WaitTurn();

        }
    }

    private void EnemiesTurn()
    {
        enemyQueue = new Queue<EnemyTurnTaker>(FindObjectsOfType<EnemyTurnTaker>());
        StartCoroutine(EnemyTurnTaker(enemyQueue));
    }

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

public interface ITurnDependant
{
    void WaitTurn();
}