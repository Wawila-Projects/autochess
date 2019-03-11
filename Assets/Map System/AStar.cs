using System;
using System.Collections.Generic;
using Priority_Queue;

/* TODO
    Hacer que los tiles ocupados cuesten mas para que el
    algoritmo automaticamente encuentre caminos alternativos 
    sin tener que buscar por los vecinos de cada Tile.
 */

public static class AStar
{
    public static List<Tile> FindPath(Tile start, Tile goal)
    {
        var cameFrom = new Dictionary<Tile, Tile>();
        var costSoFar = new Dictionary<Tile, int>();
        cameFrom[start] = null;
        costSoFar[start] = 0;

        var frontier = new SimplePriorityQueue<Tile>();
        frontier.Enqueue(start, 0);

        while (frontier.Count > 0)
        {
            var current = frontier.Dequeue();

            if(current == null || current.isObstacle) continue;

            if (current == goal)
                break;

            foreach (var tile in current.Neighbors)
            {
                var next = tile.GetComponent<Tile>();

                if(next == null || next.isObstacle || next.IsOccupied) continue;

                var newCost = costSoFar[current] + 1;

                if (costSoFar.ContainsKey(next) && newCost >= costSoFar[next]) continue;

                costSoFar[next] = newCost;
                var priority = newCost + Heuristic(goal, next);
                frontier.Enqueue(next, priority);
                cameFrom[next] = current;
            }

        }

        return GetPath(cameFrom, goal);
    }

    private static float Heuristic(Tile a, Tile b)
    {
        if(a.isObstacle || b.isObstacle || 
           a.IsOccupied || b.IsOccupied)
        {
            return 1000f;
        }
        return a.Hex.GetDistance(b.Hex);
    }

    private static List<Tile> GetPath(IDictionary<Tile, Tile> dict, Tile start)
    {
        if (!dict.ContainsKey(start))
            return new List<Tile>();

        var path = new List<Tile> { start };
        var current = dict[start];

        while (current != null)
        {
            path.Add(current);
            current = dict[current];
        }

        path.Reverse();

        return path;
    }
}


//*
// * 
//frontier = PriorityQueue()
//frontier.put(start, 0)
//came_from = {}
//cost_so_far = {}
//came_from[start] = None
//cost_so_far[start] = 0

//while not frontier.empty():
//   current = frontier.get()

//   if current == goal:
//      break

//   for next in graph.neighbors(current):
//      new_cost = cost_so_far[current] + graph.cost(current, next)
//      if next not in cost_so_far or new_cost < cost_so_far[next]:
//         cost_so_far[next] = new_cost
//         priority = new_cost + heuristic(goal, next)
//         frontier.put(next, priority)
//         came_from[next] = current
// */
