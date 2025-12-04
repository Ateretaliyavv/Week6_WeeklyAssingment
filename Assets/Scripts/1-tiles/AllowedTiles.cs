using System.Linq;
using UnityEngine;
using UnityEngine.Tilemaps;

/**
 * This component just keeps a list of allowed tiles.
 * Such a list is used both for pathfinding and for movement.
 */
public class AllowedTiles : MonoBehaviour  {
    [SerializeField] TileBase[] allowedTiles = null;

    public bool Contains(TileBase tile) {
        return allowedTiles.Contains(tile);
    }

    public TileBase[] Get() { return allowedTiles;  }

    // Adds a tile to the allowed tiles list at runtime  (if it's not already present).
    public void AddTile(TileBase tile)
    {
        if (tile == null) return;
        if (allowedTiles.Contains(tile)) return;

        var list = allowedTiles.ToList();
        list.Add(tile);
        allowedTiles = list.ToArray();
    }
}
