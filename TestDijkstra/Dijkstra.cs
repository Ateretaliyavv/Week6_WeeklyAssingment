// Dijkstra.cs
using System.Collections.Generic;
using System.Linq;

public class Dijkstra
{
    public static List<NodeType> GetPath<NodeType>(
            IWeightedGraph<NodeType> graph, 
            NodeType startNode, NodeType endNode, 
            int maxiterations = 1000)
            where NodeType : notnull
    {
        // 1. מבני נתונים
        var openQueue = new List<NodeType>() { startNode };
        var distance = new Dictionary<NodeType, float>();
        var previous = new Dictionary<NodeType, NodeType>();
        
        distance[startNode] = 0; // עלות ההגעה להתחלה היא אפס
        List<NodeType> outputPath = new List<NodeType>();
        
        int i; for (i = 0; i < maxiterations; ++i)
        {
            if (openQueue.Count == 0)
            {
                break; // אין עוד צמתים לחקור
            }

            // פונקציית הליבה: בחירת הצומת עם העלות הכוללת הנמוכה ביותר
            NodeType searchFocus = GetMinDistanceNode(openQueue, distance);
            openQueue.Remove(searchFocus);

            if (searchFocus.Equals(endNode))
            {
                // **נמצא היעד - שחזור הנתיב (זהה ל-BFS)**
                outputPath.Add(endNode);
                while (previous.ContainsKey(searchFocus))
                {
                    searchFocus = previous[searchFocus];
                    outputPath.Add(searchFocus);
                }
                outputPath.Reverse();
                return outputPath;
            }

            // חקור את השכנים של הצומת הנוכחי
            foreach (var (neighbor, cost) in graph.NeighborsWithCost(searchFocus))
            {
                // העלות החדשה להגיע לשכן
                float newDistance = distance[searchFocus] + cost;

                // אם לא ביקרנו בצומת, או שנמצאה דרך זולה יותר להגיע אליו:
                if (!distance.ContainsKey(neighbor) || newDistance < distance[neighbor])
                {
                    distance[neighbor] = newDistance; // עדכן את העלות
                    previous[neighbor] = searchFocus; // עדכן את הקודם בנתיב
                    
                    if (!openQueue.Contains(neighbor))
                    {
                        openQueue.Add(neighbor); // הוסף את השכן לרשימת הפתוחים
                    }
                }
            }
        }
        
        // אם הלולאה נגמרה ולא נמצא נתיב
        return new List<NodeType>();
    }

    // פונקציית עזר: מוצאת את הצומת עם המרחק הכי קטן מבין אלו שברשימה הפתוחה
    private static NodeType GetMinDistanceNode<NodeType>(List<NodeType> openQueue, Dictionary<NodeType, float> distance)
    {
        // הערה: חלק זה הוא ה"צוואר בקבוק" אם הרשימה גדלה מאוד. 
        // שימוש ב-PriorityQueue מובנה היה מייעל את הפונקציה הזו מאוד.
        
        NodeType minNode = openQueue.First();
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
