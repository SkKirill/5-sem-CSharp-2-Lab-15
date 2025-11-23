namespace TreeLibrary.Exceptions;

public class TreeException(string message = "Произошла ошибка при работе с деревом.") : Exception(message);

public class TreeNullException(string message = "Дерево не может быть null.") : TreeException(message);

public class TreeDelegateNullException(string message = "Делегат не может быть null.") : TreeException(message);

public class TreeTypeUnsupportedException(string message = "Тип дерева не поддерживается.") : TreeException(message);

public class TreeItemNotFoundException(string message = "Указанный элемент не найден в дереве.")
    : TreeException(message);

public class TreeInvalidOperationException(string message = "Некорректная операция над деревом.")
    : TreeException(message);

public class TreeDuplicateValueException(
    string message = "Нельзя вставлять в бинарное дерево значение, которое в нем уже есть")
    : TreeException(message);