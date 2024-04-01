

public class Pathfinder
{
    Dijkstra PathFindingAlgo { get; init; }

    public Pathfinder()
    {
        PathFindingAlgo = new Dijkstra();
        // if (PathFindingAlgo == null) throw new Exception("Pathfinder needs a Pathfinding Algo - Cannot be null");
    }

    public PathFoundBoard FindGoal(string startPos, string goalPos, Board board)
    {
        Dictionary<string, (int distance, string previousPosition, int distanceFromGoal)> distances; // position in format "x,y" and distance as integer and position in format "x,y" of previous node

        distances = PathFindingAlgo.Search(startPos, goalPos, board);



        if (distances.Where(node => node.Key == goalPos).ToList().Count() > 0)
        {
            Console.WriteLine("GOAL REACHED!");

            PathFoundBoard goalReachedBoard = new PathFoundBoard(
                board, startPos, goalPos,
                goalPos,
                distances.Where(node => node.Value.distanceFromGoal == 0).Select(node => node.Value.distance).First(),
                0
            );

            return goalReachedBoard;
        }

        PathFoundBoard closestPathBoard = new PathFoundBoard(
            board, startPos, goalPos,
            distances.Where(node => node.Value.distanceFromGoal == distances.Min(node => node.Value.distanceFromGoal)).Select(node => node.Value.previousPosition).First(),
            distances.Where(node => node.Value.distanceFromGoal == distances.Min(node => node.Value.distanceFromGoal)).Select(node => node.Value.distance).First(),
            distances.Where(node => node.Value.distanceFromGoal == distances.Min(node => node.Value.distanceFromGoal)).Select(node => node.Value.distanceFromGoal).First()


        );

        return closestPathBoard;
    }

    public List<PathFoundBoard> FindGoalForce(string startPos, string goalPos, Board motherBoard)
    {
        // boards: A list of boards from start board to final board where goal can be reached
        // insertsNeeded: How many inserts are needed before goal becomes reachable
        // bruteForce: if true, all versions will be used for next gen, if false, only versions with lower straightdistance between available area and goal is used for next gen

        var candidates = new Queue<(PathFoundBoard pathFoundBoard, List<PathFoundBoard> ancestors)>();

        candidates.Enqueue((
            new PathFoundBoard(motherBoard, startPos, goalPos, startPos, 0, PathFindingAlgo.StraightDistance(startPos, goalPos)),
            new List<PathFoundBoard>()
        ));

        int candidateNum = 0;
        int currentGeneration = 0;
        int limitNextGenAddedToCandidates = 500;

        while (candidates.Any())
        {
            // In this loop we;
            // each time a new generation is visited (it's BFS, so we go generation 0, generation 1, generetion 2 and so on)
            // we limit the amount of candidates of a given generation we wanna make new generations from // VAR LIMIT
            // we dont work with a specific limit of how big a distance the can be to the goal, we just pick the X best amount and continue with that
            // TODO EXTRAS
            // we might mix in some random tables?
            // give version number to a table, so we can track which generation and version in that generation the table is built up of

            (PathFoundBoard candidate, List<PathFoundBoard> ancestors) = candidates.Dequeue();

            if (candidate.Board.Generation > currentGeneration)
            {
                currentGeneration = candidate.Board.Generation;
                trimCandidates(limitNextGenAddedToCandidates, ref candidates, currentGeneration);
            }

            // candidat Gen logges
            // første gang ny generation mødes, regn avg ud for alle med den generation i candidates.
            // kan bruges når vi filtrerer nye generationer længere nede. Eks kun lade ny generation blive kandidater hvis de er bedre end 25 kvartilen af den nuværende generation.


            candidateNum++;
            if (candidateNum % 25 == 0)
            {
                Console.WriteLine($"Calculating candidate n. {candidateNum} - Generation n. {candidate.Board.Generation} - Distance from goal {candidate.DistanceFromClosestToGoal} - survival target {ancestors.Last().NextGenSurvivalDistance} - candidates queued {candidates.Count()} - candidates this gen {candidates.Where(c => c.pathFoundBoard.Board.Generation == candidate.Board.Generation).Count()} / next gen {candidates.Where(c => c.pathFoundBoard.Board.Generation == candidate.Board.Generation + 1).Count()}");
            }




            // Calculate new generations of current candidate
            List<Board> newGenerationBoards = candidate.Board.CalculateNewGenerations();

            List<PathFoundBoard> newGenerationsPathFoundBoards = new List<PathFoundBoard>();

            newGenerationBoards.ForEach(delegate (Board newGenBoard)
                {
                    newGenerationsPathFoundBoards.Add(this.FindGoal(startPos, goalPos, newGenBoard));
                }
            );


            // If we reach goal, return the PathFoundBoard with it's ancestors in a list
            if (newGenerationsPathFoundBoards.Where(node => node.DistanceFromClosestToGoal == 0).Any())
            {

                ancestors.Add(
                    newGenerationsPathFoundBoards.Where(node => node.DistanceFromClosestToGoal == 0).First()
                );

                return ancestors;
            }



            // if new generations of current candidate did not reach goal, add them all to the list of candidates
            foreach (PathFoundBoard newGen in newGenerationsPathFoundBoards)
            {
                // for every newGen better than previous gen avarage
                // add as candidate to candidates + list of ancestors (candidate, and candidates ancestors)

                List<PathFoundBoard> newGenAncestors = new List<PathFoundBoard>();

                newGenAncestors.AddRange(ancestors);
                newGenAncestors.Add(candidate);

                candidates.Enqueue((newGen, newGenAncestors));


            }

        }

        motherBoard.renderField();



        throw new Exception("Ran out of possible candidate - this was the closed we could get!");

    }




    public static void howManyInsertToFindGoal()
    {
        // if dijkstra returns false, try an insertion and rerun until dijkstra returns true.
        // what should it return? How to communicate solution/insertions to make goal available?
    }

    public void trimCandidates(int limit, ref Queue<(PathFoundBoard pathFoundBoard, List<PathFoundBoard> ancestors)> candidates, int? generation = null)
    {
        // trims queue of candidates to the limit by the distance from closest position to goal position

        var orderedCandidates = new Queue<(PathFoundBoard pathFoundBoard, List<PathFoundBoard> ancestors)>();

        if (generation == null)
        {
            generation = candidates.Peek().pathFoundBoard.Board.Generation;
        }

        var candidatesQuery = candidates.Where(c => c.pathFoundBoard.Board.Generation == generation).OrderBy(c => c.pathFoundBoard.DistanceFromClosestToGoal);


        foreach (var candidate in candidatesQuery.Take(limit))
        {
            orderedCandidates.Enqueue(candidate);
        }

        // empty queue
        while (candidates.Any())
        {
            candidates.Dequeue();
        }

        candidates = orderedCandidates;

    }
}
