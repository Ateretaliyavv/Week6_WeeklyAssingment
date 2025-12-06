# Unity week 5: Two-dimensional scene-building and path-finding

A project with step-by-step scenes illustrating how to construct a 2D scene using tilemaps,
and how to do path-finding using the BFS algorithm.

## Two changes were added to the original game:

1. We added the following items to the map:

    - A boat – when the player collects it, they can sail on water.

    - A goat – when the player collects it, they can climb mountains.

    - A pickaxe – when the player collects it, they can mine mountains and turn them into grass.

2. Instead of using the DFS algorithm, we implemented Dijkstra’s algorithm to find a fast route in a weighted graph.
Each type of tile has a speed value according to its terrain; for example, walking on mountains is slower than walking on hills, walking on grass is faster than walking on swamp, and shallow water is slower than deep water.
So instead of the shortest path chosen by DFS, Dijkstra will choose the fastest path.

**Game Instructions:**

To move the player, you can click on the desired location with the mouse or use the arrow keys to guide the player as you choose.

## Itch.io link: https://ateretaliya.itch.io/week6-weeklyassingment

Text explanations are available 
[here](https://github.com/gamedev-at-ariel/gamedev-5782) in folder 07.

## Cloning
To clone the project, you may need to install git lfs first:

    git lfs install 


## Credits

Graphics:
* [Ultima 4 Graphics](https://github.com/jahshuwaa/u4graphics) by Joshua Steele.

Online course:
* [Unity 2D](https://www.udemy.com/course/unitycourse/learn/lecture/10246496), a Udemy course by Gamedev.tv.
* [Unity RPG](https://www.gamedev.tv/p/unity-rpg/?product_id=1503859&coupon_code=JOINUS).

Procedural generation:
* [Habrador - Unity Programming Patterns](https://github.com/Habrador/Unity-Programming-Patterns#7-double-buffer)

Programming:
* Erel Segal-Halevi
* Ateret Cohen
* Teliya Vallerstein
