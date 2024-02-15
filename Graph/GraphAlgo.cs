namespace AlgoApp.Graph;

public static class GraphAlgo 
{
    static readonly Dictionary<string, List<string>?> ExampleGraph = new()
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
        if(graph[src] == null) return false;

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
            var currentEdge = queue.Dequeue();

            if(currentEdge == dest) return true;

            if(graph[currentEdge] is null) continue;

            foreach(var neighbour in graph[currentEdge]!)
            {
                queue.Enqueue(neighbour);
            }
        }

        return false;
    }

    public static void TestGraphAlgo()
    {
        BreadthFirstTransversal(ExampleGraph, "a");
        DepthFirstTransversal(ExampleGraph, "a");
        var depthFirstHasPath = DepthFirstHasPath(ExampleGraph, "a","j");
        var breadthFirstHasPath = BreadthFirstHasPath(ExampleGraph, "a","j");
        Console.WriteLine($"depthFirstHasPath => Expected:True; Actual:{depthFirstHasPath}");
        Console.WriteLine($"breadthFirstHasPath => Expected:True; Actual:{breadthFirstHasPath}");
    }
}