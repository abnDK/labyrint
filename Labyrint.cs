
public class Labyrint
{
    public Board Board { get; init; }

    private Pathfinder Pathfinder { get; }

    public Labyrint()
    {
        Board = new Board(6);

    }

    public (bool goalFound, int? stepsNeeded) SearchGoal(string startPos, string goalPos)
    {
        (bool goalFound, int? stepsNeeded) result = Pathfinder.FindGoal(startPos, goalPos, Board);

        Console.WriteLine(result);
        if (result.goalFound)
        {
            return result;
        }
        else
        {
            return (false, null);
        }

    }

    public void SearchGoalForce(string startPos, string goalPos)
    {
        Pathfinder.FindGoalForce(startPos, goalPos, Board);
    }

}