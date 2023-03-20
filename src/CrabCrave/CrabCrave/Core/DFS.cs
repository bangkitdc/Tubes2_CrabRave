using System.Collections.Generic;
using System.Threading.Tasks;
using System.Windows.Media;
using CrabCrave.Core;

public class DFS
{
   public Stack<Node> path; // holds the path that is used
   public int stepsTaken;
   public int treasureFound;
   public string route;
   public async Task StartDFS(Map map, bool tsp)
   {
       // priorities: U D L R
       Stack<Node> stack = new Stack<Node>();
       this.path = new Stack<Node>();
       this.stepsTaken = 0;
       this.treasureFound = 0;
       this.route = "";

       // assume
       (int currentX, int currentY) = map.getStart();

        map.map[currentX, currentY].setVisiting();
        await Task.Delay(750);
        stack.Push(map.map[currentX, currentY]);
        this.path.Push(map.map[currentX, currentY]);

        // while 
        while (stack.Count != 0) {
            if (treasureFound == map.getTreasureCount()) {
                if (!tsp) {
                    // if not tsp
                    break;
                } else {
                    // if tsp
                    if (map.adjacentToStart(currentX, currentY)) {
                        if (map.map[currentX, currentY+1].isKrustyKrab()) {
                            map.map[currentX, currentY].setVisited();
                            currentY++;
                            map.map[currentX, currentY].setVisiting();
                            await Task.Delay(750);
                            if (this.route != "") {
                                this.route += "-";
                            }
                            this.route += "R";
                        } else if (map.map[currentX-1, currentY].isKrustyKrab()) {
                            map.map[currentX, currentY].setVisited();
                            currentX--;
                            map.map[currentX, currentY].setVisiting();
                            await Task.Delay(750);
                            if (this.route != "") {
                                this.route += "-";
                            }
                            this.route += "D";
                        } else if (map.map[currentX, currentY-1].isKrustyKrab()) {
                            map.map[currentX, currentY].setVisited();
                            currentY--;
                            map.map[currentX, currentY].setVisiting();
                            await Task.Delay(750);
                            if (this.route != "") {
                                this.route += "-";
                            }
                            this.route += "L";
                        } else {
                            map.map[currentX, currentY].setVisited();
                            currentX++;
                            map.map[currentX, currentY].setVisiting();
                            await Task.Delay(750);
                            if (this.route != "") {
                                this.route += "-";
                            }
                            this.route += "U";
                        }
                        break;
                    }
                }
            }
            if (map.isRightAvailable(currentX, currentY)) {
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
                if (this.route != "") {
                    this.route += "-";
                }
                this.route += "R";
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
                if (this.route != "") {
                    this.route += "-";
                }
                this.route += "D";
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
                if (this.route != "") {
                    this.route += "-";
                }
                this.route += "L";
            } else if (map.isUpAvailable(currentX, currentY)) {
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
                if (this.route != "") {
                    this.route += "-";
                }
                this.route += "U";
            } else {
                map.map[currentX, currentY].setVisited();
                Node temp = stack.Pop();
                if (temp.x == currentX && temp.y == currentY) {

                } else {
                    this.path.Push(temp);
                    if (currentX < temp.x) {
                        if (this.route != "") {
                            this.route += "-";
                        }
                        this.route += "U";
                    } else if (currentX > temp.x) {
                        if (this.route != "") {
                            this.route += "-";
                        }
                        this.route += "D";
                    } else if (currentY < temp.y) {
                        if (this.route != "") {
                            this.route += "-";
                        }
                        this.route += "L";
                    } else {
                        if (this.route != "") {
                            this.route += "-";
                        }
                        this.route += "R";
                    }
                    currentX = temp.x;
                    currentY = temp.y;
                    map.map[currentX, currentY].setVisiting2();
                    await Task.Delay(750);
                    this.stepsTaken++;
                }
            }
            
        }
   }
}