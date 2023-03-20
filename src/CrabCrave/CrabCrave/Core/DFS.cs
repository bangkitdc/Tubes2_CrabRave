using System.Collections.Generic;
using System.Threading.Tasks;
using System.Windows.Media;
using CrabCrave.Core;

public class DFS
{
   public Stack<Node> path; // holds the path that is used
   public int treasureFound;
   public async Task StartDFS(Map map)
   {
       // priorities: U D L R
       Stack<Node> stack = new Stack<Node>();
       this.path = new Stack<Node>();
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
                map.map[currentX, currentY].setVisited();
                Node temp = stack.Pop();
                this.path.Push(temp);
                currentX = temp.x;
                currentY = temp.y;
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
                } else {
                    map.map[currentX, currentY].setVisited();
                    Node temp = stack.Pop();
                    this.path.Push(temp);
                    currentX = temp.x;
                    currentY = temp.y;
                    map.map[currentX, currentY].setVisiting2();
                    await Task.Delay(750);
                }
            }
        }
   }
}