namespace AlgoApp.BinaryTree;

public class BTreeAlgo
{
    public static BinaryTreeNode<int> GetSampleBTreeIntRootNode()
    {
        var ddd = new BinaryTreeNode<int>(4);
        var eee = new BinaryTreeNode<int>(5);
        var fff = new BinaryTreeNode<int>(6);
        var bbb = new BinaryTreeNode<int>(2, ddd, eee);
        var ccc = new BinaryTreeNode<int>(3, null, fff);
        var aaa = new BinaryTreeNode<int>(1, bbb, ccc);
        return aaa;
    }

    public static BinaryTreeNode<string> GetSampleBTreeStringtRootNode()
    {
        /*
                        a
                    b       c
                  d   e       f
        **/
        var d = new BinaryTreeNode<string>("d");
        var e = new BinaryTreeNode<string>("e");
        var f = new BinaryTreeNode<string>("f");
        var b = new BinaryTreeNode<string>("b", d, e);
        var c = new BinaryTreeNode<string>("c", null, f);
        var a = new BinaryTreeNode<string>("a", b, c);
        return a;
    }

    public static BTreeNodesEntity GetSampleBTreeEntity()
    {
        var btree = new BTreeEntity
        {
            Id = 1,
            Name = "FIRST",
            Height = 3
        };
        var dd = new BTreeNodesEntity
        {
            Value = "dd",
            IsRootNode = false,
            TreeId = btree.Id,
            Id = 4
        };
        var ee = new BTreeNodesEntity
        {
            Value = "ee",
            IsRootNode = false,
            TreeId = btree.Id,
            Id = 5
        };
        var bb = new BTreeNodesEntity
        {
            Value = "bb",
            IsRootNode = false,
            TreeId = btree.Id,
            LeftId = dd.Id,
            RightId = ee.Id,
            Id = 2
        };
        var cc = new BTreeNodesEntity
        {
            Value = "cc",
            IsRootNode = false,
            TreeId = btree.Id,
            Id = 3
        };
        var ff = new BTreeNodesEntity
        {
            Value = "ff",
            IsRootNode = false,
            TreeId = btree.Id,
            Id = 6
        };
        cc.RightId = ff.Id;
        var aa = new BTreeNodesEntity
        {
            Value = "aa",
            IsRootNode = true,
            TreeId = btree.Id,
            Id = 1,
            LeftId = bb.Id,
            RightId = cc.Id
        };
        return aa;
    }

    public static void DepthFirstSearch(BinaryTreeNode<string>? node)
    {
        if (node == null)
            return;

        Console.WriteLine(node.Value);

        if (node.Left != null)
            DepthFirstSearch(node.Left);

        if (node.Right != null)
            DepthFirstSearch(node.Right);
    }

    public static List<string> DepthFirstSearch(BinaryTreeNode<string>? node, List<string> container)
    {
        if (node == null)
            return container;

        container.Add(node.Value);

        if (node.Left != null)
            DepthFirstSearch(node.Left, container);

        if (node.Right != null)
            DepthFirstSearch(node.Right, container);

        return container;
    }

    public static void BreadFirstSearch(BinaryTreeNode<string>? node)
    {
        Queue<BinaryTreeNode<string>> queue = new Queue<BinaryTreeNode<string>>();
        if (node == null)
            return;

        queue.Enqueue(node);

        while (queue.Any())
        {
            var currentItem = queue.Dequeue();
            Console.WriteLine(currentItem.Value);

            if (currentItem.Left != null)
                queue.Enqueue(currentItem.Left);
            if (currentItem.Right != null)
                queue.Enqueue(currentItem.Right);
        }
    }

    public static int MaxSumPath(BinaryTreeNode<int>? node)
    {
        if(node == null) return int.MinValue;

        if(node.Left == null && node.Right == null) return node.Value;

        return node.Value + Math.Max(MaxSumPath(node.Left), MaxSumPath(node.Right));
    }
}
