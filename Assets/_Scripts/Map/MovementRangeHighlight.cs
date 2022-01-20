using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class MovementRangeHighlight : MonoBehaviour
{
    // Potrzebujemy danych o tilemapie na ktorej maja zostac podswietlone kafelki
    [SerializeField]
    private Tilemap highlightTilemap;
    [SerializeField]
    private TileBase highlightTile;

    // Funckja przelacza wybrane kafelki na podswietlona
    public void HighlightTiles(IEnumerable<Vector2Int> cellPositions)
    {
        ClearHighlight();
        foreach(Vector2Int tilePosition in cellPositions)
        {
            highlightTilemap.SetTile((Vector3Int)tilePosition, highlightTile);
        }
    }

    // Usuwamy podswietlenie ze wszystkich kafelek (np przy koncu tury)
    public void ClearHighlight()
    {
        highlightTilemap.ClearAllTiles();
    }
}
