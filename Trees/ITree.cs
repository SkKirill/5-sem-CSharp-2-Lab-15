namespace TreeLibrary.Trees;

public interface ITree<T> : IEnumerable<T>
{
    int Count { get; }
    bool IsEmpty { get; }
    IEnumerable<T> Nodes { get; set; }

    void Add(T item);
    void Clear();
    bool Contains(T item);
    bool Contains(ITree<T> tree);
    void Remove(T item);
}