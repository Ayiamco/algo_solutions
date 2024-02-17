namespace AlgoApp.Graph;

public static class GraphAlgo 
{
    static readonly Dictionary<string, List<string>?> ExampleDirectedGraph = new()
    {
        {"a", new List<string> { "b", "c", "g"}},
        {"b", new List<string> { "d", "e"}},
        {"c", new List<string> { "f"}},
        {"d", null},
        {"e", null},
        {"f", null},
        {"g", new List<string> {"i", "j"}},
        {"i", null},
        {"j", null},
        {"z", null},
    };

    static readonly Dictionary<string, List<string>?> ExampleUnDirectedGraph = new()
    {
        {"d", new List<string> {"b"}},
        {"e", new List<string> {"b"}},
        {"a", new List<string> { "b", "c"}},
        {"b", new List<string> { "a","d", "e"}},
        {"c", new List<string> { "a","f"}}, 
        {"f", new List<string> {"c"}},
        {"g", new List<string> {"i", "j"}},
        {"i", new List<string> {"g"}},
        {"j", new List<string> {"g"}},
        {"z", null},
    };
    static readonly List<List<string>> ExampleArrayOfEdges = new List<List<string>> 
    {
        new List<string> {"i", "j"},
        new List<string> {"k", "i"},
        new List<string> {"m", "k"},
        new List<string> {"k", "l"},
        new List<string> {"o", "n"},
    };

    public static void BreadthFirstTransversal(Dictionary<string, List<string>?>  graph, string start)
    {
        var queue = new Queue<string>();
        queue.Enqueue(start);
        while(queue.Any())
        {
            var currentNode = queue.Dequeue();
             Console.WriteLine($"{nameof(BreadthFirstTransversal)}: {currentNode}");
            var currentNodeNeigbours = graph[currentNode];
            if(currentNodeNeigbours == null) continue;

            foreach(var neighbour in currentNodeNeigbours)
            {
                queue.Enqueue(neighbour);
            }
        }
    }

    public static void DepthFirstTransversal(Dictionary<string, List<string>?>  graph, string start)
    {
        var stack = new Stack<string>();
        stack.Push(start);
        while(stack.Any())
        {
            var currentNode = stack.Pop();
            Console.WriteLine($"{nameof(DepthFirstTransversal)}: {currentNode}");
            var currentNodeNeigbours = graph[currentNode];
            if(currentNodeNeigbours == null) continue;

            foreach(var neighbour in currentNodeNeigbours)
            {
                stack.Push(neighbour);
            }
        }
    }

    /// <summary>
    /// Write a function,has path that takes in an object representing the adjacency list of a directed acyclic graph and two nodes
    /// (src,dest). The function should return a boolean indicating whether or not there exist a path between the source and destination nodes.
    /// </summary>
    public static bool DepthFirstHasPath(Dictionary<string, List<string>?> graph, string src, string dest)
    {
        if(src == dest ) return true;
        if(graph[src] == null || !graph[src].Any()) return false;

        foreach(var neighbour in graph[src]!)
        {
            if(DepthFirstHasPath(graph, neighbour, dest))
                return true;
        }
        return false;
    }

    /// <summary>
    /// Write a function,has path that takes in an object representing the adjacency list of a directed acyclic graph and two nodes
    /// (src,dest). The function should return a boolean indicating whether or not there exist a path between the source and destination nodes.
    /// </summary>
    public static bool BreadthFirstHasPath(Dictionary<string, List<string>?> graph, string src, string dest)
    {
        var queue = new Queue<string> (){};
        queue.Enqueue(src);

        while(queue.Any())
        {
            var currentNode = queue.Dequeue();

            if(currentNode == dest) return true;

            if(graph[currentNode] is null || !graph[currentNode].Any()) continue;

            foreach(var neighbour in graph[currentNode]!)
            {
                queue.Enqueue(neighbour);
            }
        }

        return false;
    }

    /// <summary>
    /// Write a function undirectedPath that takes an array of edges for an undirected graph and two nodes (nodeA, nodeB).
    /// The function should return a boolean that indicates whether or not there exist a path between nodeA and nodeB.
    /// Array of edges example = [[i,j],[j,k],[j,l]]
    /// </summary>
    public static bool BreadFirstUndirectedPath(List<List<string>> arrayOfEdges, string nodeA , string nodeB)
    {
        var undirectedGraph = ConvertArrayOfEdgesToGraph(arrayOfEdges);

        var visitedSet = new HashSet<string>();

        return HasPath(undirectedGraph,nodeA, nodeB, visitedSet);
    }

    /// <summary>
    /// A set of nodes forms a connected component in an undirected graph if any node from the set of nodes can reach any other node by traversing edges.
    /// Write a function ConnectedComponentsCount that takes the adjacency list of an undirected graph. The function should return the number of connected components
    /// withing the graph
    /// </summary>
    /// <returns></returns>
    public static int GetConnectedComponentsCount(Dictionary<string, List<string>?> graph)
    {
        var connectedComponentCount = 0;
        var visitedNodes = new HashSet<string> ();
        foreach(var node in graph.Keys)
        {
            if(visitedNodes.Any(x => x.Equals(node)))
                continue;

            TransverseConnectedComponents(graph,node,visitedNodes);
            connectedComponentCount++;
        }

        return connectedComponentCount;
    }

    /// <summary>
    /// Write a function largestComponent that takes an adjacency list of an undirected graph. The function should return the size of 
    /// the largest connected components in the graph
    /// </summary>
    /// <param name="graph"></param>
    /// <returns></returns>
    public static int GetLargestComponent(Dictionary<string, List<string>?> graph)
    {
        var largestSize = 0;
        var visitedNodes = new HashSet<string>();

        foreach(var node in graph.Keys)
        {
            if(visitedNodes.Any(x => x.Equals(node)))
                continue;

            var connectedComponentsNodeCount = CountNodesInConnectedComponent(graph,node,visitedNodes);

            largestSize = largestSize > connectedComponentsNodeCount ? largestSize : connectedComponentsNodeCount;
        };

        return largestSize;
    }


    private static int CountNodesInConnectedComponent (Dictionary<string, List<string>?> graph, string startNode, HashSet<string> visitedNodes, int count = 1)
    {
        if(visitedNodes.TryGetValue(startNode,out string val))
            return 0;
            
        visitedNodes.Add(startNode);
        if(graph[startNode] == null) return count;

        foreach(var edge in graph[startNode]!)
        {
            count += CountNodesInConnectedComponent(graph,edge,visitedNodes, 1);
        }

        return count;
    }

    private static void TransverseConnectedComponents (Dictionary<string, List<string>?> graph, string startNode, HashSet<string> visitedNodes)
    {
        if(!visitedNodes.TryGetValue(startNode,out string val))
            visitedNodes.Add(startNode);

        if(graph[startNode] == null) return;

        foreach(var edge in graph[startNode])
        {
            TransverseConnectedComponents(graph,edge,visitedNodes);
        }
    }

    private static bool HasPath(Dictionary<string, List<string>?> graph, string src, string dest, HashSet<string> visitedNodes)
    {
        if(visitedNodes.TryGetValue(src,out string val)) return false;

        visitedNodes.Add(src);

        if(src == dest ) return true;

        if(graph[src] == null || !graph[src].Any()) return false;

        foreach(var neighbour in graph[src]!)
        {
            if(HasPath(graph, neighbour, dest,visitedNodes))
                return true;
        }
        return false;
    }

    private static Dictionary<string, List<string>?> ConvertArrayOfEdgesToGraph(List<List<string>> arrayOfEdges)
    {
        var adjacencyList = new  Dictionary<string, List<string>?>();
        foreach(var edges in arrayOfEdges)
        {
            foreach(var edge in edges)
            {
                if(!adjacencyList.TryGetValue(edge, out List<string> value))
                    adjacencyList[edge] = new List<string>();

                adjacencyList[edge]!.AddRange(edges.Where( x=> x != edge));
            }
        }
        return adjacencyList;
    }

    public static void TestGraphAlgo()
    {
        BreadthFirstTransversal(ExampleDirectedGraph, "a");
        DepthFirstTransversal(ExampleDirectedGraph, "a");
        var depthFirstHasPath = DepthFirstHasPath(ExampleDirectedGraph, "a","j");
        var breadthFirstHasPath = BreadthFirstHasPath(ExampleDirectedGraph, "a","j");
        Console.WriteLine($"depthFirstHasPath => Expected:True; Actual:{depthFirstHasPath}");
        Console.WriteLine($"breadthFirstHasPath => Expected:True; Actual:{breadthFirstHasPath}");

        var pathExist = BreadFirstUndirectedPath(ExampleArrayOfEdges, "i","m");
        Console.WriteLine($"BreadFirstUndirectedPath => Expected:True; Actual:{pathExist}");

        pathExist = BreadFirstUndirectedPath(ExampleArrayOfEdges, "i","o");
        Console.WriteLine($"BreadFirstUndirectedPath => Expected:False; Actual:{pathExist}");

        var connectedComponentCount = GetConnectedComponentsCount(ExampleDirectedGraph);
        Console.WriteLine($"GetConnectedComponentsCount => Expected: 2; Actual:{connectedComponentCount}");

        var largestComponentSize = GetLargestComponent(ExampleUnDirectedGraph);
        Console.WriteLine($"GetLargestComponent => Expected:6, Actual:{largestComponentSize}");

    }
}