using System.Linq;
using System.Xml.XPath;

public class Pathfinder
{
    public static (bool goalFound, string closestPosition, int stepsNeeded, int distanceFromGoal) FindGoal(string startPos, string goalPos, Board board)
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
            return (true,
                    distances.Where(node => node.Value.distanceFromGoal == distances.Min(node => node.Value.distanceFromGoal)).Select(node => node.Key).First(),
                    distances.Where(node => node.Key == goalPos).Select(node => node.Value.distance).First(),
                    0);
        }

        return (false,
                distances.Where(node => node.Value.distanceFromGoal == distances.Min(node => node.Value.distanceFromGoal)).Select(node => node.Key).First(),
                distances.Where(node => node.Value.distanceFromGoal == distances.Min(node => node.Value.distanceFromGoal)).Select(node => node.Value.distance).First(),
                distances.Min(node => node.Value.distanceFromGoal));
    }

    public static List<Board> FindGoalForce(string startPos, string goalPos, Board motherBoard)
    {
        // boards: A list of boards from start board to final board where goal can be reached
        // insertsNeeded: How many inserts are needed before goal becomes reachable
        // bruteForce: if true, all versions will be used for next gen, if false, only versions with lower straightdistance between available area and goal is used for next gen

        Queue<(Board board, string closestPosition, int distanceFromGoal, List<(Board, string, int)> ancestors)> candidates = new Queue<(Board board, List<(Board board, string closestPosition, int distanceFromGoal)> ancestors)>();

        candidates.Enqueue((motherBoard, "0,0", motherBoard.Field.Count() * 2, new List<(Board board, string closestPosition, int distanceFromGoal)>()));

        int candidateNum = 0;

        while (candidates.Any())
        {
            var (candidate, candidateClosestPosition, candidateDistanceFromGoal, ancestors) = candidates.Dequeue();

            candidateNum++;
            Console.WriteLine($"Calculating candidate n. {candidateNum}");

            List<Board> newGenerations = candidate.NewGenerations();

            foreach (Board newGen in newGenerations)
            {

                (bool goalFound, string closestPosition, int stepsNeeded, int distanceFromGoal) newGenResult = FindGoal(startPos, goalPos, newGen);

                if (newGenResult.goalFound)
                {
                    ancestors.Add((newGen, newGenResult.closestPosition, newGenResult.distanceFromGoal));

                    return ancestors; // fix return value
                }

                if (candidateDistanceFromGoal > newGenResult.distanceFromGoal)
                {
                    List<Board> newGenAncestors = new List<Board>();

                    foreach (Board ancestor in ancestors)
                    {
                        newGenAncestors.Add((Board)ancestor.Clone(false));
                    }

                    newGenAncestors.Add(candidate);

                    // hvorfor? Format passer da når vi dequer på linje 47?
                    candidates.Enqueue((newGen, candidateClosestPosition, candidateDistanceFromGoal, newGenAncestors));
                }





            }

            (bool goalFound, int stepsNeeded, int distanceFromGoal) result = FindGoal(startPos, goalPos, candidate);

            if (result.goalFound)
            {
                ancestors.Add(candidate);

                return ancestors;
            }

            Console.WriteLine($"Calculation generation {ancestors.Count()}");

            if (candidate.Generation > 3) throw new Exception("Too many generations...");


            List<Board> newGenerations = candidate.NewGenerations();

            // new generation get candidate as ancestor.
            // if we let it be a ref, it can work as linked list. Ancestor has ancestor.

            foreach (Board newGen in newGenerations)
            {

                List<Board> newGenAncestors = new List<Board>();

                foreach (Board ancestor in ancestors)
                {
                    newGenAncestors.Add((Board)ancestor.Clone(false));
                }
                newGenAncestors.Add(candidate);
                candidates.Enqueue((newGen, newGenAncestors));

            }


        }

        throw new Exception("How did we end up here?");

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