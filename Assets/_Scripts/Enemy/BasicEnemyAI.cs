using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BasicEnemyAI : MonoBehaviour, IEnemyAI
{
    // Pobieramy informacje na temat stanu tury, zasiegu jednostki i jej animacji
    public event Action TurnFinished;

    private Unit unit;
    private CharacterMovement characterMovement;
    [SerializeField]
    private FlashFeedback selectionFeedback;
    [SerializeField]
    private AgentOutlineFeedback outlineFeedback;

    private void Awake()
    {
        characterMovement = FindObjectOfType<CharacterMovement>();
        unit = GetComponent<Unit>();
        selectionFeedback = GetComponent<FlashFeedback>();
        outlineFeedback = GetComponent<AgentOutlineFeedback>();
    }

    // Zacznamy ruch od animacji, policzenia zasiegu i wygenerowania losowej sciezki dla jednostki
    public void StartTurn()
    {
        selectionFeedback.PlayFeedback();
        outlineFeedback.Select();
        Dictionary<Vector2Int, Vector2Int?> movementRange = characterMovement.GetMovementRangeFor(unit);
        List<Vector2Int> path = GetPathToRandomPositon(movementRange);
        Queue<Vector2Int> pathQueue = new Queue<Vector2Int>(path);
        StartCoroutine(MoveUnit(pathQueue));
    }

    // Losujemy sciezke ruchu na podstawie dostepnych puntow ruchu
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

    // Obliczamy sciezke potrzebna do osiagniecia zadanego punktu na mapie
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

    // Wykonujemy ruchy zgodnie z ustalona sciezka
    private IEnumerator MoveUnit(Queue<Vector2Int> path)
    {
        yield return new WaitForSeconds(0.5F);
        if (unit.CanStillMove() == false || path.Count <= 0)
        {
            FinishMovement();
            yield break;
        }
        Vector2Int pos = path.Dequeue();
        // Przy kazdym ruchu pobieramy kierunek posuniecia ze sciezki
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

    // Przy zakonczeniu ruchu wylacza animacje i przelacza ture
    private void FinishMovement()
    {
        TurnFinished?.Invoke();
        selectionFeedback.StopFeedback();
        outlineFeedback.Deselect();
    }
}
