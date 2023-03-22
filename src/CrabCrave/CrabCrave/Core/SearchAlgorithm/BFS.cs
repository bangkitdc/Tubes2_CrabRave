using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Threading.Tasks;

namespace CrabCrave.Core.SearchAlgorithm
{
//// buat di mainwindow
//private void Search(object sender, RoutedEventArgs e)
//{
//    var m = (MainViewModel)DataContext;

//    Stopwatch stopwatch = new Stopwatch();
//    stopwatch.Start();

//    BFS bfs = new BFS(m._map);
//    //bfs.SetBacktrackOn();
//    await bfs.Search();

//    stopwatch.Stop();

//    ExecutionText.Text = stopwatch.Elapsed.TotalSeconds.ToString() + " seconds";
//}

    public class BFS
    {
        /* Variables needed in the process */
        private Queue<Node>? visitQueue; // visit queue => store next node to be visited in queue
        private Node? start; // start node => store starting node
        private int treasureFound; // number of treasure found (current)
        private int expectedTreasure; // number of treasure expected
        private Map map; // map to be searched
        private bool backtrackOn; // option to use backtrack. if set off, travelling between node can be done directly (without backtrack)

        // Result
        private ObservableCollection<Node> path; // => store path of node visited through the search (can include backtrack)
        private List<Node> treasureNodes; // => store nodes that are treasure
        private int nodeVisited; // => number of node visited
        private string route; // => solution route (in string) after the search
        private int steps; // => steps taken in route

        // Getter result
        public int NodeVisited { get => nodeVisited;}
        public string Route { get => route;}
        public int Steps { get => steps; }

        public bool isRunning;

        /* backtracking attributes */
        private Dictionary<Node, int>? depthOf; // depth of node
        private Dictionary<Node, Node>? parentOf; // => track the parent of each node
        private Dictionary<Node, List<Node>>? pathToNode; // => store path from start to each node

        /// <summary>
        /// Default constructor, the backtrack is off
        /// </summary>
        /// <param name="m">Map that will be searched</param>
        public BFS(Map m) //ctor
        {
            map = m;
            backtrackOn = true;
            isRunning = false;
            resetBFS();
        }

        /// <summary>
        /// Set backtrack option to false, BFS search will not perform backtrack
        /// </summary>
        public void SetBacktrackOff()
        {
            backtrackOn = false;
            resetBFS();
        }

        /// <summary>
        /// Set backtrack option to true, BFS search will perform backtrack
        /// </summary>
        public void SetBacktrackOn()
        {
            backtrackOn = true;
            resetBFS();
        }

        /// <summary>
        /// Set map to be searched.
        /// </summary>
        public void SetMap(Map m)
        {
            this.map = m;
        }

        /// <summary>
        /// Search the treasure in the map, the result won't be returned immediately
        /// </summary>
        /// <param name="awaitTime">Time to wait before changing node</param>
        /// <param name="tspOn">Option to use TSP. Will include go back to starting point</param>
        /// <remarks>
        /// To access the result, you can use getter NodeVisited, Route, and Steps.
        /// Alternatively, you can track the map since the algorithm alter it directly.
        /// </remarks>
        public async Task Search(int awaitTime, bool tspOn)
        {
            isRunning = true;
            /* initialize starting point */
            (int xStart, int yStart) = map.getStart();
            start = map.map[xStart, yStart];
            start.setVisiting();
            await Task.Delay(awaitTime);

            if (backtrackOn) //only initialize if backtrack on
            {
                depthOf[start] = 0;
                pathToNode[start] = new List<Node>();
            }
            /* end of initialization */

            /* Main loop */
            Node current = await next(start, 0);

            // next until found
            while (treasureFound != expectedTreasure && visitQueue.Count >= 0)
            {
                //current = next(current).Result;
                current = await next(current, awaitTime);
                if (current == null) break;
            }
            /* End of main loop */

            if (treasureFound == expectedTreasure) // search done, go back to the start
            {
                if (backtrackOn && tspOn) // only do if backtrackOn and tspOn
                {
                    Node prev = parentOf[current];
                    while (prev != start)
                    {
                        path.Add(prev);
                        await progressToNode(prev, awaitTime);
                        prev = parentOf[prev];
                    }
                    path.Add(prev);
                    await progressToNode(prev, awaitTime);
                }
            }

            /* This is for finding the solution route */
            /* Converting each path to treasures (List<Node>) to string */
            current = start; steps++;
            foreach (Node treasure in treasureNodes)
            {
                // backtrack until it is possible to go to the destined node
                while (current != start && !pathToNode[treasure].Contains(current))
                {
                    route += direction(current, parentOf[current]) + '-';
                    steps++;
                    current = parentOf[current];
                }

                // go forward until destined node
                for (int i = depthOf[current]; i < depthOf[treasure]; i++)
                {
                    Node n = pathToNode[treasure][i]; // node that should be visited first before going to the next node in the queue
                    route += direction(current, n);
                    if (!tspOn && !(treasure == treasureNodes[treasureNodes.Count-1] && i == depthOf[treasure]-1))
                    {
                        route += '-';
                    }
                    steps++;
                    current = n;
                }
            }

            if (tspOn)
            {
                while (current != start)
                {
                    route += direction(current, parentOf[current]);
                    if (parentOf[current] != start)
                    {
                        route += '-';
                    }
                    steps++;
                    current = parentOf[current];
                }
            }
            /* end of finding the solution route */

            isRunning = false;
        }

        /// <summary>
        /// Continue the search (will update queue, nodestatus, etc)
        /// </summary>
        /// <param name="current">Current node</param>
        /// <param name="awaitTime">Time to wait before changing node</param>
        /// <returns>Next node</returns>
        private async Task<Node?> next(Node current, int awaitTime)
        {
            path.Add(current);
            await progressToNode(current, awaitTime);
            nodeVisited++;

            List<Node> adjacents = adjacentNode(current); // Guaranteed that it has not been visited

            if (current.isTreasure())
            {
                treasureFound++;
                treasureNodes.Add(current);

                // if all treasure found, get out immediately
                if (treasureFound == expectedTreasure) return current;
            }

            foreach (Node node in adjacents)
            {
                visitQueue.Enqueue(node);

                if (backtrackOn)
                {
                    depthOf[node] = depthOf[current] + 1;
                    parentOf[node] = current;
                    pathToNode[node] = new List<Node>(pathToNode[current]);
                    pathToNode[node].Add(node);
                }
            }

            if (backtrackOn && visitQueue.Count > 0)
            {
                await addPathToNextNode(current, awaitTime);
            }
            current.setVisited();

            return visitQueue.Count > 0 ? visitQueue.Dequeue() : null;
        }

        /// <summary>
        /// Get all adjacent node from node c that is available (check node definition of available)
        /// </summary>
        /// <param name="c">The node</param>
        /// <returns>All node adjacent to node c that is available</returns>
        private List<Node> adjacentNode(Node c)
        {
            List<Node> nodes = new List<Node>();

            /* The order is : Right - Down - Left - Up */
            if (map.isRightAvailable(c.x, c.y))
            {
                nodes.Add(map.map[c.x, c.y + 1]);
            }
            if (map.isDownAvailable(c.x, c.y))
            {
                nodes.Add(map.map[c.x + 1, c.y]);
            }
            if (map.isLeftAvailable(c.x, c.y))
            {
                nodes.Add(map.map[c.x, c.y - 1]);
            }
            if (map.isUpAvailable(c.x, c.y))
            {
                nodes.Add(map.map[c.x - 1, c.y]);
            }

            return nodes;
        }

        /// <summary>
        /// Adding path from current node to the next node in the visit queue to the search path.
        /// Basically backtrack to the next node.
        /// </summary>
        /// <param name="current">Current Node</param>
        /// <param name="awaitTime">Time to wait before changing node</param>
        private async Task addPathToNextNode(Node current, int awaitTime)
        {
            // if next node in queue is the same level as current or deeper -> require backtracking and retracking
            if (visitQueue.Count > 0 && current != start && (depthOf[current] <= depthOf[visitQueue.Peek()]))
            {
                if (parentOf[visitQueue.Peek()] == current)
                {
                    // no need to find path
                    return;
                }

                // backtracking
                Node prev = parentOf[current];

                while (!pathToNode[visitQueue.Peek()].Contains(prev) && prev != start)
                {
                    path.Add(prev);
                    await progressToNode(prev, awaitTime);

                    prev = parentOf[prev];
                }
                path.Add(prev);
                await progressToNode(prev, awaitTime);

                // go forward until just before the destined queue
                for (int i = depthOf[prev]; i < depthOf[visitQueue.Peek()] - 1; i++)
                {
                    Node n = pathToNode[visitQueue.Peek()][i]; // node that should be visited first before going to the next node in the queue
                    path.Add(n);
                    await progressToNode(n, awaitTime);
                }
            }
        }

        /// <summary>
        /// Set all the BFS variables to default starting point
        /// </summary>
        private void resetBFS()
        {
            expectedTreasure = map.getTreasureCount();
            visitQueue = new Queue<Node>();
            path = new ObservableCollection<Node>();
            treasureNodes = new List<Node>();
            treasureFound = 0;
            steps = -1;
            nodeVisited = 0;
            route = "";

            if (backtrackOn)
            {
                depthOf = new Dictionary<Node, int>();
                parentOf = new Dictionary<Node, Node>();
                pathToNode = new Dictionary<Node, List<Node>>();
            }
        }

        private async Task progressToNode(Node n, int awaitTime)
        {
            n.setVisiting();

            // DEPRECATED (for now); use case => kalo route diperluin full search
            // kalo mau pake, jangan lupa comment route yang di atas (di fungsi search)
            //steps++;
            //if (steps > 0)
            //{
            //    if (steps != 1)
            //    {
            //        route += '-';
            //    }
            //    route += direction(path[path.Count - 2], path[path.Count - 1]);
            //}
            await Task.Delay(awaitTime);
            n.setVisited();
        }

        /// <summary>
        /// Get direction from source node to destination node
        /// </summary>
        /// <param name="source"></param>
        /// <param name="dest"></param>
        /// <returns>(R, D, L, U) based on the direction</returns>
        private string direction(Node source, Node dest)
        {
            if (source.x == dest.x)
            {
                if (source.y < dest.y)
                {
                    return "R";
                }
                else
                {
                    return "L";
                }
            }
            else
            {
                if (source.x < dest.x)
                {
                    return "D";
                }
                else
                {
                    return "U";
                }
            }
        }
    }
}
