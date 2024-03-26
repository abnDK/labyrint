using System.Linq;

public class Pathfinder
{
    public static (bool goalFound, int? stepsNeeded) FindGoal(string startPos, string goalPos, Board board)
    {


        Dictionary<string, (int distance, string previousPosition, int distanceFromGoal)> distances; // position in format "x,y" and distance as integer and position in format "x,y" of previous node

        distances = DoDijkstra(startPos, goalPos, board);

        foreach (var distance in distances)
        {
            Console.WriteLine($"Pos: {distance.Key}");
            Console.WriteLine($"Dist: {distance.Value.distance}\nPrev: {distance.Value.previousPosition}\nDistFromGoal: {distance.Value.distanceFromGoal}");
        }

        if (distances.Where(node => node.Key == goalPos).ToList().Count() > 0)
        {
            Console.WriteLine("GOAL REACHED!");
            return (true, distances.Where(node => node.Key == goalPos).Select(node => node.Value.distance).First());
        }

        return (false, null);
    }

    public static (List<Board> boards, bool bruteForce) FindGoalForce(string startPos, string goalPos, List<Board> baseBoards, List<Board> candidates)
    {
        // boards: A list of boards from start board to final board where goal can be reached
        // insertsNeeded: How many inserts are needed before goal becomes reachable
        // bruteForce: if true, all versions will be used for next gen, if false, only versions with lower straightdistance between available area and goal is used for next gen


        // THIS DOES DFS - WE WANT BFS... 
        // maybe make function without candidates, and instead add poteentialnextgenerations to list we iterate over
        // list get populated with new generations, if candidate doesnt result in hit
        // but somehow, new generations should have their ancestor/candidate prepended!??

        foreach (Board candidate in candidates)
        {
            (bool goalFound, int? stepsNeeded) result = FindGoal(startPos, goalPos, candidate);

            if (result.goalFound)
            {

                baseBoards.Add(candidate);

                return (baseBoards, true);
            }
            else
            {
                List<Board> potentialNewGenerations = candidate.PotentialNewGenerations();

                List<Board> nextGenBaseBoards = new List<Board>();
                nextGenBaseBoards.AddRange(baseBoards);
                nextGenBaseBoards.Add(candidate);

                return FindGoalForce(startPos, goalPos, nextGenBaseBoards, potentialNewGenerations);

            }


        }

        foreach (Board candidate in newCandidates)
        {
            List<Board> nextGenBaseBoards = baseBoards;
            nextGenBaseBoards.Add(candidate);
            FindGoalForce(startPos, goalPos, nextGenBaseBoards, )
        }

        // check rows reverse and add to boardsToDijkstra

        // Check columns regular and add to boardsToDijkstra

        // check columns reverse and add to boardsToDijkstra

        // for each candidate board, recursive call FindGoalForce with candidate added to boards.


        return (boardsToDijkstra, 0, false);


    }

    private static Dictionary<string, (int distance, string previousPosition, int distanceFromGoal)> DoDijkstra(string startPos, string goalPos, Board board)
    {
        List<Path> paths = board.getPaths();
        List<string> visited = new List<string>();
        Dictionary<string, (int distance, string previousPosition, int distanceFromGoal)> distances = new Dictionary<string, (int distance, string previousPosition, int distanceFromGoal)>();

        // usually dijkstra is used on weighted vertices (paths), where it makes sense to recalc the length if a node is revisited.
        // but in this case, all vertices are equal to 1, so if a node is revisited in a later iteration, it will be a longer path and no need to update.
        // this we can filter out any nodes already in the distances dictionary.

        // toVisit.Add(startPos);
        distances[startPos] = (0, startPos, StraightDistance(startPos, goalPos));
        string current = startPos;

        // as long as there is neighbours found in paths not already added to distances dictionary
        while (distances.Values.Count() > visited.Count())
        {

            // add all neighbours to distances dictionary with distance + 1 (that are not already in distances dictionary)
            List<string> toNeighbours = paths
                .Where(p => p.From == current &&
                    !distances.Keys.Contains(p.To))
                .Select(p => p.To).ToList();

            List<string> fromNeighbours = paths
                .Where(p => p.To == current &&
                    !distances.Keys.Contains(p.From))
                .Select(p => p.From).ToList();

            toNeighbours.AddRange(fromNeighbours);
            foreach (string neighbour in toNeighbours)
            {
                distances[neighbour] = (distances[current].distance + 1, current, StraightDistance(neighbour, goalPos));
            }

            visited.Add(current);

            // if unvisited neighbours left, visit them to check for unknown neighbours
            if (distances.Count() > visited.Count())
            {
                int shortestDistanceToUnvisitedNode = distances.Where(node => !visited.Contains(node.Key)).Min(node => node.Value.distance);
                KeyValuePair<string, (int distance, string previousPosition, int distanceFromGoal)> closestNeighbour = distances
                    .Where(node => !visited.Contains(node.Key) &&
                           node.Value.distance == shortestDistanceToUnvisitedNode)
                    .First();

                current = closestNeighbour.Key;
            }


        }

        return distances;

    }

    private static int StraightDistance(string startPos, string goalPos)
    {
        // Calc distance in "straight" line between start and goal,
        // This is the distance if no obstacles/borders are between 2 points
        // Calculated by moving horisontal, then vertical
        // ie. From: 1,1 - To: 3,2 - straightDistance: 2 (horisontal) + 1 (vertical) = 3

        int startX = int.Parse(startPos.Split(",")[0]);
        int startY = int.Parse(startPos.Split(",")[1]);
        int goalX = int.Parse(goalPos.Split(",")[0]);
        int goalY = int.Parse(goalPos.Split(",")[1]);

        int distanceX = int.Abs(startX - goalX);
        int distanceY = int.Abs(startY - goalY);

        return distanceX + distanceY;



    }

    public static void howManyInsertToFindGoal()
    {
        // if dijkstra returns false, try an insertion and rerun until dijkstra returns true.
        // what should it return? How to communicate solution/insertions to make goal available?
    }
}