using System.Text.Json.Serialization;

public class Program
{
    static void Main()
    {

        // ready for making a dfs algo looking for specific block based on list of paths (getPaths())

        Labyrint l = new Labyrint();

        Console.WriteLine(l.Board.AvailableBlock.Sides.ToString());

        l.Board.renderField();

        l.SearchGoalForce("1,1", "3,3");





    }
}

