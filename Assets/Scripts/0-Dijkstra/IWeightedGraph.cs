using System.Collections.Generic;

public interface IWeightedGraph<T>
{
    // function to get neighbors of a node along with the cost to reach them
    IEnumerable<(T neighbor, float cost)> NeighborsWithCost(T node);
}