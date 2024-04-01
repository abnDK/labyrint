namespace Labyrint
{


    public class PathFoundBoard
    {
        public Board Board { get; set; }

        public string StartPos { get; set; }

        public string GoalPos { get; set; }

        public string ClosestPosition { get; set; }

        public int StepsFromStartToClosest { get; set; }

        public int DistanceFromClosestToGoal { get; set; }

        public double DistanceFromGoalGenerationAvarage { get; set; } = 1000;

        public double DistanceFromGoalGenerationBestFive { get; set; } = 1000;

        public double DistanceFromGoalGenerationMinimum { get; set; } = 1000;

        public int NextGenSurvivalDistance { get; set; } = 1000;

        public PathFoundBoard(Board board, string startPos, string goalPos, string closestPosition, int stepsFromStartToClosest, int distanceFromClosestToGoal)
        {
            Board = board;
            StartPos = startPos;
            GoalPos = goalPos;
            ClosestPosition = closestPosition;
            StepsFromStartToClosest = stepsFromStartToClosest;
            DistanceFromClosestToGoal = distanceFromClosestToGoal;
        }
    }
}