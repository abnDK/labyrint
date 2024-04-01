namespace Labyrint
{

    public class Labyrint
    {
        public Board Board { get; init; }

        private Pathfinder Pathfinder { get; init; }

        public Labyrint()
        {
            Board = new Board(9);
            Pathfinder = new Pathfinder();
        }



        public void SearchGoalForce(string startPos, string goalPos)
        {

            List<PathFoundBoard> results = Pathfinder.FindGoalForce(startPos, goalPos, Board);
            foreach (PathFoundBoard pfb in results)
            {
                pfb.Board.renderField();
            }
        }

    }
}