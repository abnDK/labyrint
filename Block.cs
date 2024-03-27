public class Block : ICloneable
{
    public int X { get; set; }
    public int Y { get; set; }
    public string Name { get; init; }

    public bool Connected { get; set; }

    public Sides Sides { get; set; }

    public Block(int x, int y, string name)
    {
        X = x;
        Y = y;
        Name = name;
        Sides = new Sides();
    }

    public string Position()
    {
        return $"{X},{Y}";
    }

    public void Rotate(bool clockwise = true)
    {
        if (clockwise)
        {
            bool tempTop = Sides.Top;
            Sides.Top = Sides.Left;
            Sides.Left = Sides.Bottom;
            Sides.Bottom = Sides.Right;
            Sides.Right = tempTop;
        }
        else
        {
            bool tempTop = Sides.Top;
            Sides.Top = Sides.Right;
            Sides.Right = Sides.Bottom;
            Sides.Bottom = Sides.Left;
            Sides.Left = tempTop;
        }
    }

    public object Clone()
    {
        Block clone = new Block(X, Y, Name);
        clone.Sides = (Sides)Sides.Clone();
        clone.Connected = false;
        return clone;
    }

}

public class Sides : ICloneable
{
    public bool Top;

    public bool Bottom;

    public bool Right;

    public bool Left;

    public Sides()
    {
        Random rand = new Random();

        int[] randomizedSides = new int[4];

        for (int i = 0; i < 4; i++)
        {
            randomizedSides[i] = rand.Next(2);
        }

        if (randomizedSides.Sum() == 4)
        {
            // if all sides open, set one to closed
            randomizedSides[rand.Next(4)] = 0;
        }

        if (randomizedSides.Sum() <= 1)
        {
            int randomSide = rand.Next(4);
            randomizedSides[randomSide] = 1;
            randomizedSides[randomSide % 2] = 1;

        }

        Top = randomizedSides[0] == 0 ? false : true;
        Bottom = randomizedSides[1] == 0 ? false : true;
        Right = randomizedSides[2] == 0 ? false : true;
        Left = randomizedSides[3] == 0 ? false : true;

    }

    public Sides(string topBottomRightLeft)
    {
        // expects 4 char string of 0 or 1's.
        // 0 == closed, 1 == open

        if (topBottomRightLeft == "1111" || topBottomRightLeft == "0000")
        {
            // TODO - RANDOMIZE RESULTS
            // don't accept 4 open or 4 closed sides, so randomize it instead
            Top = false;
            Bottom = true;
            Right = true;
            Left = true;
        }
        else
        {
            Top = topBottomRightLeft.Split()[0] == "0" ? false : true;
            Bottom = topBottomRightLeft.Split()[1] == "0" ? false : true;
            Right = topBottomRightLeft.Split()[2] == "0" ? false : true;
            Left = topBottomRightLeft.Split()[3] == "0" ? false : true;
        }

    }

    public override string ToString()
    {
        return $"{(Left ? "<" : " ")}{(Top && Bottom ? "H" : Top ? "A" : Bottom ? "V" : " ")}{(Right ? ">" : " ")}";
    }

    public object Clone()
    {
        Sides clonedSides = new Sides();
        clonedSides.Top = this.Top;
        clonedSides.Bottom = this.Bottom;
        clonedSides.Left = this.Left;
        clonedSides.Right = this.Right;
        return clonedSides;
    }


}

