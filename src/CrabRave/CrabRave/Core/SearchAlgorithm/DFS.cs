using System.Collections.Generic;
using System.Threading.Tasks;
using System.Windows.Media;
using CrabRave.Core;
using System.Linq;

public class DFS
{
    public Stack<Node> path; // holds the path that is used
    public int stepsTaken;
    public int treasureFound;
    public string route;
    public int nodesVisited;
    public bool isRunning;

    public bool isItSection(Map map, int x, int y) {
        int ctr = 0;

        if (map.isRightAvailable(x, y)) {
            ctr++;
        }
        if (map.isDownAvailable(x, y)) {
            ctr++;
        }
        if (map.isLeftAvailable(x, y)) {
            ctr++;
        }
        if (map.isUpAvailable(x, y)) {
            ctr++;
        }
        if (ctr > 1) {
            return true;
        }
        return false;
    }
    
    public bool isItInTheList(List<Node> list, int x, int y) {
        for (int i = 0; i < list.Count; i++) {
            if (list.ElementAt(i).x == x && list.ElementAt(i).y == y) {
                return true;
            }
        }

        return false;
    }

    public async Task StartDFS(Map map, bool tsp, int awaitTime)
    {
        // priorities: R D L U
        Stack<Node> stack = new Stack<Node>();
        List<Node> treasureNode = new List<Node>(); // contains the node which is in a path to get a treasure
        this.path = new Stack<Node>();
        this.stepsTaken = 0;
        this.treasureFound = 0;
        this.nodesVisited = 0;
        this.route = "";
        bool thereIsTreasure = false;
        isRunning = true;

        // get the start point
        (int currentX, int currentY) = map.getStart();

        map.map[currentX, currentY].setVisiting();
        nodesVisited++;
        await Task.Delay(awaitTime);

        stack.Push(map.map[currentX, currentY]);
        this.path.Push(map.map[currentX, currentY]);

        // while stack is not empty 
        while (stack.Count != 0)
        {
            if (treasureFound == map.getTreasureCount())
            // if all treasure has been found
            {
                if (!tsp)
                {
                    // if not tsp
                    break;
                }
                else
                {
                    // if tsp, then find the route back to start
                    if (map.adjacentToStart(currentX, currentY))
                    {
                        nodesVisited--; // adjust saat visit krustykrab di awal
                        if (map.rightIsStart(currentX, currentY)) {
                            // if start is at the right side of current node
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
                            // if start is at the down side of current node
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
                            // if start is at the left side of current node
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
                            // if start is at the up side of current node
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
                // if the right side of current node is available
                if (thereIsTreasure) {
                    treasureNode.Add(map.map[currentX, currentY]);
                }
                thereIsTreasure = false;
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
                // if the down side of current node is available
                if (thereIsTreasure) {
                    treasureNode.Add(map.map[currentX, currentY]);
                }
                thereIsTreasure = false;
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
            else if (map.isLeftAvailable(currentX, currentY)) {
                // if the left side of current node is available
                if (thereIsTreasure) {
                    treasureNode.Add(map.map[currentX, currentY]);
                }
                thereIsTreasure = false;
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
                // if the up side of current node is available
                if (thereIsTreasure) {
                    treasureNode.Add(map.map[currentX, currentY]);
                }
                thereIsTreasure = false;
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
                // if there are no available nodes to be visited from the current node, then backtrack
                map.map[currentX, currentY].setVisited();
                Node temp = stack.Pop();
                
                // if while backtracking there is treasure
                if (temp.isTreasure()  && !thereIsTreasure) {
                    thereIsTreasure = true;
                    currentX = temp.x;
                    currentY = temp.y;
                    map.map[currentX, currentY].setVisiting();
                }

                if (temp.x == currentX && temp.y == currentY)
                {
                    if (!thereIsTreasure) {
                        // if while backtracking there are no treasures, then delete it from the path
                        this.path.Pop();
                        stepsTaken--;
                        this.route = this.route.Remove(this.route.Length - 1);
                        if (this.route.Length >= 2) {
                            this.route = this.route.Remove(this.route.Length - 1);
                        }
                    }
                }
                else
                {
                    if (thereIsTreasure) {
                        // if while backtracking there are treasures, then add it to the path
                        this.path.Push(temp);
                        this.stepsTaken++;
                    }

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

                    if (!thereIsTreasure) {
                        this.route = this.route.Remove(this.route.Length - 1);
                        if (this.route.Length >= 2) {
                            this.route = this.route.Remove(this.route.Length - 1);
                        }
                    }

                    currentX = temp.x;
                    currentY = temp.y;
                    stack.Push(map.map[currentX, currentY]);
                    map.map[currentX, currentY].setVisiting();
                    await Task.Delay(awaitTime);

                    // check if the current node is actually a needed node in the path
                    if (isItInTheList(treasureNode, currentX, currentY)) {
                        thereIsTreasure = true;
                    }
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
        // highlight solution
        while (this.path.Count != 0) {
            Node temp = this.path.Pop();
            map.map[temp.x, temp.y].highlightSolution();
            await Task.Delay(awaitTime);
        }
        isRunning = false;
    }
}