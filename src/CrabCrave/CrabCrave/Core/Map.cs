using System.ComponentModel;
using System.Windows;
using System.Windows.Controls.Primitives;
using System.Windows.Data;
using System.Windows.Media;
using System.Xml.Linq;
using CrabCrave;
using CrabCrave.Core;

public class Map : INotifyPropertyChanged
{
    public Node[,] map;
    public int rowEff;
    public int colEff;
    public int treasureCount;

    public Node[,] Maps
    {
        get { return map; }
        set
        {
            if (map != value)
            {
                map = value;
                OnPropertyChanged(nameof(Maps));
            }
        }
    }

    public int RowEff
    {
        get { return rowEff; }
        set
        {
            if (rowEff != value)
            {
                rowEff = value;
                OnPropertyChanged(nameof(RowEff));
            }
        }
    }

    public int ColEff
    {
        get { return colEff; }
        set
        {
            if (colEff != value)
            {
                colEff = value;
                OnPropertyChanged(nameof(ColEff));
            }
        }
    }

    public int TreasureCount
    {
        get { return treasureCount; }
        set
        {
            if (treasureCount != value)
            {
                treasureCount = value;
                OnPropertyChanged(nameof(TreasureCount));
            }
        }
    }

    public event PropertyChangedEventHandler PropertyChanged;

    protected virtual void OnPropertyChanged(string propertyName)
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }

    public Map(int rows, int columns)
    {
        this.map = new Node[rows, columns];
        this.treasureCount = 0;
        for (int i = 0; i < rows; i++)
        {
            for (int j = 0; j < columns; j++)
            {
                this.map[i, j] = new Node(i, j, 0, 0, Brushes.WhiteSmoke);
            }
        }
        this.rowEff = rows;
        this.colEff = columns;
    }

    public void GenerateMap(List<List<Node>> listOfListNode)
    {
        rowEff = listOfListNode.Count;
        colEff = listOfListNode[0].Count;

        int rows = listOfListNode.Count;
        int cols = listOfListNode[0].Count;

        for (int i = 0; i < rows; i++)
        {
            for (int j = 0; j < cols; j++)
            {
                if (listOfListNode[i][j].isTreasure())
                {
                    treasureCount++;
                }
                map[i, j] = listOfListNode[i][j];
            }
        }
    }

    /* GETTER */
    public (int, int) getStart()
    {
        // returns the index of the starting point
        for (int i = 0; i < this.rowEff; i++)
        {
            for (int j = 0; j < this.colEff; i++)
            {
                if (map[i, j].isKrustyKrab())
                {
                    return (i, j);
                }
            }
        }

        return (-1, -1); // to handle the return error
    }

    /* SETTER */
    public void setStartInMap(int x, int y)
    {
        this.map[x, y].setPath();
    }

    public void setPathInMap(int x, int y)
    {
        this.map[x, y].setPath();
    }

    // Assume all the index input is correct
    public bool isUpAvailable(int x, int y)
    {
        if (x == 0)
        {
            return false;
        }
        else
        {
            if ((this.map[x - 1, y].isPath() || this.map[x - 1, y].isTreasure() || this.map[x - 1, y].isKrustyKrab()) && !this.map[x - 1, y].hasBeenVisited())
            {
                return true;
            }
            else
            {
                return false;
            }
        }
    }

    public bool isRightAvailable(int x, int y)
    {
        if (y == this.colEff - 1)
        {
            return false;
        }
        else
        {
            if ((this.map[x, y + 1].isPath() || this.map[x, y + 1].isTreasure() || this.map[x, y + 1].isKrustyKrab()) && !this.map[x, y + 1].hasBeenVisited())
            {
                return true;
            }
            else
            {
                return false;
            }
        }
    }

    public bool isLeftAvailable(int x, int y)
    {
        if (y == 0)
        {
            return false;
        }
        else
        {
            if ((this.map[x, y - 1].isPath() || this.map[x, y - 1].isTreasure() || this.map[x, y - 1].isKrustyKrab()) && !this.map[x, y - 1].hasBeenVisited())
            {
                return true;
            }
            else
            {
                return false;
            }
        }
    }

    public bool isDownAvailable(int x, int y)
    {
        if (x == this.rowEff - 1)
        {
            return false;
        }
        else
        {
            if ((this.map[x + 1, y].isPath() || this.map[x + 1, y].isTreasure() || this.map[x + 1, y].isKrustyKrab()) && !this.map[x + 1, y].hasBeenVisited())
            {
                return true;
            }
            else
            {
                return false;
            }
        }
    }

    public void printMap()
    { // for simple terminal view
        for (int i = 0; i < this.rowEff; i++)
        {
            for (int j = 0; j < this.colEff; j++)
            {
                if (this.map[i, j].isKrustyKrab())
                {
                    Console.Write("K");
                }
                else if (this.map[i, j].isTreasure())
                {
                    Console.Write("T");
                }
                else if (this.map[i, j].isPath())
                {
                    Console.Write("P");
                }
                else if (!this.map[i, j].isPath())
                {
                    Console.Write(".");
                }
            }
            Console.WriteLine("");
        }
    }

    public int getTreasureCount()
    {
        return this.treasureCount;
    }
}