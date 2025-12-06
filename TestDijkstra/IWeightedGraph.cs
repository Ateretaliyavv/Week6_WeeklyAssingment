// IWeightedGraph.cs
using System.Collections.Generic;

// T היא סוג הצומת (כמו Vector3Int ביוניטי)
public interface IWeightedGraph<T>
{
    // הפונקציה מחזירה רשימה של שכנים, כאשר כל שכן מגיע עם עלות המעבר אליו.
    IEnumerable<(T neighbor, float cost)> NeighborsWithCost(T node);
}
