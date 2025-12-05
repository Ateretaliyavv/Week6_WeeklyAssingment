using UnityEngine;
using UnityEngine.Tilemaps;

/**
 * Handles collecting special tiles (goat, boat, pick)
 * and updates the allowed tiles and the world accordingly.
 */
public class PlayerTileCollector : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private Tilemap worldTilemap;   //The Tilemap that contains all world tiles
    [SerializeField] private AllowedTiles allowedTiles;

    [Header("Pickup Tiles")]
    [SerializeField] private TileBase goatTile;      //Goat pickup tile
    [SerializeField] private TileBase boatTile;      //Boat pickup tile
    [SerializeField] private TileBase pickTile;   //Pickaxe pickup tile

    [Header("Terrain Tiles")]
    [SerializeField] private TileBase mountainTile;  //Mountain tile (becomes passable after goat)
    [SerializeField] private TileBase shalowWaterTile;  //Water tile (becomes passable after boat)
    [SerializeField] private TileBase mediumWaterTile;
    [SerializeField] private TileBase deepWaterTile;
    [SerializeField] private TileBase grassTile;     //Grass tile (replaces mountain with pickaxe)

    private bool hasGoat = false;
    private bool hasBoat = false;
    private bool hasPick = false;

    private Vector3Int lastCellPosition;
    private bool hasLastCell = false;

    private void Update()
    {
        if (worldTilemap == null) return;

        //Get the grid cell position where the player currently is
        Vector3Int cellPos = worldTilemap.WorldToCell(transform.position);

        //Run logic only when the player actually enters a new cell
        if (!hasLastCell || cellPos != lastCellPosition)
        {
            OnEnterCell(cellPos);
            lastCellPosition = cellPos;
            hasLastCell = true;
        }
    }

    private void OnEnterCell(Vector3Int cellPos)
    {
        TileBase tile = worldTilemap.GetTile(cellPos);
        if (tile == null) return;

        //Goat pickup
        if (tile == goatTile)
        {
            hasGoat = true;

            //Remove the goat tile from the map
            worldTilemap.SetTile(cellPos, grassTile);

            //Allow crossing mountains
            if (mountainTile != null && allowedTiles != null)
                allowedTiles.AddTile(mountainTile);

            return;
        }

        //Boat pickup
        if (tile == boatTile)
        {
            hasBoat = true;

            //Remove the boat tile from the map
            worldTilemap.SetTile(cellPos, grassTile);

            //Allow crossing water
            if (shalowWaterTile != null && mediumWaterTile != null && deepWaterTile != null && allowedTiles != null)
            {
                allowedTiles.AddTile(shalowWaterTile);
                allowedTiles.AddTile(mediumWaterTile);
                allowedTiles.AddTile(deepWaterTile);
            }
            return;
        }

        //Pick pickup
        if (tile == pickTile)
        {
            hasPick = true;

            //Remove the pickaxe tile from the map
            worldTilemap.SetTile(cellPos, grassTile);
            allowedTiles.AddTile(mountainTile);

            return;
        }

        //Using pickaxe - turn mountain into grass
        if (hasPick && tile == mountainTile)
        {
            if (grassTile != null)
            {
                worldTilemap.SetTile(cellPos, grassTile);
            }
        }
    }
}
