using System.Collections;
using TreeLibrary.Exceptions;

namespace TreeLibrary.Trees;

// Бинарное дерево поиска, реализованное с использованием массива (как структура кучи)
// Правила:
// • корень = индекс 0
// • левый потомок = 2*i + 1
// • правый потомок = 2*i + 2
// • левый < корень < правый
// Размер массива увеличивается автоматически путём удвоения
public class ArrayTree<T> : ITree<T> where T : IComparable<T>
{
    private T?[] _array = new T?[4];
    public int Count { get; private set; }
    public bool IsEmpty => Count == 0;
    public IEnumerable<T> Nodes => _array;

    public void Add(T item)
    {
        if (item == null)
            throw new TreeNullException();

        // Если нет корня
        if (Count == 0)
        {
            _array[0] = item;
            Count = 1;
            return;
        }

        var index = 0;
        while (true)
        {
            EnsureCapacity(index);
            var cmp = item.CompareTo(_array[index]);
            switch (cmp)
            {
                case < 0:
                {
                    var left = 2 * index + 1;
                    EnsureCapacity(left);
                    if (Equals(_array[left], null))
                    {
                        _array[left] = item;
                        Count++;
                        return;
                    }

                    index = left;
                    break;
                }
                case > 0:
                {
                    var right = 2 * index + 2;
                    EnsureCapacity(right);
                    if (Equals(_array[right], null))
                    {
                        _array[right] = item;
                        Count++;
                        return;
                    }

                    index = right;
                    break;
                }
                default:
                    throw new TreeDuplicateValueException();
            }
        }
    }

    public void Clear()
    {
        _array = new T[4];
        Count = 0;
    }

    public bool Contains(T item)
    {
        if (item == null)
            throw new TreeNullException();

        var index = 0;
        var cmp = item.CompareTo(_array[index]);
        while (index < _array.Length)
        {
            if (Equals(_array[index], null))
                return false;

            switch (cmp)
            {
                case 0:
                    return true;
                case < 0:
                    index = 2 * index + 1;
                    break;
                default:
                    index = 2 * index + 2;
                    break;
            }
        }

        return false;
    }

    public bool Contains(ITree<T> tree)
    {
        if (tree == null)
            throw new TreeNullException();


        foreach (var node in tree.Nodes)
        {
            if (!Equals(node, null) && !Contains(node))
                return false;
        }
        return true;
    }

    public void Remove(T item)
    {
        if (item == null)
            throw new TreeNullException();


        var index = 0;
        while (index < _array.Length)
        {
            var cmp = item.CompareTo(_array[index]);


            switch (cmp)
            {
                case 0:
                    _array[index] = default;
                    Count--;
                    return;
                case < 0:
                    index = 2 * index + 1;
                    break;
                default:
                    index = 2 * index + 2;
                    break;
            }
        }

        throw new TreeItemNotFoundException();
    }

    public IEnumerator<T> GetEnumerator()
    {
        foreach (var t in _array)
        {
            if (!Equals(t, null)) 
                continue;

            if (t != null)
                yield return t;
        }
    }

    IEnumerator IEnumerable.GetEnumerator()
    {
        return GetEnumerator();
    }

    private void EnsureCapacity(int index)
    {
        if (index < _array.Length)
            return;

        var newSize = _array.Length;
        while (newSize <= index)
            newSize *= 2;

        Array.Resize(ref _array, newSize);
    }
}