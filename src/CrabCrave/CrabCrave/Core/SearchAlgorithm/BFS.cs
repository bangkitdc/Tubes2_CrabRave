using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.Threading.Tasks;
using System.Diagnostics;
using System.Windows;
using System.Collections.Concurrent;

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
        // Process Variables
        private Queue<Node>? visitQueue; // visit queue
        private Node? start; // start node
        private int treasureFound; // number of treasure found
        private int expectedTreasure; // number of treasure expected
        private Map map;
        private bool backtrackOn;

        // Result
        private ObservableCollection<Node> path;
        private int nodeVisited;
        private string route;
        private int steps;

        // Getter result
        public int NodeVisited { get => nodeVisited;}
        public string Route { get => route;}
        public int Steps { get => steps; }

        public bool isRunning;

        /* backtracking attributes */
        private Dictionary<Node, int>? depthOf; // depth of node
        private Dictionary<Node, Node>? parentOf; //backtracking
        private Dictionary<Node, List<Node>>? pathToNode; //retracking

        /// <summary>
        /// Default constructor, the backtrack is off
        /// </summary>
        /// <param name="m">Map that will be searched</param>
        public BFS(Map m)
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

        public void SetMap(Map m)
        {
            this.map = m;
        }

        /// <summary>
        /// Search the treasure in the map
        /// </summary>
        /// <returns>
        /// null if treasure found is less then expected. otherwise, will return the path
        /// </returns>
        public async Task Search(int awaitTime)
        {
            isRunning = true;
            // init
            (int xStart, int yStart) = map.getStart();
            start = map.map[xStart, yStart];
            start.setVisiting();
            await Task.Delay(awaitTime);

            if (backtrackOn) //only set if backtrack on
            {
                depthOf[start] = 0;
                pathToNode[start] = new List<Node>();
            }
            
            Node current = await next(start, 0);

            // next until found
            while (treasureFound != expectedTreasure && visitQueue.Count >= 0)
            {
                //current = next(current).Result;
                current = await next(current, awaitTime);
                if (current == null) break;
            }

            if (treasureFound != expectedTreasure)
            {
                //do nothing
            }
            else
            {
                if (backtrackOn) //only set if backtrack on
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

            Debug.WriteLine("Steps: " + steps);
            Debug.WriteLine("Node Visited: " + nodeVisited);
            Debug.WriteLine("Treasure Found: " + treasureFound);
            Debug.WriteLine("Expected Treasure: " + expectedTreasure);
            Debug.WriteLine("Route: " + route);

            isRunning = false;
        }

        /// <summary>
        /// Continue the search (will update queue, nodestatus, etc)
        /// </summary>
        /// <param name="current">Current node</param>
        /// <returns></returns>
        private async Task<Node?> next(Node current, int awaitTime)
        {
            path.Add(current);
            await progressToNode(current, awaitTime);
            nodeVisited++;

            List<Node> adjacents = adjacentNode(current); // Guaranteed that it has not been visited

            if (current.isTreasure())
            {
                treasureFound++;

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
        /// Adding path to the search path from current node to the next node in the visit queue
        /// </summary>
        /// <param name="current">Current Node</param>
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
            steps++;
            if (steps > 1)
            {
                route += direction(path[path.Count - 2], path[path.Count - 1]);
            }
            await Task.Delay(awaitTime);
            n.setVisited();
        }

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

        private string EnumerableToString(IEnumerable<Node> q)
        {
            string result = "";
            foreach (Node curr in q)
            {
                result += curr.x + "," + curr.y + " ";
            }
            return result;
        }
    }
}
