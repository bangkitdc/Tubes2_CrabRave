class Node
{   
    private int x;
    private int y;
    private bool path; // if the node is a path then true
    private bool treasure; // true if the node contains treasure
    private bool unvisited; // true if the node is unvisited, false if the node have been visited
    private bool visiting; // true if the node is currently being visited

    public Node(int x, int y, bool path, bool treasure, bool unvisited, bool visiting) {
        this.x = x;
        this.y = y;
        this.path = path;
        this.treasure = treasure;
        this.unvisited = unvisited;
        this.visiting = visiting;
    }

    /* SETTER */
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
