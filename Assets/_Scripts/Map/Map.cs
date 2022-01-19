using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Tilemaps;

public class Map : MonoBehaviour
{
    Dictionary<Vector3Int, GameObject> buildings = new Dictionary<Vector3Int, GameObject>();

    // Dane potrzebne do rysowania mapy w scenie Unity
    [SerializeField]
    private Tilemap islandCollidersTilemap, forestTilemap, mountainsTilemap;

    private List<Vector2Int> islandTiles, forestTiles, mountainTiles, emptyTiles;

    // I dane dla testowania, uzywane przez Gizmos
    [SerializeField]
    private bool showEmpty, showMountains, showForest;

    // Tutaj zwracamy tylko pola dostepne w zasiegu danej jednostki, zeby nie liczyc wszystkich na mapie
    // Uzywamy BFS opisanego w GraphSearch.cs
    public Dictionary<Vector2Int, Vector2Int?> GetMovementRange(Vector3 worldPosition, int currentMovementPoints)
    {
        Vector3Int cellWorldPosition = GetCellWorldPositionFor(worldPosition);
        return GraphSearch.BFS(mapGrid, (Vector2Int)cellWorldPosition, currentMovementPoints);
    }

    private MapGrid mapGrid;

    // Przy wlaczeniu gry pobieramy ze wszystkich warst siatki dane o kafelkach mapy
    private void Awake()
    {
        forestTiles = GetTilemapWorldPositionsFrom(forestTilemap);
        mountainTiles = GetTilemapWorldPositionsFrom(mountainsTilemap);
        islandTiles = GetTilemapWorldPositionsFrom(islandCollidersTilemap);
        emptyTiles = GetEmptyTiles(islandTiles, forestTiles.Concat(mountainTiles).ToList());
        PrepareMapGrid();
    }

    public int GetMovementCost(Vector2Int cellWorldPosition)
    {
        return mapGrid.GetMovementCost(cellWorldPosition);
    }

    // Zapisujemy dane do siatki mapy na podstawie ulozonych kafelek w roznych warstwach
    private void PrepareMapGrid()
    {
        mapGrid = new MapGrid();
        mapGrid.AddToGrid(forestTilemap.GetComponent<TerrainTypeReference>().GetTerrainData(), forestTiles);
        mapGrid.AddToGrid(mountainsTilemap.GetComponent<TerrainTypeReference>().GetTerrainData(), mountainTiles);
        mapGrid.AddToGrid(islandCollidersTilemap.GetComponent<TerrainTypeReference>().GetTerrainData(), emptyTiles);
    }

    // Zwraca pozycje "wolnych" pol
    private List<Vector2Int> GetEmptyTiles(List<Vector2Int> islandTiles, List<Vector2Int> nonEmptyTiles)
    {
        HashSet<Vector2Int> emptyTilesHashset = new HashSet<Vector2Int>(islandTiles);
        emptyTilesHashset.ExceptWith(nonEmptyTiles);
        return new List<Vector2Int>(emptyTilesHashset);
    }


    private List<Vector2Int> GetTilemapWorldPositionsFrom(Tilemap tilemap)
    {
        List<Vector2Int> tempList = new List<Vector2Int>();
        foreach (Vector2Int cellPosition in tilemap.cellBounds.allPositionsWithin)
        {
            if (tilemap.HasTile((Vector3Int)cellPosition) == false)
                continue;

            Vector3Int worldPosition = GetWorldPositionFor(cellPosition);
            tempList.Add((Vector2Int)worldPosition);
        }
        return tempList;
    }

    // Tutaj zwraca pozucje calego panelu
    private Vector3Int GetCellWorldPositionFor(Vector3 worldPosition)
    {
        return Vector3Int.CeilToInt(islandCollidersTilemap.CellToWorld(islandCollidersTilemap.WorldToCell(worldPosition)));
    }

    // Funkcja zwraca vector z pozycja z mapy
    private Vector3Int GetWorldPositionFor(Vector2Int cellPosition)
    {
        return Vector3Int.CeilToInt(islandCollidersTilemap.CellToWorld((Vector3Int)cellPosition));
    }

    // Na jednym polu moze byc tylko jedna budowla, latwo mozemy sprawdzic czy wskazana pozycja zawiera juz budowle
    public void AddStructure(Vector3 worldPosition, GameObject structure)
    {
        Vector3Int position = GetCellWorldPositionFor(worldPosition);

        if (buildings.ContainsKey(position))
        {
            Debug.LogError($"There is a structure already at this position {worldPosition}");
            return;
        }

        buildings[position] = structure;
    }

    // Sprawdzamy poprawnosc pozycji
    public bool IsPositionInvalid(Vector3 worldPosition)
    {
        return buildings.ContainsKey(GetCellWorldPositionFor(worldPosition));
    }

    // Dla kazdej warstwy rysujemy kolka
    private void OnDrawGizmos()
    {
        if (Application.isPlaying == false)
            return;
        DrawGizomOf(emptyTiles, Color.white, showEmpty);
        DrawGizomOf(forestTiles, Color.yellow, showForest);
        DrawGizomOf(mountainTiles, Color.red, showMountains);
    }

    // Rysujemy kolka na niedostepnych polach (pomocne przy sprawdzaniu czy ruchy dostepne sa poprawne)
    private void DrawGizomOf(List<Vector2Int> tiles, Color color, bool isShowing)
    {
        if (isShowing)
        {
            Gizmos.color = color;
            foreach (Vector2Int pos in tiles)
            {
                Gizmos.DrawSphere(new Vector3(pos.x + 0.5f, pos.y + 0.5f, 0), 0.3f);
            }
        }
    }
}
