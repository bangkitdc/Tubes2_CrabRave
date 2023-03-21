using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Media;
using CrabCrave.Core;

public class DFS
{
   public Stack<Node> path; // holds the path that is used
   public int stepsTaken;
   public int treasureFound;
   public string route;
   public int nodesVisited;
   public bool isRunning;

    public async Task StartDFS(Map map, bool tsp, int awaitTime)
    {
        // priorities: U D L R
        Stack<Node> stack = new Stack<Node>();
        this.path = new Stack<Node>();
        this.stepsTaken = 0;
        this.treasureFound = 0;
        this.nodesVisited = 0;
        this.route = "";
        isRunning = true;

        // assume
        (int currentX, int currentY) = map.getStart();

        map.map[currentX, currentY].setVisiting();
        nodesVisited++;
        await Task.Delay(awaitTime);

        stack.Push(map.map[currentX, currentY]);
        this.path.Push(map.map[currentX, currentY]);

        // while 
        while (stack.Count != 0)
        {
            if (treasureFound == map.getTreasureCount())
            {
                if (!tsp)
                {
                    // if not tsp
                    break;
                }
                else
                {
                    // if tsp
                    if (map.adjacentToStart(currentX, currentY))
                    {
                        nodesVisited--; // adjust saat visit krustykrab di awal
                        if (map.rightIsStart(currentX, currentY)) {
                            map.map[currentX, currentY].setVisited();
                            currentY++;
                            map.map[currentX, currentY].setVisiting();
                            await Task.Delay(awaitTime);
                            this.stepsTaken++;
                            this.path.Push(map.map[currentX, currentY]);
                            if (this.route != "") {
                                this.route += "-";
                            }

                            if (!map.map[currentX, currentY].hasBeenVisited())
                            {
                                nodesVisited++;
                            }
                            this.route += "R";
                        } else if (map.downIsStart(currentX, currentY)) {
                            map.map[currentX, currentY].setVisited();
                            currentX++;
                            map.map[currentX, currentY].setVisiting();
                            await Task.Delay(awaitTime);
                            this.stepsTaken++;
                            this.path.Push(map.map[currentX, currentY]);
                            if (this.route != "") {
                                this.route += "-";
                            }

                            if(!map.map[currentX, currentY].hasBeenVisited())
                            {
                                nodesVisited++;
                            }

                            this.route += "D";
                        } else if (map.leftIsStart(currentX, currentY)) {
                            map.map[currentX, currentY].setVisited();
                            currentY--;
                            map.map[currentX, currentY].setVisiting();
                            await Task.Delay(awaitTime);
                            this.stepsTaken++;
                            this.path.Push(map.map[currentX, currentY]);
                            if (this.route != "") {
                                this.route += "-";
                            }

                            if (!map.map[currentX, currentY].hasBeenVisited())
                            {
                                nodesVisited++;
                            }

                            this.route += "L";
                        }
                        else
                        {
                            map.map[currentX, currentY].setVisited();
                            currentX--;
                            map.map[currentX, currentY].setVisiting();
                            await Task.Delay(awaitTime);
                            this.stepsTaken++;
                            this.path.Push(map.map[currentX, currentY]);
                            if (this.route != "") {
                                this.route += "-";
                            }

                            if (!map.map[currentX, currentY].hasBeenVisited())
                            {
                                nodesVisited++;
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
                await Task.Delay(awaitTime);

                if (map.map[currentX, currentY].isTreasure())
                {
                    treasureFound++;
                }
                this.path.Push(map.map[currentX, currentY]);
                stack.Push(map.map[currentX, currentY]);
                this.stepsTaken++;
                if (this.route != "")
                {
                    this.route += "-";
                }

                if (!map.map[currentX, currentY].hasBeenVisited())
                {
                    nodesVisited++;
                }

                this.route += "R";
            } else if (map.isDownAvailable(currentX, currentY)) {
                map.map[currentX, currentY].setVisited();
                currentX++;
                map.map[currentX, currentY].setVisiting();
                await Task.Delay(awaitTime);

                if (map.map[currentX, currentY].isTreasure())
                {
                    treasureFound++;
                }
                this.path.Push(map.map[currentX, currentY]);
                stack.Push(map.map[currentX, currentY]);
                this.stepsTaken++;
                if (this.route != "")
                {
                    this.route += "-";
                }

                if (!map.map[currentX, currentY].hasBeenVisited())
                {
                    nodesVisited++;
                }

                this.route += "D";
            }
            else if (map.isLeftAvailable(currentX, currentY))
            {
                map.map[currentX, currentY].setVisited();
                currentY--;
                map.map[currentX, currentY].setVisiting();

                await Task.Delay(awaitTime);

                if (map.map[currentX, currentY].isTreasure())
                {
                    treasureFound++;
                }
                this.path.Push(map.map[currentX, currentY]);
                stack.Push(map.map[currentX, currentY]);
                this.stepsTaken++;
                if (this.route != "")
                {
                    this.route += "-";
                }

                if (!map.map[currentX, currentY].hasBeenVisited())
                {
                    nodesVisited++;
                }

                this.route += "L";
            } else if (map.isUpAvailable(currentX, currentY)) {
                map.map[currentX, currentY].setVisited();
                currentX--;
                map.map[currentX, currentY].setVisiting();

                await Task.Delay(awaitTime);

                if (map.map[currentX, currentY].isTreasure())
                {
                    treasureFound++;
                }
                this.path.Push(map.map[currentX, currentY]);
                stack.Push(map.map[currentX, currentY]);
                this.stepsTaken++;
                if (this.route != "")
                {
                    this.route += "-";
                }

                if (!map.map[currentX, currentY].hasBeenVisited())
                {
                    nodesVisited++;
                }

                this.route += "U";
            } else {
                map.map[currentX, currentY].setVisited();
                Node temp = stack.Pop();
                if (temp.x == currentX && temp.y == currentY)
                {

                }
                else
                {
                    this.path.Push(temp);
                    if (currentX < temp.x)
                    {
                        if (this.route != "")
                        {
                            this.route += "-";
                        }
                        this.route += "D";
                    } else if (currentX > temp.x) {
                        if (this.route != "") {
                            this.route += "-";
                        }
                        this.route += "U";
                    } else if (currentY < temp.y) {
                        if (this.route != "") {
                            this.route += "-";
                        }
                        this.route += "R";
                    } else {
                        if (this.route != "") {
                            this.route += "-";
                        }
                        this.route += "L";
                    }

                    currentX = temp.x;
                    currentY = temp.y;
                    stack.Push(map.map[currentX, currentY]);
                    map.map[currentX, currentY].setVisiting();
                    await Task.Delay(awaitTime);
                    this.stepsTaken++;
                }

                if (stack.Count == 0) {
                    if (map.isRightAvailable(currentX, currentY)) {
                        stack.Push(map.map[currentX, currentY]);
                    } else if (map.isDownAvailable(currentX, currentY)) {
                        stack.Push(map.map[currentX, currentY]);
                    } else if (map.isLeftAvailable(currentX, currentY)) {
                        stack.Push(map.map[currentX, currentY]);
                    } else if (map.isUpAvailable(currentX, currentY)) {
                        stack.Push(map.map[currentX, currentY]);
                    }
                }
            }
        }
        isRunning = false;
    }
}