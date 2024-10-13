using System;
using System.Collections.Generic;

public class Grid : IEquatable<Grid>
{
    private char[,] cells;

    public Grid(char[,] cells)
    {
        this.cells = cells;
    }

    public bool Equals(Grid other)
    {
        if (other == null) return false;
        if (ReferenceEquals(this, other)) return true;

        for (int i = 0; i < cells.GetLength(0); i++)
        {
            for (int j = 0; j < cells.GetLength(1); j++)
            {
                if (cells[i, j] != other.cells[i, j])
                    return false;
            }
        }
        return true;
    }

    public override bool Equals(object obj)
    {
        if (obj is Grid)
        {
            return Equals((Grid)obj);
        }
        return false;
    }

    public override int GetHashCode()
    {
        int hash = 17;
        for (int i = 0; i < cells.GetLength(0); i++)
        {
            for (int j = 0; j < cells.GetLength(1); j++)
            {
                unchecked
                {
                    hash = hash * 31 + cells[i, j].GetHashCode();
                }
            }
        }
        return hash;
    }

    public void PrintGrid()
    {
        for (int i = 0; i < cells.GetLength(0); i++)
        {
            for (int j = 0; j < cells.GetLength(1); j++)
            {
                Console.Write(cells[i, j] + " ");
            }
            Console.WriteLine();
        }
    }
}



