namespace Labyrint
{

    public class Dijkstra
    {

        public Dictionary<string, (int distance, string previousPosition, int distanceFromGoal)> Search(string startPos, string goalPos, Board board)
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

        public int StraightDistance(string startPos, string goalPos)
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
    }
}
