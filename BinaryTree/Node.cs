using System.Text.RegularExpressions;

namespace AlgoApp.BinaryTree;

public class BinaryTreeNode<T>
{
    public BinaryTreeNode(T value)
    {
        this.Value = value;
    }

    public BinaryTreeNode(BinaryTreeNode<T> left,BinaryTreeNode<T> right)
    {
        this.Left = left;
        this.Right = right;
    }

    public BinaryTreeNode(T value, BinaryTreeNode<T> left, BinaryTreeNode<T> right)
    {
        this.Value = value;
        this.Left = left;
        this.Right = right;
    }

    public BinaryTreeNode<T>? Left {get;set;}

    public BinaryTreeNode<T>? Right {get;set;}

    public T Value {get;set;}
    public int Id {get;set;}
    
}


public class BTreeNodesEntity 
{
    public string Value {get;set;}
    public int TreeId {get;set;}
    public int LeftId {get;set;}
    public int RightId {get;set;}
    public bool IsRootNode {get;set;}
    public int Id {get;set;}
}

public class BTreeEntity{
    public string Name {get;set;}
    public int Id {get;set;}
    public int Height {get;set;}

    public static BinaryTreeNode<string> GetBTree(List<BTreeNodesEntity> bTreeNodesEntities)
    {
        var rootBTreeEntity = bTreeNodesEntities.FirstOrDefault(x => x.IsRootNode);

        if(rootBTreeEntity == null ) throw new Exception ("Binary tree does not have a root node");


        var rootNode = new BinaryTreeNode<string>(rootBTreeEntity.Value);
        rootNode.Id = rootBTreeEntity.Id;
        
        var stack = new Stack<BinaryTreeNode<string>> ();
        stack.Push(rootNode);
        
        while(true)
        {
            var currentNode = stack.Pop();
            var currentEntity =  bTreeNodesEntities.First(x => x.Id ==  currentNode.Id);

            var leftChild = bTreeNodesEntities.FirstOrDefault(x => x.Id == currentEntity.LeftId);
            var rightChild = bTreeNodesEntities.FirstOrDefault(x => x.Id == currentEntity.RightId);
            var leftNode = leftChild == null ? null : new BinaryTreeNode<string> (leftChild.Value){Id = currentEntity.LeftId};
            var RightNode = rightChild == null ? null : new BinaryTreeNode<string> (rightChild.Value) {Id = currentEntity.RightId};

            currentNode.Right = RightNode;
            currentNode.Left = leftNode;

            if(leftNode != null) stack.Push(leftNode);
            if(RightNode != null) stack.Push(RightNode);

            if(!stack.Any()) break;
        }
        
        return rootNode;
    }
}
