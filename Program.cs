using System.Text.Json.Serialization;

public class Program
{
    static void Main()
    {

        // ready for making a dfs algo looking for specific block based on list of paths (getPaths())

        Labyrint l = new Labyrint();

        List<Path> oldPaths = l.Board.getPaths();

        l.Board.renderField(false);


        Console.WriteLine();
        foreach (Path p in oldPaths)
        {
            Console.WriteLine($"{p.From} => {p.To}");
        }


        Block b = new Block(9, 9, "test");
        b.Sides.Top = true;
        b.Sides.Bottom = true;
        b.Sides.Right = true;
        b.Sides.Left = true;

        Block b2 = new Block(9, 9, "test");
        b2.Sides.Top = true;
        b2.Sides.Bottom = true;
        b2.Sides.Right = true;
        b2.Sides.Left = true;

        Block b3 = new Block(9, 9, "test");
        b3.Sides.Top = true;
        b3.Sides.Bottom = true;
        b3.Sides.Right = true;
        b3.Sides.Left = true;

        Console.WriteLine($"New1: {b.Sides.Show()}");

        Block leftOver1 = l.Board.insertBlockIntoRow(b, 0, false);
        l.Board.insertBlockIntoRow(b2, 0, false);
        l.Board.insertBlockIntoRow(b3, 0, false);
        Console.WriteLine($"Old1: {leftOver1.Sides.Show()}");
        l.Board.renderField(false);


        List<Path> newPaths = l.Board.getPaths();



        Console.WriteLine();
        foreach (Path p in newPaths)
        {
            Console.WriteLine($"{p.From} => {p.To}");
        }



        l.SearchGoalForce("4,3", "3,3");





    }
}

