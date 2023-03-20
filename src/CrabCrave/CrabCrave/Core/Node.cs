using System.ComponentModel;
using System.Threading.Tasks;
using System.Windows;
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
    public int numOfVisits;
    public Value val;
    public Progress prog;
    public SolidColorBrush color;

    public Node(int x, int y, int value, int progress, SolidColorBrush color)
    {
        this.x = x;
        this.y = y;
        this.numOfVisits = 0;
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
    public Node(Node other)
    {
        this.x = other.x;
        this.y = other.y;
        this.numOfVisits = other.numOfVisits;
        this.val = other.val;
        this.prog = other.prog;
        this.color = other.color;
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

    public SolidColorBrush Colors
    {
        get { return color; }
        set
        {
            if (color != value)
            {
                color = value;
                OnPropertyChanged(nameof(Colors));
            }
        }
    }

    public event PropertyChangedEventHandler PropertyChanged;
    protected void OnPropertyChanged(string propertyName)
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }

    /* SETTER */

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
        if (Val == Value.Treasure)
        {
            Colors = Brushes.DarkGoldenrod;
        }
        else
        {
            Colors = Brushes.LightGreen;
        }
        this.numOfVisits++;
    }

    public void setVisiting2()
    {
        this.prog = (Progress)1;
        byte darkAlpha = 128; // 50% opacity
        Color darkColor = Color.FromArgb(darkAlpha, Colors.Color.R, Colors.Color.G, Colors.Color.B);
        SolidColorBrush darkBrush = new SolidColorBrush(darkColor);

        Colors = darkBrush;
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