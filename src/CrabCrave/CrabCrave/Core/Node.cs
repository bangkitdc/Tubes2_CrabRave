using System.Windows.Media;
public class Node
{   
    public enum Value {
        Wall, // 0
        Path, // 1
        Treasure, // 2
        KrustyKrab // 3
    }
    public enum Progress {
        Unvisited, // 0
        InVisit, // 1
        Visited // 2
    }
    public int x;
    public int y;
    public Value val;
    public Progress prog;
    public Brush color;

    public Node(int x, int y, int value, int progress, Brush color) {
        this.x = x;
        this.y = y;
        this.val = (Value) value;
        this.prog = (Progress) progress;
        this.color = color;
    }

    public Node(Node other)
    {
        this.x = other.x;
        this.y = other.y;
        this.val = other.val;
        this.prog = other.prog;
        this.color = other.color;
    }

    /* SETTER */
    public void setPath() {
        this.val = (Value) 1;
    }
    public void setTreasure() {
        this.val = (Value) 2;
    }
    public void setStart() {
        this.val = (Value) 3;
    }

    public void setUnvisited() {
        this.prog = (Progress) 0;
    }
    public void setVisiting() {
        this.prog = (Progress) 1;
    }
    public void setVisited() {
        this.prog = (Progress) 2;
    }
    /**/
    public bool isPath() {
        return this.val == (Value) 1;
    }
    public bool isTreasure() {
        return this.val == (Value) 2;
    }
    public bool isKrustyKrab() {
        return this.val == (Value) 3;
    }
    public bool hasBeenVisited() {
        return this.prog == (Progress) 2;
    }
}
