using System.Collections.Generic;

public class Dijkstra
{
    public static List<NodeType> GetPath<NodeType>(
            IWeightedGraph<NodeType> graph,
            NodeType startNode, NodeType endNode,
            int maxiterations = 1000)
            where NodeType : notnull
    {
        var openQueue = new List<NodeType>() { startNode };
        var distance = new Dictionary<NodeType, float>();
        var previous = new Dictionary<NodeType, NodeType>();

        distance[startNode] = 0;
        List<NodeType> outputPath = new List<NodeType>();

        int i; for (i = 0; i < maxiterations; ++i)
        {
            if (openQueue.Count == 0) break;

            NodeType searchFocus = GetMinDistanceNode(openQueue, distance);
            openQueue.Remove(searchFocus);

            if (searchFocus.Equals(endNode))
            {
                outputPath.Add(endNode);
                while (previous.ContainsKey(searchFocus))
                {
                    searchFocus = previous[searchFocus];
                    outputPath.Add(searchFocus);
                }
                outputPath.Reverse();
                return outputPath;
            }

            foreach (var (neighbor, cost) in graph.NeighborsWithCost(searchFocus))
            {
                float newDistance = distance[searchFocus] + cost;

                if (!distance.ContainsKey(neighbor) || newDistance < distance[neighbor])
                {
                    distance[neighbor] = newDistance;
                    previous[neighbor] = searchFocus;

                    if (!openQueue.Contains(neighbor)) openQueue.Add(neighbor);
                }
            }
        }
        return new List<NodeType>();
    }

    private static NodeType GetMinDistanceNode<NodeType>(List<NodeType> openQueue, Dictionary<NodeType, float> distance)
        where NodeType : notnull
    {
        NodeType minNode = openQueue[0];
        float minDist = distance[minNode];

        foreach (var node in openQueue)
        {
            if (distance.ContainsKey(node) && distance[node] < minDist)
            {
                minDist = distance[node];
                minNode = node;
            }
        }
        return minNode;
    }
}
