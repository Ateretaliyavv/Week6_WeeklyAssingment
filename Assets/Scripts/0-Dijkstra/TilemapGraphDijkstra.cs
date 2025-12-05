using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

/**
 * Graph that represents a tilemap, using only the allowed tiles, with weights for Dijkstra's algorithm.
 */
public class TilemapGraphDijkstra : MonoBehaviour, IWeightedGraph<Vector3Int>
{
    [SerializeField] Tilemap tilemap = null;
    [SerializeField] AllowedTiles allowedTiles = null;

    [Header("tiles")]
    [SerializeField] TileBase deepWaterTile;
    [SerializeField] TileBase mediumWaterTile;
    [SerializeField] TileBase shalowWaterTile;
    [SerializeField] TileBase bushTile;
    [SerializeField] TileBase grassTile;
    [SerializeField] TileBase swampTile;
    [SerializeField] TileBase hillsTile;
    [SerializeField] TileBase mountainTile;
    [SerializeField] TileBase goatTile;
    [SerializeField] TileBase boatTile;
    [SerializeField] TileBase pickTile;

    [Header("Set weight for tiles")]
    [SerializeField] float deepWaterTileWeight = 6f;
    [SerializeField] float mediumWaterTileWeight = 5f;
    [SerializeField] float shalowWaterTileWeight = 4f;
    [SerializeField] float bushTileWeight = 2f;
    [SerializeField] float grassTileWeight = 1f;
    [SerializeField] float swampTileWeight = 10f;
    [SerializeField] float hillsTileWeight = 6f;
    [SerializeField] float mountainTileWeight = 8f;
    [SerializeField] float goatTileWeight = 0f;
    [SerializeField] float boatTileWeight = 0f;
    [SerializeField] float pickTileWeight = 0f;





    static Vector3Int[] directions = {
            new Vector3Int(-1, 0, 0),
            new Vector3Int(1, 0, 0),
            new Vector3Int(0, -1, 0),
            new Vector3Int(0, 1, 0),
    };


    public IEnumerable<(Vector3Int neighbor, float cost)> NeighborsWithCost(Vector3Int node)
    {
        foreach (var direction in directions)
        {
            Vector3Int neighborPos = node + direction;
            TileBase neighborTile = tilemap.GetTile(neighborPos);

            if (allowedTiles.Contains(neighborTile))
            {
                // calculate cost based on tile type
                float cost = GetTileCost(neighborTile);
                yield return (neighborPos, cost);
            }
        }
    }

    // function to determine cost based on tile type
    private float GetTileCost(TileBase tile)
    {
        if (tile == bushTile) return bushTileWeight;
        if (tile == grassTile) return grassTileWeight;
        if (tile == swampTile) return swampTileWeight;
        if (tile == hillsTile) return hillsTileWeight;
        if (tile == mountainTile) return mountainTileWeight;
        if (tile == shalowWaterTile) return shalowWaterTileWeight;
        if (tile == mediumWaterTile) return mediumWaterTileWeight;
        if (tile == deepWaterTile) return deepWaterTileWeight;
        if (tile == goatTile) return goatTileWeight;
        if (tile == boatTile) return boatTileWeight;
        if (tile == pickTile) return pickTileWeight;
        return 1f;
    }
}
