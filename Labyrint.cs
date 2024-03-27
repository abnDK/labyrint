
public class Labyrint
{
    public Board Board { get; init; }

    private Pathfinder Pathfinder { get; } = new();

    public Labyrint()
    {
        Board = new Board(5);

    }

    public (bool goalFound, int stepsNeeded, int distanceFromGoal) SearchGoal(string startPos, string goalPos)
    {
        (bool goalFound, int stepsNeeded, int distanceFromGoal) result = Pathfinder.FindGoal(startPos, goalPos, Board);

        return result;


    }

    public void SearchGoalForce(string startPos, string goalPos)
    {
        List<Board> results = Pathfinder.FindGoalForce(startPos, goalPos, Board);
        foreach (Board b in results)
        {
            b.renderField();
        }
    }

}