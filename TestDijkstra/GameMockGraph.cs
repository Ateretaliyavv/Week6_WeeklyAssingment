public class GameMockGraph : IWeightedGraph<GridNode>
{
    // Tile Types (Without Goat/Bonus tiles)
    public enum TileType { Grass, Bush, ShallowWater, MediumWater, DeepWater, Hills, Mountain, Swamp }

    // Weights configuration
    private Dictionary<TileType, float> weights = new()
    {
        { TileType.Grass, 1f },
        { TileType.Bush, 2f },
        { TileType.ShallowWater, 4f },
        { TileType.MediumWater, 5f },
        { TileType.DeepWater, 6f }, 
        { TileType.Hills, 6f },     
        { TileType.Mountain, 8f },
        { TileType.Swamp, 10f }
    };

    private Dictionary<GridNode, TileType> map = new();

    public void SetTile(int x, int y, TileType type)
    {
        map[new GridNode { X = x, Y = y }] = type;
    }

    public IEnumerable<(GridNode neighbor, float cost)> NeighborsWithCost(GridNode node)
    {
        var dirs = new[] { 
            new GridNode{X=0, Y=1}, new GridNode{X=0, Y=-1}, 
            new GridNode{X=1, Y=0}, new GridNode{X=-1, Y=0} 
        };

        foreach (var dir in dirs)
        {
            GridNode next = new GridNode { X = node.X + dir.X, Y = node.Y + dir.Y };
            if (map.ContainsKey(next))
            {
                float cost = weights[map[next]];
                yield return (next, cost);
            }
        }
    }

    public float GetCost(GridNode a, GridNode b)
    {
        if (map.ContainsKey(b)) return weights[map[b]];
        return 0;
    }
}