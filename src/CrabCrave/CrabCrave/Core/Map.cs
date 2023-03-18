using System.Windows.Media;
public class Map
{
    public Node[,] map;
    public int rowEff;
    public int colEff;

    public Map() {
        map = new Node[1, 1];
        this.rowEff = 1;
        this.colEff = 1;
    }

    public Map(int rows, int columns) {
        this.map = new Node[rows, columns];
        this.rowEff = rows;
        this.colEff = columns;
        for (int i = 0; i < rows; i++) {
            for (int j = 0; j < columns; i++) {
                this.map[i, j] = new Node(i, j, false, false, false, true, false, Brushes.WhiteSmoke);
                // by default the map doesn't have path and only contain walls
            }
        }
    }

    /* GETTER */ 
    public (int, int) getStart() {
        // returns the index of the starting point
        for (int i = 0; i < this.rowEff; i++) {
            for (int j = 0; j < this.colEff; i++) {
                if (this.map[i, j].isKrustyKrab()) {
                    return (i, j);
                }
            }
        }

        return (-1, -1); // to handle the return error
    }

    /* SETTER */
    public void setStartInMap(int x, int y) {
        this.map[x, y].setPath();
    }

    public void setPathInMap(int x, int y) {
        this.map[x, y].setPath();
    }

    public void setTreasureInMap(int x, int y) {
        this.map[x, y].setTreasure();
    }

    // Assume all the index input is correct
    public bool isUpAvailable(int x, int y) {
        if (x == 0) {
            return false;
        } else {
            if (this.map[x-1, y].isPath() && !this.map[x-1, y].hasBeenVisited()) {
                return true;
            } else {
                return false;
            }
        }

    }

    public bool isRightAvailable(int x, int y) {
        if (y == this.colEff-1) {
            return false;
        } else {
            if (this.map[x, y+1].isPath() && !this.map[x, y+1].hasBeenVisited()) {
                return true;
            } else {
                return false;
            }
        }
    }

    public bool isLeftAvailable(int x, int y) {
        if (y == 0) {
            return false;
        } else {
            if (this.map[x, y-1].isPath() && !this.map[x, y-1].hasBeenVisited()) {
                return true;
            } else {
                return false;
            }
        }
    }

    public bool isDownAvailable(int x, int y) {
        if (x == this.rowEff-1) {
            return false;
        } else {
            if (this.map[x+1, y].isPath() && !this.map[x+1, y].hasBeenVisited()) {
                return true;
            } else {
                return false;
            }
        }
    }
}