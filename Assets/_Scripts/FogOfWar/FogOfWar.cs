using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class FogOfWar : MonoBehaviour
{
    [SerializeField]
    private Tilemap seaTilemap, fogOfWarTilemap;

    [SerializeField]
    private TileBase fowTile;

    // Przygotowanie tilemapy dla fow na podstawie wartosci ustalonych w panelu
    void Awake()
    {
        fogOfWarTilemap.size = seaTilemap.size;

        fogOfWarTilemap.
            BoxFill(
            seaTilemap.cellBounds.min,
            fowTile,
            seaTilemap.cellBounds.min.x,
            seaTilemap.cellBounds.min.y,
            seaTilemap.cellBounds.max.x,
            seaTilemap.cellBounds.max.y);

    }

    // Usuwanie fow w wyznaczonych przez jednostke miejscach
    public void ClearFOW(List<Vector2> positionsToClear)
    {
        foreach (Vector2 position in positionsToClear)
        {
            fogOfWarTilemap.SetTile(fogOfWarTilemap.WorldToCell(position), null);
        }
    }
}