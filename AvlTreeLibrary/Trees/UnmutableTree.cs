using System.Collections;
using AvlTreeData.Services;
using AvlTreeData.Trees;
using AvlTreeData.Views;
using TreeLibrary.Exceptions;

namespace AvlTreeLibrary.Trees;

public class UnmutableTree<T> : ITree<T> where T : IComparable<T>
{
    private readonly ITree<T> _tree;
    
    public int Count => _tree.Count;
    public bool IsEmpty => _tree.IsEmpty;
    public IEnumerable<T> Nodes => _tree.Nodes;

    public UnmutableTree(ITree<T> tree)
    {
        _tree = tree;
    }

    public void Add(T node)
    {
        throw new TreeUnmutableException();
    }

    public void Clear()
    {
        throw new TreeUnmutableException();
    }

    public bool Contains(T node)
    {
        return _tree.Contains(node);
    }

    public bool Contains(ITree<T> tree1)
    {
        return _tree.Contains(tree1);
    }

    public void Remove(T node)
    {
        throw new TreeUnmutableException();
    }

    public IEnumerator<T> GetEnumerator()
    {
        return _tree.GetEnumerator();
    }
    IEnumerator IEnumerable.GetEnumerator()
    {
        return GetEnumerator();
    }

    public VisualNode<T>? ConvertToVisualNode()
    {
        return _tree.ConvertToVisualNode();
    }
}