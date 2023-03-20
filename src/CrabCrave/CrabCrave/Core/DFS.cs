using System.Collections.Generic;
using System.Threading.Tasks;
using System.Windows.Media;
using CrabCrave.Core;

public class DFS
{
   public Stack<Node> path; // holds the path that is used
   public int stepsTaken;
   public int treasureFound;
   public async Task StartDFS(Map map)
   {
       // priorities: U D L R
       Stack<Node> stack = new Stack<Node>();
       this.path = new Stack<Node>();
       this.stepsTaken = 0;
       this.treasureFound = 0;

       // assume
       (int currentX, int currentY) = map.getStart();

        map.map[currentX, currentY].setVisiting();
        await Task.Delay(750);
        stack.Push(map.map[currentX, currentY]);
        this.path.Push(map.map[currentX, currentY]);

        // while 
        while (stack.Count != 0) {
            if (treasureFound == map.getTreasureCount()) {
                break;
            } else {
                if (map.isUpAvailable(currentX, currentY)) {
                    map.map[currentX, currentY].setVisited();
                    currentX--;
                    map.map[currentX, currentY].setVisiting();
                    await Task.Delay(750);
                    if (map.map[currentX, currentY].isTreasure()) {
                        treasureFound++;
                    }
                    this.path.Push(map.map[currentX, currentY]);
                    stack.Push(map.map[currentX, currentY]);
                    this.stepsTaken++;
                } else if (map.isDownAvailable(currentX, currentY)) {
                    map.map[currentX, currentY].setVisited();
                    currentX++;
                    map.map[currentX, currentY].setVisiting();
                    await Task.Delay(750);
                    if (map.map[currentX, currentY].isTreasure()) {
                        treasureFound++;
                    }
                    this.path.Push(map.map[currentX, currentY]);
                    stack.Push(map.map[currentX, currentY]);
                    this.stepsTaken++;
                } else if (map.isLeftAvailable(currentX, currentY)) {
                    map.map[currentX, currentY].setVisited();
                    currentY--;
                    map.map[currentX, currentY].setVisiting();
                    await Task.Delay(750);
                    if (map.map[currentX, currentY].isTreasure()) {
                        treasureFound++;
                    }
                    this.path.Push(map.map[currentX, currentY]);
                    stack.Push(map.map[currentX, currentY]);
                    this.stepsTaken++;
                } else if (map.isRightAvailable(currentX, currentY)) {
                    map.map[currentX, currentY].setVisited();
                    currentY++;
                    map.map[currentX, currentY].setVisiting();
                    await Task.Delay(750);
                    if (map.map[currentX, currentY].isTreasure()) {
                        treasureFound++;
                    }
                    this.path.Push(map.map[currentX, currentY]);
                    stack.Push(map.map[currentX, currentY]);
                    this.stepsTaken++;
                } else {
                    map.map[currentX, currentY].setVisited();
                    Node temp = stack.Pop();
                    if (temp.x == currentX && temp.y == currentY) {

                    } else {
                        this.path.Push(temp);
                        currentX = temp.x;
                        currentY = temp.y;
                        map.map[currentX, currentY].setVisiting();
                        this.stepsTaken++;
                    }
                }
            }
        }
   }
}