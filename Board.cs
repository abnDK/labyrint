public class Board : ICloneable
{
    public List<List<Block>> Field { get; set; }

    private int Size { get; init; }

    private Block AvailableBlock { get; set; }

    public Board(int size)
    {
        Field = initField(size);
        Size = size;
    }

    private List<List<Block>> initField(int size)
    {

        if (size % 2 == 0)
        {
            // can only build fields of uneven size in order to keep outer rows/columns fixed.
            size++;
        }

        List<List<Block>> newField = new List<List<Block>>();

        for (int i = 0; i < size; i++)
        {

            List<Block> newRow = new List<Block>();
            newField.Add(newRow);

            for (int j = 0; j < size; j++)
            {
                Block newNode = new Block(j, i, "test");
                newRow.Add(newNode);
            }
        }

        AvailableBlock = new Block(-1, -1, "initial available block");

        return newField;
    }

    public void renderField(bool position = false)
    {

        // header column count
        Console.Write("   ");

        for (int columnIndex = 0; columnIndex < Field[0].Count; columnIndex++)
        {
            Console.Write($"{columnIndex}   ");
        }

        Console.Write("\n");

        for (int i = 0; i < Field.Count; i++)
        {

            // column count
            Console.Write($"{i} ");

            for (int j = 0; j < Field[i].Count; j++)
            {
                if (position)
                {
                    Console.Write($"{Field[i][j].Position()}  ");
                }
                else
                {
                    Console.Write($"{Field[i][j].Sides.Show()} ");

                }

            }
            Console.WriteLine();
        }
    }

    public List<Block> getRow(int index)
    {
        return Field[index];
    }

    public List<Block> getColumn(int index)
    {

        List<Block> column = new List<Block>();

        for (int i = 0; i < Field.Count; i++)
        {
            column.Add(Field[i][index]);
        }

        return column;
    }

    private void writeColumn(int index, List<Block> column)
    {
        for (int i = 0; i < Field.Count; i++)
        {
            Field[i][index] = column[i];
        }
    }

    public List<Path> getPaths()
    {
        List<Path> paths = new List<Path>();

        for (int i = 0; i < Field.Count; i++)
        {
            for (int j = 0; j < Field[i].Count; j++)
            {
                // this algo scans board from top left to bottom right checking for right and bottom neighbour.
                // no need to skip any expect when at right or bottom edge.
                // with this approach we dont need to look for duplicates as all potential paths are only checked once

                // clear all previous .Connected bools
                Field[i][j].Connected = false;

                // right neighbour
                if (Field[i][j].Sides.Right && j < Field[i].Count - 1)
                {
                    if (Field[i][j + 1].Sides.Left)
                    {
                        paths.Add(new Path(
                            Field[i][j].Position(),
                            Field[i][j + 1].Position()
                        ));

                        Field[i][j].Connected = true;
                        Field[i][j + 1].Connected = true;
                    }
                }

                // bottom neighbour
                if (Field[i][j].Sides.Bottom && i < Field.Count - 1)
                {
                    if (Field[i + 1][j].Sides.Top)
                    {
                        paths.Add(new Path(
                            Field[i][j].Position(),
                            Field[i + 1][j].Position()
                        ));

                        Field[i][j].Connected = true;
                        Field[i + 1][j].Connected = true;
                    }
                }

            }
        }

        return paths;
    }

    public void insertBlockIntoRow(int index, bool reverse)
    {
        // insert block into row and returns the left over block
        //      YYY
        //  X > YYY
        //      YYY
        // 
        //      YYY
        //      XYY > Y
        //      YYY

        List<Block> row = getRow(index);

        if (reverse) row.Reverse();



        row.Insert(0, (Block)AvailableBlock.Clone());

        AvailableBlock = row[row.Count - 1];
        row.RemoveAt(row.Count - 1);
        AvailableBlock.X = -1;
        AvailableBlock.Y = -1;

        if (reverse) row.Reverse();

        // update all x and y's of blocks.
        for (int i = 0; i < row.Count; i++)
        {
            row[i].X = i;
            row[i].Y = index;
        }

    }

    public void insertBlockIntoColumn(int index, bool reverse)
    {
        // insert block into column and returns the left over block
        // 
        //       X
        //       v 
        // 
        //      YYY
        //      YYY
        //      YYY
        // 
        //      YXY
        //      XYY
        //      YYY
        //
        //       v 
        //       Y

        List<Block> column = getColumn(index);

        if (reverse) column.Reverse();

        column.Insert(0, (Block)AvailableBlock.Clone());

        AvailableBlock = column[column.Count - 1];
        column.RemoveAt(column.Count - 1);
        AvailableBlock.X = -1;
        AvailableBlock.Y = -1;

        if (reverse) column.Reverse();

        // update all x and y's of blocks.
        for (int i = 0; i < column.Count; i++)
        {
            column[i].X = index;
            column[i].Y = i;
        }

        writeColumn(index, column);

    }

    public List<Board> PotentialNewGenerations()
    {
        List<Board> newGenerations = new List<Board>();

        // get board and check if goal is found
        List<int> dynamicRowsColumnsIndexes = DynamicRowsColumnsIndexes();

        // Check rows regular and add to boardsToDijkstra
        foreach (int index in dynamicRowsColumnsIndexes)
        {

            for (int orientationIndex = 0; orientationIndex < 4; orientationIndex++)
            {

                Board insertFromTopBoard = (Board)Clone();
                insertFromTopBoard.insertBlockIntoColumn(index, false);
                newGenerations.Add(insertFromTopBoard);

                Board insertFromBottomBoard = (Board)Clone();
                insertFromBottomBoard.insertBlockIntoColumn(index, true);
                newGenerations.Add(insertFromBottomBoard);

                Board insertFromLeftBoard = (Board)Clone();
                insertFromLeftBoard.insertBlockIntoRow(index, false);
                newGenerations.Add(insertFromLeftBoard);

                Board insertFromRightBoard = (Board)Clone();
                insertFromRightBoard.insertBlockIntoRow(index, true);
                newGenerations.Add(insertFromRightBoard);

                RotateAvailableBlock();

            }


        }

        return newGenerations;
    }

    public List<int> DynamicRowsColumnsIndexes()
    {

        List<int> indexes = new List<int>();

        for (int i = 0; i < Size; i++)
        {
            if (i % 2 == 1)
            {
                indexes.Add(i);
            }
        }

        return indexes;
    }

    public object Clone()
    {
        Board clonedBoard = new Board(Size);
        clonedBoard.Field = Field;
        clonedBoard.AvailableBlock = AvailableBlock;
        return clonedBoard;
    }

    public void RotateAvailableBlock()
    {
        AvailableBlock.Rotate();
    }
}

public class Path
{
    public string From { get; init; }
    public string To { get; init; }

    public Path(string from, string to)
    {
        From = from;
        To = to;
    }
}