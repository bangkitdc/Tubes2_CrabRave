using System.Collections.Generic;

namespace CrabCrave.Core.SearchAlgorithm
{
    public class BFS : SearchAlgorithm<Node>
    {
        private Queue<Node>? visitQueue; // visit queue
        private Node? start; // start node
        private int treasureFound; // number of treasure found
        private int expectedTreasure; // number of treasure expected

        private Map originalMap; // maintain original map for reset
        private bool backtrackOn;

        /* backtracking attributes */
        private Dictionary<Node, int>? depthOf; // depth of node
        private Dictionary<Node, Node>? parentOf; //backtracking
        private Dictionary<Node, Queue<Node>>? pathToNode; //retracking

        /// <summary>
        /// Default constructor, the backtrack is off
        /// </summary>
        /// <param name="m">Map that will be searched</param>
        public BFS(Map m) : base(m)
        {
            originalMap = new Map(m);
            backtrackOn = false;
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
        public override List<Node> Search()
        {
            // init
            (int xStart, int yStart) = map.getStart();
            start = map.map[xStart, yStart];
            start.setVisited();

            if (backtrackOn) //only set if backtrack on
            {
                depthOf[start] = 0;
                pathToNode[start] = new Queue<Node>();
            }

            Node current = next(start);

            // next until found
            while (treasureFound != expectedTreasure && visitQueue.Count >= 0)
            {
                current = next(current);
            }

            if (treasureFound != expectedTreasure)
            {
                return null;
            }
            else
            {
                if (backtrackOn) //only set if backtrack on
                {
                    Node prev = parentOf[current];
                    while (prev != start)
                    {
                        path.Add(prev);
                        prev = parentOf[prev];
                    }
                    path.Add(prev);
                }
            }

            return path;
        }

        /// <summary>
        /// Continue the search (will update queue, nodestatus, etc)
        /// </summary>
        /// <param name="current">Current node</param>
        /// <returns></returns>
        private Node? next(Node current)
        {
            path.Add(current);
            List<Node> adjacents = adjacentNode(current); // Guaranteed that it has not been visited

            if (current.isTreasure())
            {
                treasureFound++;

                // if all treasure found, get out immediately
                if (treasureFound == expectedTreasure) return current;
            }

            foreach (Node node in adjacents)
            {
                node.setVisited();
                visitQueue.Enqueue(node);

                if (backtrackOn)
                {
                    depthOf[node] = depthOf[current] + 1;
                    parentOf[node] = current;
                    pathToNode[node] = new Queue<Node>(pathToNode[current]);
                    pathToNode[node].Enqueue(node);
                }
            }

            if (backtrackOn)
            {
                addPathToNextNode(current);
            }

            return visitQueue.Count > 0 ? visitQueue.Dequeue() : current;
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
        private void addPathToNextNode(Node current)
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
                    prev = parentOf[prev];
                }
                path.Add(prev);

                // go forward until just before the destined queue
                Queue<Node> pathToDest = new Queue<Node>(pathToNode[visitQueue.Peek()]);
                if (prev != start)
                {
                    for (int i = depthOf[prev]; i >= 1; i--)
                    {
                        pathToDest.Dequeue();
                    }
                }

                while (pathToDest.Count > 1)
                {
                    Node a = pathToDest.Dequeue();
                    path.Add(a);
                }
            }
        }

        /// <summary>
        /// Set all the BFS variables to default starting point
        /// </summary>
        private void resetBFS()
        {
            map = new Map(originalMap);
            expectedTreasure = map.getTreasureCount();
            visitQueue = new Queue<Node>();
            path = new List<Node>();
            treasureFound = 0;

            if (backtrackOn)
            {
                depthOf = new Dictionary<Node, int>();
                parentOf = new Dictionary<Node, Node>();
                pathToNode = new Dictionary<Node, Queue<Node>>();
            }
        }

        private string QueueToString(Queue<Node> q)
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
