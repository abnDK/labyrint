public class Block : ICloneable, IEquatable<Block>
{
    public int X { get; set; }
    public int Y { get; set; }
    public string Name { get; init; }

    public bool Connected { get; set; }

    public Sides Sides { get; set; }

    public Block(int x, int y, string name, string? sidesConfig = null)
    {
        X = x;
        Y = y;
        Name = name;
        if (sidesConfig != null)
        {
            Sides = new Sides(sidesConfig);
        }
        else
        {
            Sides = new Sides();
        }
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
        Block clone = new Block(X, Y, $"{Name} - cloned");
        clone.Sides = (Sides)Sides.Clone();
        clone.Connected = false;
        return clone;
    }

    public bool Equals(Block? other)
    {

        if (other == null)
        {
            throw new ArgumentException("Cannot compare to a null object");
        }


        return (
            X == other.X &&
            Y == other.Y &&
            Name == other.Name &&
            Connected == other.Connected &&
            Sides.Top == other.Sides.Top &&
            Sides.Right == other.Sides.Right &&
            Sides.Bottom == other.Sides.Bottom &&
            Sides.Left == other.Sides.Left
        );
    }

}



