namespace TreeLibrary.Exceptions;

public class TreeException : Exception
{
    protected TreeException(string message = "Произошла ошибка при работе с деревом.") : base(message)
    {
    }
}

public class TreeNullException : TreeException
{
    public TreeNullException(string message = "Дерево не может быть null.") : base(message)
    {
    }
}

public class TreeDelegateNullException : TreeException
{
    public TreeDelegateNullException(string message = "Делегат не может быть null.") : base(message)
    {
    }
}

public class TreeTypeUnsupportedException : TreeException
{
    public TreeTypeUnsupportedException(string message = "Тип дерева не поддерживается.") : base(message)
    {
    }
}

public class TreeItemNotFoundException : TreeException
{
    public TreeItemNotFoundException(string message = "Указанный элемент не найден в дереве.") : base(message)
    {
    }
}

public class TreeDuplicateValueException : TreeException
{
    public TreeDuplicateValueException(
        string message = "Нельзя вставлять в бинарное дерево значение, которое в нем уже есть") : base(message)
    {
    }
}

public class TreeUnmutableException : TreeException
{
    public TreeUnmutableException(string message = "Функция недоступна.") : base(message)
    {
    }
}