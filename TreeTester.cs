using TreeLibrary.Trees;
using TreeLibrary.Utilities;

namespace TreeLibrary;

public static class TreeTester
{
    /// <summary>
    /// Метод запускает полный набор тестов для конкретного дерева.
    /// </summary>
    public static void RunTests<T>(ITree<T> tree, IEnumerable<T> sampleData) where T : IComparable<T>, new()
    {
        Console.ForegroundColor = ConsoleColor.Yellow;
        Console.WriteLine(new string('=', 60));
        Console.Write($"Запуск тестов для дерева: ");
        Console.ForegroundColor = ConsoleColor.Cyan;
        Console.WriteLine($"{tree.GetType().Name}");
        Console.ForegroundColor = ConsoleColor.Yellow;
        Console.WriteLine(new string('=', 60));
        Console.ForegroundColor = ConsoleColor.Gray;
        Console.WriteLine();

        // 1. Добавление элементов
        foreach (var item in sampleData)
        {
            ExecuteCommand(tree, $"Add({item})", t => t.Add(item));
        }

        // 2. Проверка Contains
        foreach (var item in sampleData)
        {
            ExecuteCommand(tree, $"Contains({item})", t =>
            {
                var exists = t.Contains(item);
                Console.WriteLine($"Contains({item}) = {exists}");
            });
        }

        var testItem = new T();
        ExecuteCommand(tree, $"Contains({testItem})", t =>
        {
            var exists = t.Contains(testItem);
            Console.WriteLine($"Contains({testItem}) = {exists}");
        });

        // 3. Удаление элементов
        if (sampleData is IList<T> list)
        {
            ExecuteCommand(tree, $"Remove({list[0]})", t => t.Remove(list[0]));
            ExecuteCommand(tree, $"Remove({testItem})", t => t.Remove(testItem));
        }

        // 4. Проверка утилит TreeUtils
        ExecuteCommand(tree, "TreeUtils.Exists(x => x.CompareTo(testItem) > 0)", t =>
        {
            var any = TreeUtils<T>.Exists(t, x => x.CompareTo(testItem) > 0);
            Console.WriteLine($"TreeUtils.Exists = {any}");
        });

        ExecuteCommand(tree, "TreeUtils.CheckForAll(x => x.CompareTo(testItem) > 0)", t =>
        {
            var all = TreeUtils<T>.CheckForAll(t, x => x.CompareTo(testItem) > 0);
            Console.WriteLine($"TreeUtils.CheckForAll = {all}");
        });

        ExecuteCommand(tree, "TreeUtils.FindAll(x => x.CompareTo(testItem) > 0)", t =>
        {
            var filtered = TreeUtils<T>.FindAll(t, x => x.CompareTo(testItem) > 0);
            Console.WriteLine("TreeUtils.FindAll создало дерево:");
            PrintTree(filtered);
        });

        ExecuteCommand(tree, "TreeUtils.ForEach(x => if (x.CompareTo(testItem) > 0))",
            t =>
            {
                TreeUtils<T>.ForEach(t, x =>
                {
                    if (x.CompareTo(testItem) > 0)
                        Console.WriteLine($"Элемент: {x} больше {testItem}");
                });
            });

        // 5. Очистка дерева
        ExecuteCommand(tree, "Clear", t => t.Clear());
    }

    /// <summary>
    /// Метод для тестирования неизменяемого дерева на основе исходного.
    /// </summary>
    public static void RunTestsWithImmutable<T>(ITree<T> baseTree) where T : IComparable<T>, new()
    {
        Console.ForegroundColor = ConsoleColor.DarkMagenta;
        Console.WriteLine(new string('=', 60));
        Console.WriteLine("Тестирование неизменяемого дерева (UnmutableTree)");
        Console.WriteLine(new string('=', 60));
        Console.WriteLine();
        Console.ForegroundColor = ConsoleColor.Gray;

        var immutableTree = new UnmutableTree<T>(baseTree);
        RunTests(immutableTree, baseTree.Nodes);
    }

    /// <summary>
    /// Метод выполняет команду над деревом, логируя процесс и результат.
    /// </summary>
    private static void ExecuteCommand<T>(ITree<T> tree, string commandName, Action<ITree<T>> action)
        where T : IComparable<T>
    {
        Console.WriteLine(new string('-', 60));
        Console.Write($"Попытка выполнить команду: ");
        Console.ForegroundColor = ConsoleColor.White;
        Console.WriteLine($"{commandName}");
        Console.ForegroundColor = ConsoleColor.Gray;
        Console.Write($"Тип дерева: ");
        Console.ForegroundColor = ConsoleColor.Cyan;
        Console.WriteLine($"{tree.GetType().Name}");
        Console.ForegroundColor = ConsoleColor.Gray;
        Console.WriteLine("Состояние дерева ДО выполнения:");
        PrintTree(tree);

        try
        {
            action(tree);
            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine($"Команда '{commandName}' выполнена успешно!");
        }
        catch (Exception ex)
        {
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine($"Обработана ошибка при выполнении '{commandName}': {ex.Message}");
        }

        Console.ForegroundColor = ConsoleColor.Gray;
        Console.WriteLine("Состояние дерева ПОСЛЕ выполнения:");
        PrintTree(tree);
        Console.WriteLine(new string('-', 60));
        Console.WriteLine();
    }

    /// <summary>
    /// Печатает элементы дерева.
    /// </summary>
    private static void PrintTree<T>(ITree<T> tree) where T : IComparable<T>
    {
        if (tree.IsEmpty)
        {
            Console.WriteLine($"[Дерево пустое]: {tree.IsEmpty}");
            return;
        }

        Console.WriteLine($"Элементы ({tree.Count} шт.): {string.Join(", ", tree.Nodes)}");
    }
}