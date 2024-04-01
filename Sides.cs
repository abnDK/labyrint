namespace Labyrint
{


    public class Sides : ICloneable
    {
        public bool Top;

        public bool Bottom;

        public bool Right;

        public bool Left;

        public Sides()
        {
            string topRightBottomLeft = RandomizeSides();

            Top = topRightBottomLeft[0] == '0' ? false : true;
            Right = topRightBottomLeft[1] == '0' ? false : true;
            Bottom = topRightBottomLeft[2] == '0' ? false : true;
            Left = topRightBottomLeft[3] == '0' ? false : true;
        }

        public Sides(string topRightBottomLeft)
        {
            // expects 4 char string of 0 or 1's.
            // 0 == closed, 1 == open
            if (topRightBottomLeft.Length != 4) throw new ArgumentException("Only string of length 4 is a valid input");

            int sidesSum = 0;

            for (int i = 0; i < 4; i++)
            {

                if (topRightBottomLeft[i] != '0' && topRightBottomLeft[i] != '1') throw new ArgumentException("Input can only consist of 1's or 0's");

                sidesSum += int.Parse(topRightBottomLeft[i].ToString());
            }

            if (sidesSum <= 1 || sidesSum > 3)
            {
                topRightBottomLeft = RandomizeSides();
            }


            Top = topRightBottomLeft[0] == '0' ? false : true;
            Right = topRightBottomLeft[1] == '0' ? false : true;
            Bottom = topRightBottomLeft[2] == '0' ? false : true;
            Left = topRightBottomLeft[3] == '0' ? false : true;






        }

        public string RandomizeSides()
        {
            Random rand = new Random();

            int[] randomizedSides = new int[4];

            for (int i = 0; i < 4; i++)
            {
                randomizedSides[i] = rand.Next(2);
            }

            if (randomizedSides.Sum() <= 1 || randomizedSides.Sum() == 4)
            {
                // do 2 (corner/straight) or 3 sides (T-shape)

                // set all sides to closed before creating new
                for (int i = 0; i < 4; i++)
                {
                    randomizedSides[i] = 0;
                }

                int originIndex = rand.Next(4); // index to start new sides from 0-3


                if (rand.Next(2) == 0)
                {

                    // corner
                    if (rand.Next(2) == 0)
                    {
                        // corner
                        randomizedSides[originIndex] = 1;
                        randomizedSides[(originIndex + 1) % 4] = 1;


                    }
                    else
                    {
                        // straight
                        randomizedSides[originIndex] = 1;
                        randomizedSides[(originIndex + 2) % 4] = 1;
                    }
                }
                else
                {
                    // T
                    randomizedSides[originIndex] = 1;
                    randomizedSides[(originIndex + 1) % 4] = 1;
                    randomizedSides[(originIndex + 2) % 4] = 1;
                }

            }

            return $"{randomizedSides[0]}{randomizedSides[1]}{randomizedSides[2]}{randomizedSides[3]}";

        }

        public override string ToString()
        {
            return $"{(Left ? "<" : " ")}{(Top && Bottom ? "H" : Top ? "A" : Bottom ? "V" : " ")}{(Right ? ">" : " ")}";
        }

        public object Clone()
        {
            return new Sides() { Top = Top, Bottom = Bottom, Left = Left, Right = Right };
        }


    }
}
