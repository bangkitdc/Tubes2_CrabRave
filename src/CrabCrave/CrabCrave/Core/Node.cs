using System.ComponentModel;
using System.Windows.Media;
public class Node : INotifyPropertyChanged
{
    public enum Value
    {
        Wall, // 0
        Path, // 1
        Treasure, // 2
        KrustyKrab // 3
    }
    public enum Progress
    {
        Unvisited, // 0
        InVisit, // 1
        Visited // 2
    }
    public int x;
    public int y;
    public Value val;
    public Progress prog;
    public Brush color;
    public Node(int x, int y, int value, int progress, Brush color)
    {
        this.x = x;
        this.y = y;
        this.val = (Value)value;
        this.prog = (Progress)progress;
        this.color = color;
    }

    public int X
    {
        get { return this.x; }
        set
        {
            if (x != value)
            {
                x = value;
                OnPropertyChanged(nameof(X));
            }
        }
    }

    public int Y
    {
        get { return y; }
        set
        {
            if (y != value)
            {
                y = value;
                OnPropertyChanged(nameof(Y));
            }
        }
    }

    public Value Val
    {
        get { return val; }
        set
        {
            if (val != value)
            {
                val = value;
                OnPropertyChanged(nameof(Val));
            }
        }
    }

    public Progress Prog
    {
        get { return prog; }
        set
        {
            if (prog != value)
            {
                prog = value;
                OnPropertyChanged(nameof(Prog));
            }
        }
    }

    public Brush Color
    {
        get { return color; }
        set
        {
            if (color != value)
            {
                color = value;
                OnPropertyChanged(nameof(Color));
            }
        }
    }

    public event PropertyChangedEventHandler PropertyChanged;
    protected void OnPropertyChanged(string propertyName)
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }

    /* SETTER */
    public void setColor(Brush color)
    {
        this.color = color;
    }

    public void setPath()
    {
        this.val = (Value)1;
    }
    public void setTreasure()
    {
        this.val = (Value)2;
    }
    public void setStart()
    {
        this.val = (Value)3;
    }

    public void setUnvisited()
    {
        this.prog = (Progress)0;
    }
    public void setVisiting()
    {
        this.prog = (Progress)1;
    }
    public void setVisited()
    {
        this.prog = (Progress)2;
    }
    /**/
    public bool isPath()
    {
        return this.val == (Value)1;
    }
    public bool isTreasure()
    {
        return this.val == (Value)2;
    }
    public bool isKrustyKrab()
    {
        return this.val == (Value)3;
    }
    public bool hasBeenVisited()
    {
        return this.prog == (Progress)2;
    }
}