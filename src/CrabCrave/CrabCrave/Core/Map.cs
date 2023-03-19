using System.Windows.Media;
public class Map
{
    public Node[,] map;
    public int rowEff;
    public int colEff;
    public int treasureCount;

    public Map() {
        this.map = new Node[1, 1];
        this.map[0, 0] = new Node(0, 0, 0, 0, Brushes.WhiteSmoke);
        this.treasureCount = 0;
        this.rowEff = 1;
        this.colEff = 1;
    }

    public Map(int rows, int columns) {
        this.map = new Node[rows, columns];
        this.treasureCount = 0;
        for (int i = 0; i < rows; i++) {
            for (int j = 0; j < columns; j++) {
                this.map[i, j] = new Node(i, j, 0, 0, Brushes.WhiteSmoke);
                // by default the map doesn't have path and only contain walls
            }
        }
        this.rowEff = rows;
        this.colEff = columns;
    }

    /* GETTER */ 
    public (int, int) getStart() {
        // returns the index of the starting point
        for (int i = 0; i < this.rowEff; i++) {
            for (int j = 0; j < this.colEff; j++) {
                if (this.map[i, j].isKrustyKrab()) {
                    return (i, j);
                }
            }
        }

        return (-1, -1); // to handle the return error
    }

    /* SETTER */
    public void setStartInMap(int x, int y) {
        this.map[x, y].setStart();
    }

    public void setPathInMap(int x, int y) {
        this.map[x, y].setPath();
    }

    public void setTreasureInMap(int x, int y) {
        this.map[x, y].setTreasure();
        this.treasureCount++;
    }

    // Assume all the index input is correct
    public bool isUpAvailable(int x, int y) {
        if (x == 0) {
            return false;
        } else {
            if ((this.map[x-1, y].isPath() || this.map[x-1, y].isTreasure() || this.map[x-1, y].isKrustyKrab()) && !this.map[x-1, y].hasBeenVisited()) {
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
            if ((this.map[x, y+1].isPath() || this.map[x, y+1].isTreasure() || this.map[x, y+1].isKrustyKrab()) && !this.map[x, y+1].hasBeenVisited()) {
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
            if ((this.map[x, y-1].isPath() || this.map[x, y-1].isTreasure() || this.map[x, y-1].isKrustyKrab()) && !this.map[x, y-1].hasBeenVisited()) {
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
            if ((this.map[x+1, y].isPath() || this.map[x+1, y].isTreasure() || this.map[x+1, y].isKrustyKrab()) && !this.map[x+1, y].hasBeenVisited()) {
                return true;
            } else {
                return false;
            }
        }
    }

    public void printMap() { // for simple terminal view
        for (int i = 0; i < this.rowEff; i++) {
            for (int j = 0; j < this.colEff; j++) {
                if (this.map[i, j].isKrustyKrab()) {
                    Console.Write("K");
                } else if (this.map[i, j].isTreasure()) {
                    Console.Write("T");
                } else if (this.map[i, j].isPath()) {
                    Console.Write("P");
                } else if (!this.map[i, j].isPath()) {
                    Console.Write(".");
                }
            }
            Console.WriteLine("");
        }
    }
}