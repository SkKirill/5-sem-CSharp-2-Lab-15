using System.Text;
using TreeLibrary.Trees;

namespace TreeLibrary;

internal static class Program
{
    public static void Main(string[] args)
    {
        Console.InputEncoding = new UTF8Encoding();
        Console.OutputEncoding = new UTF8Encoding();

        var sampleData = new List<IntClassTester> { new(1), new(2), new(3), new(4), new(5) };

        // Тест ArrayTree
        var arrayTree = new ArrayTree<IntClassTester>();
        TreeTester.RunTests(arrayTree, sampleData);
        arrayTree = new ArrayTree<IntClassTester> { new(1), new(2), new(3), new(4), new(5) };
        TreeTester.RunTestsWithImmutable(arrayTree);

        // Тест LinkedTree
        var linkedTree = new LinkedTree<IntClassTester>();
        TreeTester.RunTests(linkedTree, sampleData);
        linkedTree = new LinkedTree<IntClassTester> { new(1), new(2), new(3), new(4), new(5) };
        TreeTester.RunTestsWithImmutable(linkedTree);
    }
}