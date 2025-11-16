using Tree.Exceptions;
using Tree.Tree;

namespace Tree;

/// <summary>
/// Делегат проверки элемента дерева.
/// Возвращает true, если элемент удовлетворяет условию.
/// </summary>
/// <typeparam name="T">Тип хранимых данных.</typeparam>
public delegate bool CheckDelegate<in T>(T item);

/// <summary>
/// Делегат, выполняющий действие над элементом дерева.
/// </summary>
/// <typeparam name="T">Тип хранимых данных.</typeparam>
public delegate void ActionDelegate<in T>(T item);

/// <summary>
/// Делегат, создающий новый экземпляр дерева.
/// </summary>
/// <typeparam name="T">Тип элементов дерева.</typeparam>
public delegate ITree<T> TreeConstructorDelegate<T>();

/// <summary>
/// Набор вспомогательных операций над деревьями.
/// Содержит методы поиска, фильтрации и обхода.
/// </summary>
public static class TreeUtils<T>
{
    /// <summary>
    /// Конструктор для массива-дерева.
    /// </summary>
    private static readonly TreeConstructorDelegate<T> ArrayTreeConstructor = () => new ArrayTree<T>();

    /// <summary>
    /// Конструктор для связанного дерева.
    /// </summary>
    private static readonly TreeConstructorDelegate<T> LinkedTreeConstructor = () => new LinkedTree<T>();

    // ========================== ОТКРЫТЫЕ МЕТОДЫ ==============================

    /// <summary>
    /// Проверяет, существует ли в дереве элемент,
    /// удовлетворяющий проверке check.
    /// </summary>
    public static bool Exists(ITree<T> tree, CheckDelegate<T> check)
    {
        ValidateTree(tree);
        ValidateDelegate(check, nameof(check));

        return tree.Any(item => check(item));
    }

    /// <summary>
    /// Возвращает новое дерево, содержащее только те элементы,
    /// которые удовлетворяют проверке check.
    /// </summary>
    public static ITree<T> FindAll(ITree<T> tree, CheckDelegate<T> check)
    {
        ValidateTree(tree);
        ValidateDelegate(check, nameof(check));

        // Получаем нужный конструктор и создаем дерево.
        var filteredTree = GetConstructorForTree(tree).Invoke();

        // Заполняем коллекцию
        foreach (var item in filteredTree)
        {
            if (check(item))
                filteredTree.Add(item);
        }

        return filteredTree;
    }

    /// <summary>
    /// Выполняет действие для каждого элемента дерева.
    /// </summary>
    public static void ForEach(ITree<T> tree, ActionDelegate<T> action)
    {
        ValidateTree(tree);
        ValidateDelegate(action, nameof(action));

        foreach (var item in tree)
            action(item);
    }

    /// <summary>
    /// Проверяет, что ВСЕ элементы дерева удовлетворяют check.
    /// </summary>
    public static bool CheckForAll(ITree<T> tree, CheckDelegate<T> check)
    {
        ValidateTree(tree);
        ValidateDelegate(check, nameof(check));

        return tree.All(item => check(item));
    }

    // ========================== ВНУТРЕННИЕ МЕТОДЫ ===========================

    /// <summary>
    /// Проверяет корректность объекта дерева.
    /// </summary>
    private static void ValidateTree(ITree<T> tree)
    {
        if (tree == null)
            throw new TreeNullException("Передано null вместо дерева.");
    }

    /// <summary>
    /// Проверяет корректность делегатов.
    /// </summary>
    private static void ValidateDelegate(object del, string name)
    {
        if (del == null)
            throw new TreeDelegateNullException($"Делегат '{name}' равен null.");
    }

    /// <summary>
    /// Определяет подходящий конструктор нового дерева на основе типа исходного.
    /// </summary>
    private static TreeConstructorDelegate<T> GetConstructorForTree(ITree<T> tree)
    {
        return tree switch
        {
            ArrayTree<T> => ArrayTreeConstructor,
            LinkedTree<T> => LinkedTreeConstructor,
            _ => throw new TreeTypeUnsupportedException(
                $"Тип дерева '{tree.GetType().Name}' не поддерживается.")
        };
    }
}