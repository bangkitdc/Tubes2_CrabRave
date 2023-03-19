//using System.Collections.Generic;

public class DFS 
{
    public Stack<Node> path; // holds the path that is used
    public int treasureFound;
    public DFS(Map map) {
        // priorities: U D L R
        Stack<Node> stack = new Stack<Node>();
        this.path = new Stack<Node>();
        this.treasureFound = 0;
 
        // assume
        (int currentX, int currentY) = map.getStart();

        map.map[currentX, currentY].setVisiting();
        map.map[currentX, currentY].setVisited();
        stack.Push(map.map[currentX, currentY]);
        this.path.Push(map.map[currentX, currentY]);

        // while 
        while (stack.Count != 0) {
            if (treasureFound == map.treasureCount) {
                map.map[currentX, currentY].setLeaving();
                Node temp = stack.Pop();
                this.path.Push(temp);
                currentX = temp.x;
                currentY = temp.y;
            } else {
                if (map.isUpAvailable(currentX, currentY)) {
                    map.map[currentX, currentY].setLeaving();
                    currentX--;
                    map.map[currentX, currentY].setVisiting();
                    map.map[currentX, currentY].setVisited();
                    if (map.map[currentX, currentY].isTreasure()) {
                        treasureFound++;
                    }
                    this.path.Push(map.map[currentX, currentY]);
                    stack.Push(map.map[currentX, currentY]);
                } else if (map.isDownAvailable(currentX, currentY)) {
                    map.map[currentX, currentY].setLeaving();
                    currentX++;
                    map.map[currentX, currentY].setVisiting();
                    map.map[currentX, currentY].setVisited();
                    if (map.map[currentX, currentY].isTreasure()) {
                        treasureFound++;
                    }
                    this.path.Push(map.map[currentX, currentY]);
                    stack.Push(map.map[currentX, currentY]);
                } else if (map.isLeftAvailable(currentX, currentY)) {
                    map.map[currentX, currentY].setLeaving();
                    currentY--;
                    map.map[currentX, currentY].setVisiting();
                    map.map[currentX, currentY].setVisited();
                    if (map.map[currentX, currentY].isTreasure()) {
                        treasureFound++;
                    }
                    this.path.Push(map.map[currentX, currentY]);
                    stack.Push(map.map[currentX, currentY]);
                } else if (map.isRightAvailable(currentX, currentY)) {
                    map.map[currentX, currentY].setLeaving();
                    currentY++;
                    map.map[currentX, currentY].setVisiting();
                    map.map[currentX, currentY].setVisited();
                    if (map.map[currentX, currentY].isTreasure()) {
                        treasureFound++;
                    }
                    this.path.Push(map.map[currentX, currentY]);
                    stack.Push(map.map[currentX, currentY]);
                } else {
                    map.map[currentX, currentY].setLeaving();
                    Node temp = stack.Pop();
                    this.path.Push(temp);
                    currentX = temp.x;
                    currentY = temp.y;
                }
            }
        }
    }
}