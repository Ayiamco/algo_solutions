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
        new() {"i", "j"},
        new() {"k", "i"},
        new() {"m", "k"},
        new() {"k", "l"},
        new() {"o", "n"},
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

    /// <summary>
    /// Write a function shortestPath that takes in an array of edges for an undirected graph and two nodes (nodeA,nodeB).
    /// The function should return the length of the shortest path between A and B. Consider the length as the number of edges in the path not the number of nodes.
    /// if there is no path between A and B return -1.
    /// </summary>
    /// <param name="graph"></param>
    /// <param name="src"></param>
    /// <param name="dest"></param>
    /// <returns></returns>
    public static int GetShortestPath(List<List<string>> edges, string src, string dest)
    {
        var graph = ConvertArrayOfEdgesToGraph(edges);
        var queue = new Queue<string>();
        queue.Enqueue(src);
        var distanceMap = new Dictionary<string, int>
        {
            { src, 0 }
        };
        var visitedNodes = new HashSet<string>();

        while(queue.Any())
        {
            var currentNode = queue.Dequeue();
            visitedNodes.Add(currentNode);
            var currentNodeDistanceFromSrc = distanceMap[currentNode];

            if(currentNode == dest )
                return currentNodeDistanceFromSrc;

            if(graph[currentNode] == null) continue;

            foreach(var neighbour in graph[currentNode]!)
            {
                if(visitedNodes.Contains(neighbour)) 
                    continue;

                queue.Enqueue(neighbour);
                distanceMap.Add(neighbour, currentNodeDistanceFromSrc + 1);
            }
        }

        return -1;
    }

    /// <summary>
    /// Given an m x n 2D binary grid grid which represents a map of '1's (land) and '0's (water), return the number of islands.
    /// An island is surrounded by water and is formed by connecting adjacent lands horizontally or vertically. 
    /// You may assume all four edges of the grid are all surrounded by water.
    /// Example 1:
    /// Input: grid = 
    ///  [
    ///     ["1","1","1","1","0"],
    ///     ["1","1","0","1","0"],
    ///     ["1","1","0","0","0"],
    ///     ["0","0","0","0","0"]
    ///  ]
    /// Output: 1
    /// </summary>
    /// <param name="matrixGraph"></param>
    /// <returns></returns>

    public static int GetIslandCount (List<List<int>> matrixGraph)
    {
        var visitedNodes = new List<string>();
        var islandCount = 0;
        for(var y = 0;  y <  matrixGraph.Count(); y++ )
        {
            for(var x = 0; x<  matrixGraph[y].Count(); x++)
            {
                var hasIsland = Transverse2DMaxtrix(matrixGraph,y,x,visitedNodes);
                if(hasIsland) islandCount++;
            }
        }
        return islandCount;
    }

    /// <summary>
    /// Given an m x n 2D binary grid grid which represents a map of 'L's (land) and 'W's (water), return the number of islands.
    /// An island is surrounded by water and is formed by connecting adjacent lands horizontally or vertically. 
    /// You may assume all four edges of the grid are all surrounded by water.
    /// Example 1:
    /// Input: grid = 
    ///  [
    ///     ["L","L","L","L","W"],
    ///     ["L","L","W","L","W"],
    ///     ["L","L","W","W","W"],
    ///     ["W","W","W","W","W"]
    ///  ]
    /// Output: 1
    /// </summary>
    /// <param name="matrixGraph"></param>
    /// <returns></returns>

    public static int GetIslandCount(List<List<string>> matrixGraph)
	{
		var count = 0;
		var rowCount = matrixGraph.Count;
		var columnCount = matrixGraph[0].Count;
		var visitedNodes = new HashSet<string> {};
		for(var r = 0; r < rowCount; r++)
		{
			for(var c = 0; c < columnCount; c++)
			{
				var currentNode = matrixGraph[r][c];
				if(currentNode == "W" || visitedNodes.Contains($"{r},{c}")) 
					continue;
				
				count++;
				TransverseLand(r,c,matrixGraph,visitedNodes);
			}
		}
		return count;
	}
	
	public static void TransverseLand(int row, int column,List<List<string>> matrixGraph, HashSet<string> visitedNodes)
	{
		
		if(visitedNodes.Contains($"{row},{column}"))
		   return;

        visitedNodes.Add($"{row},{column}");
		   
		if(row - 1 >= 0 &&  matrixGraph[row - 1][column] == "L" )
			TransverseLand(row - 1, column, matrixGraph, visitedNodes);

		if(row + 1 < matrixGraph.Count &&  matrixGraph[row + 1][column] == "L" )
			TransverseLand(row + 1, column, matrixGraph, visitedNodes);
		
		if(column - 1 >= 0 &&  matrixGraph[row][column - 1] == "L" )
			TransverseLand(row, column - 1, matrixGraph, visitedNodes);
		
		if(column + 1 < matrixGraph[0].Count &&  matrixGraph[row][column + 1] == "L" )
			TransverseLand(row, column + 1, matrixGraph, visitedNodes);
	} 

    private static bool Transverse2DMaxtrix(List<List<int>> matrixGraph,int y,int x, List<string> visitedNodes)
    {
        var maxY = matrixGraph.Count() - 1;
        var maxX = matrixGraph[0].Count() - 1;
        var neighboursMap = new Dictionary<int, Func<int, int,int, (int y,int x )>>
        {
            {1, (int index, int y, int x)=> (y+1,x) },
            {2, (int index, int y, int x)=> (y-1,x) },
            {3, (int index, int y, int x)=> (y,x+1) },
            {4, (int index, int y, int x)=> (y,x-1) },
        };

        var queue = new Queue<string>();
        queue.Enqueue($"{x},{y}");

        var hasIsland = false;
        while (queue.Any())
        {
            var currentNode = queue.Dequeue();
            if(visitedNodes.Contains(currentNode))
                continue;

            visitedNodes.Add(currentNode);
            y = Convert.ToInt16(currentNode.Split(",")[1]);
            x = Convert.ToInt16(currentNode.Split(",")[0]);

            var currentNodeValue = matrixGraph[y][x];
            hasIsland = currentNodeValue == 1;
            
            for(var i = 1; i<= 4; i++)
            {
                var (yIndex,xIndex) = neighboursMap[i](i,y,x);
                if(yIndex < 0 || yIndex > maxY || xIndex < 0 || xIndex > maxX)
                    continue;

                var currentNeighbourValue = matrixGraph[yIndex][xIndex];
                if(currentNeighbourValue == currentNodeValue)
                    queue.Enqueue($"{xIndex},{yIndex}");
            }
        }

        return hasIsland;
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
        
        var shortestPath = GetShortestPath(ExampleArrayOfEdges,"i","l");
        Console.WriteLine($"GetLargestComponent => Expected:2, Actual:{shortestPath}");
        shortestPath = GetShortestPath(ExampleArrayOfEdges,"i","o");
        Console.WriteLine($"GetLargestComponent => Expected:-1, Actual:{shortestPath}");

        var islandCount = GetIslandCount(new List<List<int>> 
        {
            new() {0,0,1,0,1},
            new() {0,0,1,1,0},
            new() {0,1,0,0,0},
        });
        Console.WriteLine($"{nameof(GetIslandCount)} => Expected:3, Actual:{islandCount}");
        
        islandCount = GetIslandCount(new List<List<int>> 
        {
            new() {1,1,1,1,0},
            new() {1,1,0,1,0},
            new() {1,1,0,0,0},
            new() {0,0,0,0,0},
        });
        Console.WriteLine($"{nameof(GetIslandCount)} => Expected:1, Actual:{islandCount}");

        var islandCount2 = GetIslandCount(new List<List<string>> 
        {
            new() {"W","W","L","W","L"},
            new() {"W","W","L","L","W"},
            new() {"W","L","W","W","W"},
        });
        Console.WriteLine($"{nameof(GetIslandCount)} => Expected:3, Actual:{islandCount2}");

        islandCount2 = GetIslandCount(new List<List<string>> 
        {
            new() {"L","L","L","L","W"},
            new() {"L","L","W","L","W"},
            new() {"L","L","W","W","W"},
            new() {"W","W","W","W","W"},
        });

        Console.WriteLine($"{nameof(GetIslandCount)} => Expected:1, Actual:{islandCount}");

    }
}