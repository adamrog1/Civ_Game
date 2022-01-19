using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class GraphSearch
{
    // Okreslanie zasiegu ruchu dla danej jednostki przy uzyciu algorytmu BFS (przeszukiwanie wszerz)
    public static Dictionary<Vector2Int, Vector2Int?> BFS
        (MapGrid mapGraph, Vector2Int startPoint, int movementPoints)
    {
        Dictionary<Vector2Int, Vector2Int?> visitedNodes = new Dictionary<Vector2Int, Vector2Int?>();
        Dictionary<Vector2Int, int> costSoFar = new Dictionary<Vector2Int, int>();
        Queue<Vector2Int> nodesToVisitQueue = new Queue<Vector2Int>();

        nodesToVisitQueue.Enqueue(startPoint);
        costSoFar.Add(startPoint, 0);
        visitedNodes.Add(startPoint, null);

        // Musimy odwiedzic kazde pole dookola jednostki
        while(nodesToVisitQueue.Count > 0)
        {
            Vector2Int currentNode = nodesToVisitQueue.Dequeue();
            // Dla kazdego sasiedniego pola musimy odwiedzic kolejnego sasiada jednoczesnie zapamietujac koszt pokonania dotychczaswoej trasy
            foreach(Vector2Int neighbourPosition in mapGraph.GetNeighboursFor(currentNode))
            {
                // Pomijamy wode i gory
                if (mapGraph.CheckIfPositionIsValid(neighbourPosition) == false)
                    continue;

                // Liczymy koszt ruchu
                int nodeCost = mapGraph.GetMovementCost(neighbourPosition);
                int currentCost = costSoFar[currentNode];
                int newCost = currentCost + nodeCost;

                // Jesli koszt ruchu jest mniejszy niz liczba punktow ruchu
                if(newCost <= movementPoints)
                {
                    // Jesli ten punk odwiedzamy pierwszy raz to zapamietujemy dane ruchu w ten punkt
                    if (!visitedNodes.ContainsKey(neighbourPosition))
                    {
                        visitedNodes[neighbourPosition] = currentNode;
                        costSoFar[neighbourPosition] = newCost;
                        nodesToVisitQueue.Enqueue(neighbourPosition);
                    }
                    // JEsli aktualnie policzoy koszt do tego punktu jest lepszy niz poprzedni to dokonujemy zamiany
                    else if (costSoFar[neighbourPosition] > newCost)
                    {
                        costSoFar[neighbourPosition] = newCost;
                        visitedNodes[neighbourPosition] = currentNode;
                    }
                }
            }
        }
        return visitedNodes;
    }
}
