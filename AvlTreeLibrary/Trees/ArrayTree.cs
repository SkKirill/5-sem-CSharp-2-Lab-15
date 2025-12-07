using System.Collections;
using TreeLibrary.Exceptions;
using TreeLibrary.Trees;

namespace AvlTreeLibrary.Trees;

// Бинарное дерево поиска, реализованное с использованием массива (как структура кучи)
// Правила:
// • корень = индекс 0
// • левый потомок = 2*i + 1
// • правый потомок = 2*i + 2
// • левый < корень < правый
// Размер массива увеличивается автоматически путём удвоения
public class ArrayTree<T> : ITree<T> where T : IComparable<T>
{
    private T[] _array = new T[4];
    private bool[] _arrayIsValue = new bool[4];
    public int Count { get; private set; }
    public bool IsEmpty => Count == 0;

    public IEnumerable<T> Nodes
    {
        get
        {
            var list = new List<T>();
            for (var index = 0; index < _array.Length; index++)
            {
                var node = _array[index];
                if (_arrayIsValue[index])
                {
                    list.Add(node);
                }
            }

            return list;
        }
    }

    public void Add(T item)
    {
        if (item == null)
            throw new TreeNullException();

        // Если нет корня
        if (Count == 0)
        {
            _array[0] = item;
            _arrayIsValue[0] = true;
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
                    if (!_arrayIsValue[left])
                    {
                        _array[left] = item;
                        _arrayIsValue[left] = true;
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
                    if (!_arrayIsValue[right])
                    {
                        _array[right] = item;
                        _arrayIsValue[right] = true;
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
        _arrayIsValue = new bool[4];
        Count = 0;
    }

    public bool Contains(T item)
    {
        if (item == null)
            throw new TreeNullException();

        var index = 0;
        while (index < _array.Length)
        {
            var cmp = item.CompareTo(_array[index]);
            if (!_arrayIsValue[index])
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
                    _arrayIsValue[index] = false;
                    Count--;
                    var rebase = new List<T>();
                    CutBranch(ref rebase, index);
                    foreach (var node in rebase)
                    {
                        Add(node);
                    }

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
        for (var index = 0; index < _array.Length; index++)
        {
            if (_arrayIsValue[index])
                continue;

            yield return _array[index];
        }
    }

    IEnumerator IEnumerable.GetEnumerator()
    {
        return GetEnumerator();
    }

    private void CutBranch(ref List<T> list, int index)
    {
        if (2 * index + 1 < Count && _arrayIsValue[2 * index + 1])
        {
            list.Add(_array[2 * index + 1]);
            _arrayIsValue[2 * index + 1] = false;
            CutBranch(ref list, 2 * index + 1);
        }

        if (2 * index + 2 < Count && _arrayIsValue[2 * index + 1])
        {
            list.Add(_array[2 * index + 2]!);
            _arrayIsValue[2 * index + 2] = false;
            CutBranch(ref list, 2 * index + 2);
        }
    }

    private void EnsureCapacity(int index)
    {
        if (index < _array.Length)
            return;

        var newSize = _array.Length;
        while (newSize <= index)
            newSize *= 2;

        Array.Resize(ref _array, newSize);
        Array.Resize(ref _arrayIsValue, newSize);
    }
}