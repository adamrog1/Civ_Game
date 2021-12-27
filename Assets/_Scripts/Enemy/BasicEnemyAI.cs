using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BasicEnemyAI : MonoBehaviour, IEnemyAI
{
    public event Action TurnFinished;

    private Unit unit;
    private CharacterMovement characterMovement;
    [SerializeField]
    private FlashFeedback selectionFeedback;
    private void Awake()
    {
        characterMovement = FindObjectOfType<CharacterMovement>();
        unit = GetComponent<Unit>();
        selectionFeedback = GetComponent<FlashFeedback>();
    }

    public void StartTurn()
    {
        Debug.Log("TAKES A TURN");
        selectionFeedback.PlayFeedback();
        Dictionary<Vector2Int, Vector2Int?> movementRange = characterMovement.GetMovementRangeFor(unit);
        List<Vector2Int> path = GetPathToRandomPositon(movementRange);
        Queue<Vector2Int> pathQueue = new Queue<Vector2Int>(path);
        StartCoroutine(MoveUnit(pathQueue));
    }

    private List<Vector2Int> GetPathToRandomPositon(Dictionary<Vector2Int, Vector2Int?> movementRange)
    {
        List<Vector2Int> possibleDestination = movementRange.Keys.ToList();
        possibleDestination.Remove(Vector2Int.RoundToInt(transform.position));

        Vector2Int selectedDestination=possibleDestination[UnityEngine.Random.Range(0, possibleDestination.Count)];
        List<Vector2Int> listToReturn = GetPathTo(selectedDestination, movementRange);
        return listToReturn;
        //Debug.Log(movementRange.Keys.ToList()[2]);
     //   return new List<Vector2Int> { movementRange.Keys.ToList()[2] };
    }

    private List<Vector2Int> GetPathTo(Vector2Int destination, Dictionary<Vector2Int, Vector2Int?> movementRange)
    {
        List<Vector2Int> path = new List<Vector2Int>();
        path.Add(destination);
        while (movementRange[destination] != null) {
            path.Add(movementRange[destination].Value);
            destination = movementRange[destination].Value;
        }
        path.Reverse();
        return path.Skip(1).ToList();
    }

    private IEnumerator MoveUnit(Queue<Vector2Int> path)
    {
        yield return new WaitForSeconds(0.5F);
        if (unit.CanStillMove() == false || path.Count <= 0)
        {
            FinishMovement();
            yield break;
        }
        Vector2Int pos = path.Dequeue();
        Vector3Int direction = Vector3Int.RoundToInt(new Vector3(pos.x + 0.5F, pos.y + 0.5F, 0) - transform.position);
        unit.HandleMovement(direction, 0);
        Debug.Log("Finished");
        yield return new WaitForSeconds(0.2F);
        if (path.Count > 0)
        {
            StartCoroutine(MoveUnit(path));
        }
        else {
            yield return new WaitForSeconds(0.5F);
            FinishMovement();
        }
    }

    private void FinishMovement()
    {
        TurnFinished?.Invoke();
        selectionFeedback.StopFeedback();
    }
}
