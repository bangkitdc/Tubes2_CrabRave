using System.Windows.Media;
public class Node
{   
    public int x;
    public int y;
    public bool startpoint; // if the node is krusty krab then true
    public bool path; // if the node is a path then true
    public bool treasure; // true if the node contains treasure
    public bool unvisited; // true if the node is unvisited, false if the node have been visited
    public bool visiting; // true if the node is currently being visited
    public Brush color;

    public Node(int x, int y, bool path, bool treasure, bool unvisited, bool visiting, Brush color) {
        this.x = x;
        this.y = y;
        this.path = path;
        this.treasure = treasure;
        this.unvisited = unvisited;
        this.visiting = visiting;
        this.color = color;
    }

    /* SETTER */
    public void setStart() {
        this.startpoint = true;
    }

    public void setPath() {
        this.path = true;
    }

    public void setTreasure() {
        this.path = true;
        this.treasure = true;
    }

    public void setVisited() {
        this.unvisited = true;
    }

    public void setVisiting() {
        this.visiting = true;
    }
    public void setUnvisited() {
        this.unvisited = false;
    }

    /**/
    public bool isKrustyKrab() {
        return this.startpoint;
    }

    public bool isPath() {
        return this.path;
    }

    public bool isTreasure() {
        return this.treasure;
    }

    public bool hasBeenVisited() {
        return this.unvisited;
    }
}
