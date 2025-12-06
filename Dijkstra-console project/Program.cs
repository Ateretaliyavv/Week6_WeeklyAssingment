using System;
using System.Collections.Generic;
using System.Linq;


class Program
{
    static void Main(string[] args)
    {
        Console.WriteLine("=== RUNNING 5 DIJKSTRA PATHFINDING TESTS ===\n");

        Test1_AvoidSwamp();         // Does it avoid the expensive swamp?
        Test2_MountainVsBushes();   // Does it prefer bushes over a mountain?
        Test3_TheUTurn();           // Can it perform a U-Turn to save cost?
        Test4_WaterCrossing();      // Does it choose the shallow water bridge?
        Test5_ImpossiblePath();     // Does it recognize when there is no path?

        Console.WriteLine("\n=== ALL TESTS COMPLETED ===");
    }

    // --- Test 1: Avoid Swamp (Classic) ---
    static void Test1_AvoidSwamp()
    {
        var graph = new GameMockGraph();
        
        // Scenario: We want to go from (0,0) to (2,0).
        // In the middle at (1,0), there is an expensive Swamp (Cost 10).
        // Above, there is cheap Grass (Cost 1).
        
        // Direct and expensive line
        graph.SetTile(0, 0, GameMockGraph.TileType.Grass); 
        graph.SetTile(1, 0, GameMockGraph.TileType.Swamp); // Cost 10
        graph.SetTile(2, 0, GameMockGraph.TileType.Grass); 

        // Cheap detour (above)
        graph.SetTile(0, 1, GameMockGraph.TileType.Grass);
        graph.SetTile(1, 1, GameMockGraph.TileType.Grass);
        graph.SetTile(2, 1, GameMockGraph.TileType.Grass);

        // Detour Calculation: (0,0)->(0,1)->(1,1)->(2,1)->(2,0)
        // 4 steps on Grass = 4.
        // Direct line would cost 10.
        
        RunTest("1. Avoid Swamp", graph, new GridNode{X=0,Y=0}, new GridNode{X=2,Y=0}, 4f);
    }

    // --- Test 2: Mountains vs. Bushes ---
    static void Test2_MountainVsBushes()
    {
        var graph = new GameMockGraph();
        // Check if it prefers walking through 3 Bushes (Cost 2 each = 6)
        // or crossing 3 Mountains (Cost 8 each = 24).
        
        // Start and End
        graph.SetTile(0, 0, GameMockGraph.TileType.Grass);
        graph.SetTile(4, 0, GameMockGraph.TileType.Grass);

        // Short path - Mountains
        graph.SetTile(1, 0, GameMockGraph.TileType.Mountain);
        graph.SetTile(2, 0, GameMockGraph.TileType.Mountain);
        graph.SetTile(3, 0, GameMockGraph.TileType.Mountain);
        // Cost: 8+8+8 + 1 (end step) = 25 (Very expensive!)

        // Detour path - Bushes
        graph.SetTile(0, 1, GameMockGraph.TileType.Bush); // 2
        graph.SetTile(1, 1, GameMockGraph.TileType.Bush); // 2
        graph.SetTile(2, 1, GameMockGraph.TileType.Bush); // 2
        graph.SetTile(3, 1, GameMockGraph.TileType.Bush); // 2
        graph.SetTile(4, 1, GameMockGraph.TileType.Bush); // 2
        // Cost: 2+2+2+2+2 + 1 (end step) = 11

        RunTest("2. Bushes vs Mountain", graph, new GridNode{X=0,Y=0}, new GridNode{X=4,Y=0}, 11f);
    }

    // --- Test 3: The U-Turn ---
    static void Test3_TheUTurn()
    {
        // Sometimes the shortest path requires going backwards first.
        var graph = new GameMockGraph();
        
        // S(0,0) ----> E(2,0)
        // Direct path blocked by Deep Water
        graph.SetTile(0, 0, GameMockGraph.TileType.Grass);
        graph.SetTile(1, 0, GameMockGraph.TileType.DeepWater); // Cost 6
        graph.SetTile(2, 0, GameMockGraph.TileType.Grass);

        // There is a "Grass Bridge" below
        graph.SetTile(0, -1, GameMockGraph.TileType.Grass);
        graph.SetTile(1, -1, GameMockGraph.TileType.Grass);
        graph.SetTile(2, -1, GameMockGraph.TileType.Grass);

        // Direct path cost: 6 + 1 = 7.
        // Detour (Down, Right, Up):
        // (0,-1)[1] -> (1,-1)[1] -> (2,-1)[1] -> (2,0)[1] = Total 4.
        
        RunTest("3. U-Turn Detour", graph, new GridNode{X=0,Y=0}, new GridNode{X=2,Y=0}, 4f);
    }

    // --- Test 4: Water Crossing Selection ---
    static void Test4_WaterCrossing()
    {
        var graph = new GameMockGraph();
        
        GridNode start = new GridNode{X=0, Y=0};
        GridNode end = new GridNode{X=2, Y=0};

        graph.SetTile(0, 0, GameMockGraph.TileType.Grass);
        graph.SetTile(2, 0, GameMockGraph.TileType.Grass);

        // Middle is blocked by Swamp (Cost 10)
        graph.SetTile(1, 0, GameMockGraph.TileType.Swamp); 
        
        // Side path has Shallow Water (Cost 4)
        graph.SetTile(0, 1, GameMockGraph.TileType.Grass);
        graph.SetTile(1, 1, GameMockGraph.TileType.ShallowWater); 
        graph.SetTile(2, 1, GameMockGraph.TileType.Grass);

        // Direct (Swamp): 10 + 1 = 11.
        // Detour (Shallow Water): 1(Grass) + 4(Water) + 1(Grass) + 1(End) = 7.
        
        RunTest("4. Shallow vs Swamp", graph, start, end, 7f);
    }

    // --- Test 5: Impossible Path ---
    static void Test5_ImpossiblePath()
    {
        var graph = new GameMockGraph();
        graph.SetTile(0, 0, GameMockGraph.TileType.Grass);
        
        // Target is far away and disconnected
        GridNode target = new GridNode{X=10, Y=10}; 

        RunTest("5. Impossible Path", graph, new GridNode{X=0,Y=0}, target, 0f);
    }


    // --- Helper Function to Run Tests ---
    static void RunTest(string testName, GameMockGraph graph, GridNode start, GridNode end, float expectedCost)
    {
        Console.ForegroundColor = ConsoleColor.Yellow;
        Console.WriteLine($"Running Test: {testName}...");
        Console.ResetColor();

        // Call Dijkstra
        var path = Dijkstra.GetPath(graph, start, end);
        
        // Calculate actual cost
        float actualCost = 0;
        if (path.Count > 0) {
            for(int i=0; i<path.Count-1; i++) 
                actualCost += graph.GetCost(path[i], path[i+1]);
        }

        string pathStr = path.Count > 0 ? string.Join(" -> ", path) : "NO PATH FOUND";
        
        // Check success
        // Success means Actual Cost equals Expected Cost
        // (If expected is 0, path count should also be 0)
        bool success = (Math.Abs(actualCost - expectedCost) < 0.01f) && (expectedCost > 0 == path.Count > 0);

        if (success)
        {
            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine($"   [PASSED] Cost: {actualCost}");
            Console.ResetColor();
            Console.WriteLine($"   Path: {pathStr}");
        }
        else
        {
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine($"   [FAILED] Expected Cost: {expectedCost}, Got: {actualCost}");
            Console.ResetColor();
            Console.WriteLine($"   Path: {pathStr}");
        }
        Console.WriteLine(new string('-', 50));
    }
}

